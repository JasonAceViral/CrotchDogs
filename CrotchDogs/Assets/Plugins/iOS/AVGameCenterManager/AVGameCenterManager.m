//
//  GameCenterManager.m
//  FAViOS2
//
//  Created by SHAZAD MOHAMMED on 25/11/2011.
//  Copyright (c) 2011 ACE VIRAL. All rights reserved.
//

#import "AVGameCenterManager.h"
#import "AVDataManager.h"

#define kErrorCodeUserCancelled         2
#define kErrorCodeAlreadyAuthenticating 7

#define KEYS_FAILED_SCORES          @"AVGameCenterManagerFailedScores"
#define KEYS_FAILED_ACHIEVEMENTS    @"AVGameCenterManagerFailedAchievements"
#define KEYS_USER_ACHIEVEMENTS      @"AVGameCenterManagerUserAchievements"

static AVGameCenterManager* sharedInstance = nil;

#pragma mark -
#pragma mark Private Interface
#pragma mark -

@interface AVGameCenterManager (Private)

#pragma mark Failed Scores

-(void) saveFailedScores;
-(void) saveFailedScore:(NSNumber*) score forLeaderboard:(NSString*) leaderboardID;

-(void) saveFailedAchievements;
-(void) saveFailedPercentComplete:(float) percent forAchievement:(NSString*) achievementID DEPRECATED_ATTRIBUTE;
-(void) saveFailedProgressComplete:(float)progress forAchievement:(NSString *)achievementID;

#pragma mark Downloading Achievements

-(void) retrieveAchievmentMetadata;
-(void) retrieveAchievements;
@end

#pragma mark -
#pragma mark AVGameCenterManager Implementation
#pragma mark -

@implementation AVGameCenterManager
{
    bool availableCheckDone;
    bool gcIsAvailable;
}

#pragma mark -
#pragma mark Init
#pragma mark -

+(id) sharedManager
{
    @synchronized ([AVGameCenterManager class])
    {
        if (!sharedInstance) sharedInstance = [[AVGameCenterManager alloc] init];
        return sharedInstance;
    }
}

+(id) alloc
{
    NSAssert(sharedInstance == nil, @"Use shared instance AVGameCenterManager#sharedManager");
    return [super alloc];
}



- (id)init
{
    self = [super init];
    if (self != nil)
    {
        failedScores = [[[[NSUserDefaults standardUserDefaults] objectForKey:KEYS_FAILED_SCORES] mutableCopy] retain];
        
        if (!failedScores)
        {
            failedScores = [[NSMutableDictionary alloc] init];
            [self saveFailedScores];
        }
        
        failedAchievements = [[[[NSUserDefaults standardUserDefaults] objectForKey:KEYS_FAILED_ACHIEVEMENTS] mutableCopy] retain];
        
        if (!failedAchievements)
        {
            failedAchievements = [[NSMutableDictionary alloc] init];
            [self saveFailedAchievements];
        }
        
        achievementMetadata = [[NSMutableDictionary alloc] init];
        achievementProgress = [[NSMutableDictionary alloc] init];
        
        leaderboardDismissCompletionBlock = nil;
        
    }
    return self;
}

-(void)presentViewController:(UIViewController*)vc
{
    UIViewController* rootVC = [[AVDataManager sharedInstance] getViewController];
    [rootVC presentViewController:vc animated:YES completion:nil];
}

#pragma mark -
#pragma mark Availability
#pragma mark -

-(BOOL) isAvailableForDevice
{
    if(!availableCheckDone)
    {
        // Test if device is running iOS 4.1 or higher
        NSString* reqSysVer = @"4.1";
        
        if (versionNumber == nil) self.versionNumber = [[UIDevice currentDevice] systemVersion];
        
        BOOL isOSVer41 = ([versionNumber compare:reqSysVer options:NSNumericSearch] != NSOrderedAscending);
        
        gcIsAvailable = ( (NSClassFromString(@"GKLocalPlayer") != nil) && isOSVer41);
        availableCheckDone = true;
    }
    
    return gcIsAvailable;
}

-(BOOL) isAuthenticated
{
    if (![self isAvailableForDevice]) return NO;
    else return [GKLocalPlayer localPlayer].isAuthenticated;
}

#pragma mark -
#pragma mark Authentication
#pragma mark -

-(void) authenticate
{
    if (![self isAvailableForDevice])
    {
        UnitySendMessage("AVGameServicesInterface", "SignInComplete", "failure");
        return;
    }
	if ([GKLocalPlayer localPlayer].isAuthenticated)
    {
        UnitySendMessage("AVGameServicesInterface", "SignInComplete", "success");
        return;
	}
    
    
    // iOS6 and greater
    if([AVDataManager sharedInstance].isiOS6orHigher)
    {
        [GKLocalPlayer localPlayer].authenticateHandler = ^(UIViewController *viewController, NSError *error)
        {
            if([GKLocalPlayer localPlayer].authenticated)
                _gameCenterFeaturesEnabled = YES;
            
            // Present the view controller if needed
            if (![GKLocalPlayer localPlayer].authenticated)
            {
                if(viewController)
                    [self presentViewController:viewController];
                return;
            }
            
            if (error)
            {
                UnitySendMessage("AVGameServicesInterface", "SignInComplete", "failure");
            }
            else
            {
                [self authenticationCompleted];
                UnitySendMessage("AVGameServicesInterface", "SignInComplete", "success");
            }
        };
    }
    else
    // Less than iOS6
    {
        [[GKLocalPlayer localPlayer] authenticateWithCompletionHandler:^(NSError* error)
         {
             if (error)
             {
                 UnitySendMessage("AVGameServicesInterface", "SignInComplete", "failure");
             }
             else
             {
                 [self authenticationCompleted];
                 UnitySendMessage("AVGameServicesInterface", "SignInComplete", "success");
             }
         }];
    }
}

-(void) authenticationCompleted
{
    [self loadFriendData];
    [self retrieveAchievmentMetadata];
    [self retrieveAchievements];
}

#pragma mark -
#pragma mark Leaderboards
#pragma mark -

#pragma mark Display

-(void) showLeaderboards
{
    [self showLeaderboard:nil];
}

-(void) showLeaderboard:(NSString*) leaderboardID
{
    if (![self isAvailableForDevice]) return;
    if([[AVDataManager sharedInstance] getViewController] == NULL)
    {
        NSLog(@"**** View Controller in AVDataManager is not set! ****");
        return;
    }
    
    if (![GKLocalPlayer localPlayer].isAuthenticated)
    {
        //User has actively asked to see something to do with Game Center. Ask them to login again if they aren't authenticated.
        [self authenticate];
    }
    
    if (leaderboardDismissCompletionBlock)
    {
        leaderboardDismissCompletionBlock(); //execute block
        Block_release(leaderboardDismissCompletionBlock);
        leaderboardDismissCompletionBlock = nil;
    }
    
    GKLeaderboardViewController *leaderboardController = [[GKLeaderboardViewController alloc] init];
    
    leaderboardController.category = leaderboardID;
    leaderboardController.timeScope = GKLeaderboardTimeScopeAllTime;
    leaderboardController.leaderboardDelegate = self;
    [leaderboardController popViewControllerAnimated:NO];
    
    [[[AVDataManager sharedInstance] getViewController] presentModalViewController:leaderboardController animated:YES];
    
    [leaderboardController release];
}

-(void) showLeaderboardsWithCompletionBlock:(void(^)(void)) block
{
    if (![self isAvailableForDevice]) return;
    leaderboardDismissCompletionBlock = Block_copy(block);
    [self showLeaderboards];
}

#pragma mark Posting Scores

-(void) postScore:(NSNumber*) score toLeaderboard:(NSString*) leaderboardID
{
    if (![self isAvailableForDevice]) return;
    if (![GKLocalPlayer localPlayer].isAuthenticated)
    {
        [self saveFailedScore:score forLeaderboard:leaderboardID];
        return;
    }
    
    __block id selfReference = self;
    GKScore* scoreObject = [[GKScore alloc] initWithCategory:leaderboardID];
    scoreObject.value = [score longLongValue];
    
    [scoreObject reportScoreWithCompletionHandler:^(NSError *error)
     {
         if (error)
         {
             [selfReference saveFailedScore:score forLeaderboard:leaderboardID];
         }
     }];
    [scoreObject release];
}

#pragma mark Failed Scores

-(void) saveFailedScore:(NSNumber*) score forLeaderboard:(NSString*) leaderboardID
{
    if (![self isAvailableForDevice]) return;
    if ([score intValue] > [[failedScores valueForKey:leaderboardID] intValue])
    {
        [failedScores setValue:score forKey:leaderboardID];
        [self saveFailedScores];
    }
}

-(void) submitFailedScores
{
    if (![self isAvailableForDevice]) return;
    NSArray* keys = [failedScores allKeys];
    for(NSString* key in keys)
    {
        NSNumber* score = [failedScores valueForKey:key];
        [self postScore:score toLeaderboard:key];
        [failedScores removeObjectForKey:key];
    }
    [self saveFailedScores];
}

-(void) saveFailedScores
{
    if (![self isAvailableForDevice]) return;
    [[NSUserDefaults standardUserDefaults] setObject:failedScores forKey:KEYS_FAILED_SCORES];
    [[NSUserDefaults standardUserDefaults] synchronize];
}

#pragma mark GKLeaderboardViewControllerDelegate

- (void)leaderboardViewControllerDidFinish:(GKLeaderboardViewController *)viewController
{
    if (![self isAvailableForDevice]) return;
    [viewController dismissModalViewControllerAnimated:YES];
    
    if (leaderboardDismissCompletionBlock)
    {
        leaderboardDismissCompletionBlock(); //execute block
        Block_release(leaderboardDismissCompletionBlock);
        leaderboardDismissCompletionBlock = nil;
    }
}

#pragma mark -
#pragma mark Achievements
#pragma mark -

#pragma mark Display
-(void) showAchievements
{
    if (![self isAvailableForDevice]) return;
    NSAssert([[AVDataManager sharedInstance] getViewController] != nil, @"viewController not set");
    
    if (![GKLocalPlayer localPlayer].isAuthenticated)
    {
        //User has actively asked to see something to do with Game Center. Ask them to login again if they aren't authenticated.
        [self authenticate];
    }
    
    GKAchievementViewController *achievementsViewController = [[GKAchievementViewController alloc] init];
    if (achievementsViewController != nil)
    {
        achievementsViewController.achievementDelegate = self;
        [[[AVDataManager sharedInstance] getViewController] presentModalViewController: achievementsViewController animated:YES];
    }
    [achievementsViewController release];
}

-(float) localPercentageForAchievementIdentifier:(NSString *)achievementID
{
    return [self localProgressForAchievementIdentifier:achievementID] * 100.0f;
}

-(float) localProgressForAchievementIdentifier:(NSString*) achievementID
{
    return [[achievementProgress valueForKey:achievementID] floatValue];
}

#pragma mark Posting achievements


- (void) reportPercentage:(float) percent forAchievementIdentifier: (NSString*) achievementID
{
    [self reportAchievementProgress:percent/100.0f forAchievementIdentifier:achievementID];
}

- (void) reportAchievementProgress:(float) progress forAchievementIdentifier: (NSString*) achievementID
{
    if (![self isAvailableForDevice]) return;
    
    if (progress >= 1.0f) progress = 1.0f;
    
    if (![GKLocalPlayer localPlayer].isAuthenticated)
    {
        [self saveFailedProgressComplete:progress forAchievement:achievementID];
        return;
    }
    
    if (([[achievementProgress valueForKey:achievementID] floatValue]) >= progress * 100.0f)
    {
        //Already got a higher percentage
        return;
    }
    
    __block id selfReference = self;
    
    GKAchievement *achievement = [[[GKAchievement alloc] initWithIdentifier: achievementID] autorelease];
    if (achievement)
    {
        achievement.showsCompletionBanner = true;
        achievement.percentComplete = progress * 100.0f;
        [achievement reportAchievementWithCompletionHandler:^(NSError *error)
         {
             if (error != nil)
             {
                // NSLog(@"Found error:%@", error.localizedDescription);
                 [selfReference saveFailedProgressComplete:progress forAchievement:achievementID];
             }
             else if (progress >= 1.0f)
             {
                 //NSAssert(delegate != nil, @"No delegate set for achievements");
                 //[delegate achievementCompleted:achievementID];
                 [achievementProgress setValue:[NSNumber numberWithFloat:achievement.percentComplete] forKey:achievementID];
                 [[NSUserDefaults standardUserDefaults] setValue:achievementProgress forKey:KEYS_USER_ACHIEVEMENTS];
                 [[NSUserDefaults standardUserDefaults] synchronize];
                 [selfReference retrieveAchievmentMetadata];
             }
         }];
    }
}

#pragma mark Failed Achievements

-(void) saveFailedPercentComplete:(float)percent forAchievement:(NSString *)achievementID
{
    [self saveFailedProgressComplete:percent * 100.0f forAchievement:achievementID];
}

-(void) saveFailedProgressComplete:(float)progress forAchievement:(NSString *)achievementID
{
    if (![self isAvailableForDevice]) return;
    if (progress > [[failedAchievements valueForKey:achievementID] floatValue])
    {
        [failedAchievements setValue:[NSNumber numberWithFloat:progress] forKey:achievementID];
        //[failedAchievements setValue:@(progress) forKey:achievementID];
        [self saveFailedAchievements];
    }
}

-(void) submitFailedAchievements
{
    if (![self isAvailableForDevice]) return;
    NSArray* keys = [failedAchievements allKeys];
    for(NSString* key in keys)
    {
        NSNumber* progressComplete = [failedAchievements valueForKey:key];
        [self reportAchievementProgress:[progressComplete floatValue] forAchievementIdentifier:key];
        [failedAchievements removeObjectForKey:key];
    }
    [self saveFailedAchievements];
}

-(void) saveFailedAchievements
{
    if (![self isAvailableForDevice]) return;
    [[NSUserDefaults standardUserDefaults] setObject:failedAchievements forKey:KEYS_FAILED_ACHIEVEMENTS];
    [[NSUserDefaults standardUserDefaults] synchronize];
}


#pragma mark GKAchievementViewControllerDelegate

- (void)achievementViewControllerDidFinish:(GKAchievementViewController *)viewController
{
    if (![self isAvailableForDevice]) return;
    [viewController dismissModalViewControllerAnimated:YES];
}

#pragma mark Metadata

- (void) retrieveAchievmentMetadata
{
    if (![self isAvailableForDevice]) return;
    [achievementMetadata removeAllObjects];
    [GKAchievementDescription loadAchievementDescriptionsWithCompletionHandler:
     ^(NSArray *descriptions, NSError *error) {
         if (error != nil)
         {
             
         }
         else if (descriptions != nil)
         {
             for (GKAchievementDescription *achievement in descriptions)
             {
                 [achievementMetadata setObject:achievement forKey:achievement.identifier];
             }
         }
     }];
}

-(void) retrieveAchievements
{
    if (![self isAvailableForDevice]) return;
    [achievementProgress removeAllObjects];
    [achievementProgress addEntriesFromDictionary:[[NSUserDefaults standardUserDefaults] dictionaryForKey:KEYS_USER_ACHIEVEMENTS]];
    [GKAchievement loadAchievementsWithCompletionHandler:^(NSArray *achievements, NSError *error) {
        if (error != nil)
        {
            
        }
        if (achievements != nil)
        {
            for (GKAchievement *achievement in achievements)
            {
                [achievementProgress setValue:[NSNumber numberWithFloat:achievement.percentComplete] forKey:achievement.identifier];
            }
            
            [[NSUserDefaults standardUserDefaults] setObject:achievementProgress forKey:KEYS_USER_ACHIEVEMENTS];
            [[NSUserDefaults standardUserDefaults] synchronize];
        }
    }];
}

#pragma mark Clear Achievements

-(void) resetAchievements
{
    [GKAchievement resetAchievementsWithCompletionHandler:^(NSError *error) {
        if (error) NSLog(@"Error:%@", error.localizedDescription);
        //else NSLog(@"AVGameCenterManager :: Achievements reset");
    }];
    [achievementProgress removeAllObjects];
    
    [[NSUserDefaults standardUserDefaults] setObject:achievementProgress forKey:KEYS_USER_ACHIEVEMENTS];
    [[NSUserDefaults standardUserDefaults] synchronize];
}

#pragma mark -
#pragma mark Friends
#pragma mark -

-(void) loadFriendData
{
    if (![self isAvailableForDevice]) return;
    
    [[GKLocalPlayer localPlayer] loadFriendsWithCompletionHandler:^(NSArray *friends, NSError *error)
     {
         if (error)
         {
             return;
         }
         [GKPlayer loadPlayersForIdentifiers:friends withCompletionHandler:^(NSArray *players, NSError *error)
          {
              if (error != nil)
              {
                  return;
              }
              if (players != nil)
              {
                  if (friendMetadata) [friendMetadata release];
                  friendMetadata = [players retain];

              }
          }];
     }];
    
    
}

-(void) friendsScores:(NSRange)range forLeaderboard:(NSString *)identifier completionBlock:(void(^)(NSDictionary *friendsScores, NSError *error)) block
{
    if (![self isAvailableForDevice]) return;
    
    if ([friendMetadata count] <= 0)
    {
        return;
    }
    
    GKLeaderboard *leaderboardRequest = [[[GKLeaderboard alloc] init] autorelease];
    if (leaderboardRequest != nil)
    {
        leaderboardRequest.category = identifier;
        leaderboardRequest.timeScope = GKLeaderboardPlayerScopeFriendsOnly;
        leaderboardRequest.range = range;
        [leaderboardRequest loadScoresWithCompletionHandler:
         ^(NSArray *scores, NSError *error)
         {
             if (error)
             {
                 block(nil, error);
             }
             else
             {
                 NSMutableDictionary *friendsScores = [NSMutableDictionary dictionary];
                 for (GKScore *score in scores)
                 {
                     for (GKPlayer *p in friendMetadata)
                     {
                         if ([score.playerID isEqualToString:p.playerID])
                         {
                             [friendsScores setObject:score forKey:p.alias];
                         }
                     }
                 }
                 block([NSDictionary dictionaryWithDictionary:friendsScores], error);
             }
         }];
    }
}

-(NSString *) description
{
    return [NSString stringWithFormat:@"<AVGameCenterManager Achievements:%@>", achievementProgress];
}

#pragma mark -
#pragma mark Deallocations
#pragma mark -

-(void) dealloc
{
    [failedScores release];
    [achievementMetadata release];
    [achievementProgress release];
    
    if (friendMetadata) [friendMetadata release];

    [super dealloc];
}

#pragma mark -
#pragma mark Properties
#pragma mark -

@synthesize delegate;
@synthesize achievementMetadata;
@synthesize versionNumber;
@synthesize achievementProgress;
@end
