//
//  GooglePlusHeler.m
//  Unity-iPhone
//
//  Created by Aron Springfield on 02/04/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import "GooglePlusHelper.h"
#import "AVDataManager.h"
#import "SBJSON.h"

static GooglePlusHelper *sharedInstance;
static bool iosChecked = false;

@implementation GooglePlusHelper

+(GooglePlusHelper*) sharedInstance
{
    @synchronized([GooglePlusHelper class])
    {
        if (sharedInstance == nil && !iosChecked)
        {
            // Only return the instance if iOS is 6 or higher
            if([AVDataManager sharedInstance].isiOS6orHigher)
            {
                sharedInstance = [[GooglePlusHelper alloc] init];
            }
        }
        return sharedInstance;
    }
}

-(bool) isGoogleGamesSignedIn
{
    return [GPGManager sharedInstance].isSignedIn;
}

-(void) startWithClientId:(NSString *)clientId
{
    _clientId = clientId;
    
    GPPSignIn *signIn = [GPPSignIn sharedInstance];
    
    // You set kClientID in a previous step
    signIn.clientID = _clientId;
    signIn.scopes = [NSArray arrayWithObjects:@"https://www.googleapis.com/auth/games", nil];
    signIn.language = [[NSLocale preferredLanguages] objectAtIndex:0];
    signIn.delegate = self;
    signIn.shouldFetchGoogleUserID =YES;
    
    [[GPPSignIn sharedInstance] trySilentAuthentication];
}

-(void) signIn
{
    if(![GPGManager sharedInstance].isSignedIn)
    {
        if(![[GPPSignIn sharedInstance] trySilentAuthentication])
        {
            if(![[NSUserDefaults standardUserDefaults] boolForKey:@"hasAskedGPSignIn"])
            {
                [[[UIAlertView alloc] initWithTitle:@"Leaderboards & Achievements" message:@"An external login may be required to use these features." delegate:self cancelButtonTitle:@"No Thanks" otherButtonTitles:@"Continue", nil] show];
            }
            else
            {
                [[GPPSignIn sharedInstance] authenticate];
            }
        }
    }
}

-(void) signIntoGoogleGames
{
    [GPGManager sharedInstance].statusDelegate = self;
    
    // The GPPSignIn object has an auth token now. Pass it to the GPGManager.
    [[GPGManager sharedInstance] signIn:[GPPSignIn sharedInstance] reauthorizeHandler:^(BOOL requiresKeychainWipe, NSError *error)
     {
         // If you hit this, auth has failed and you need to authenticate.
         // Most likely you can refresh behind the scenes
         if (requiresKeychainWipe) {
             [[GPPSignIn sharedInstance] signOut];
         }
         [[GPPSignIn sharedInstance] authenticate];
     }];
}

-(void) unlockAchievementWithId:(NSString*)achId
{
    [[GPGAchievement achievementWithId:achId] unlockAchievementWithCompletionHandler:^(BOOL newlyUnlocked, NSError *error) {
        if (!error && newlyUnlocked)
        {
            NSLog(@"Hooray! Achievement unlocked!");
        }
    }];
}

-(void) submitLeaderboardScoreWithId:(NSString*) boardId score:(float)score
{
    GPGScore *myScore = [[GPGScore alloc] initWithLeaderboardId:boardId];
    myScore.value = score;
    [myScore submitScoreWithCompletionHandler: ^(GPGScoreReport *report, NSError *error)
    {
        if (error)
        {
            // Handle the error
        }
        else
        {
            // Analyze the report, if you'd like
        }
    }];
}

-(void) showAchievements
{
    GPGAchievementController* ac = [[[GPGAchievementController alloc] init] autorelease];
    ac.achievementDelegate = self;
    [[[AVDataManager sharedInstance] getViewController] presentModalViewController:ac animated:YES];
}

-(void) showLeaderboards
{
    GPGLeaderboardsController *leadsController = [[GPGLeaderboardsController alloc] init];
    leadsController.leaderboardsDelegate = self;
    [[[AVDataManager sharedInstance] getViewController] presentViewController:leadsController animated:YES completion:nil];
}

-(void) showLeaderboard:(NSString*) key
{
    GPGLeaderboardController *leadController = [[GPGLeaderboardController alloc] initWithLeaderboardId:key];
    leadController.leaderboardDelegate = self;
    [[[AVDataManager sharedInstance] getViewController] presentViewController:leadController animated:YES completion:nil];
}

#pragma mark -
#pragma mark Google Plus Delegate Methods

- (void)finishedWithAuth:(GTMOAuth2Authentication *)auth error:(NSError *)error
{
    if (error.code == 0 && auth)
    {
        NSLog(@"Success signing in to Google! Auth object is %@", auth);
        [self signIntoGoogleGames];
    }
    else
    {
        NSLog(@"Finished google plus authentication with error: %@", error); NSLog(@"Success signing in to Google! Auth object is %@", auth);
    }
}

// Optional

// Finished disconnecting user from the app.
// The operation was successful if |error| is |nil|.
- (void)didDisconnectWithError:(NSError *)error
{
     NSLog(@"Finished google plus didDisconnectWithError error: %@", error);
}

#pragma mark -
#pragma mark Google Play Games Delegate Methods

// optional
- (void)didFinishGamesSignInWithError:(NSError *)error
{
    NSLog(@"Google Play Games didFinishGamesSignInWithError error: %@", error);
    UnitySendMessage("AVGooglePlayInterface", "SignInComplete", error ? @"false".UTF8String : @"true".UTF8String);
}

#pragma mark -
#pragma mark Achievements & Leaderboards Delegate Methods

- (void)achievementViewControllerDidFinish:(GPGAchievementController *)viewController
{
    [[[AVDataManager sharedInstance] getViewController] dismissModalViewControllerAnimated:YES];
}

- (void)leaderboardsViewControllerDidFinish: (GPGLeaderboardsController *)viewController
{
    [[[AVDataManager sharedInstance] getViewController] dismissViewControllerAnimated:YES completion:nil];
}

- (void)leaderboardViewControllerDidFinish: (GPGLeaderboardController *)viewController
{
    [[[AVDataManager sharedInstance] getViewController] dismissViewControllerAnimated:YES completion:nil];
}

#pragma mark -
#pragma mark UIAlert Delegate

-(void) alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    [[NSUserDefaults standardUserDefaults] setBool:true forKey:@"hasAskedGPSignIn"];
    [[NSUserDefaults standardUserDefaults] synchronize];
    
    if(buttonIndex==1)
        [[GPPSignIn sharedInstance] authenticate];
}
@end
