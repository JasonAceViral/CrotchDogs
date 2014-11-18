//
//  GameCenterManager.h
//  FAViOS2
//
//  Created by SHAZAD MOHAMMED on 25/11/2011.
//  Copyright (c) 2011 ACE VIRAL. All rights reserved.
//


#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>

@protocol AVGameCenterAchievementCompletionDelegate <NSObject>

-(void) achievementCompleted:(NSString*) achievementID;

@end

@interface AVGameCenterManager : NSObject <GKLeaderboardViewControllerDelegate, GKAchievementViewControllerDelegate>
{
    NSMutableDictionary *failedScores;
    NSMutableDictionary *failedAchievements;
    
    NSMutableDictionary *achievementMetadata;
    NSMutableDictionary *achievementProgress;
    
    NSArray *friendMetadata;
    
    void (^leaderboardDismissCompletionBlock)(void);
    
    id <AVGameCenterAchievementCompletionDelegate> delegate;
    
    NSString *versionNumber;
}

@property (nonatomic, assign) id<AVGameCenterAchievementCompletionDelegate> delegate;
@property (nonatomic, readonly) bool gameCenterFeaturesEnabled;
@property (nonatomic, retain, readonly) NSMutableDictionary *achievementMetadata;
@property (nonatomic, retain) NSString* versionNumber;
@property (nonatomic, retain, readonly) NSMutableDictionary *achievementProgress;

+ (id) sharedManager;

/* Returns YES for devices (and their iOS versions) that support Game Center */
-(BOOL) isAvailableForDevice;

/* Returns YES if the user has been authenticated */
-(BOOL) isAuthenticated;

/*
 Other calls to methods in this class will be ignored if not authenticated.
 */
-(void) authenticate;
/*
 Optionally Provide error handler for failed authentification
 */
-(void) authenticateWithCompletionHandler:(void (^)(NSError *))completionHandler;

/*
 Explicitly request failed scores and achievements to be submitted to Game Center
 */
-(void) submitFailedScores;
-(void) submitFailedAchievements;

/* You should give the NSNumber a long long int as this is the length of value leaderboards use */
-(void) postScore:(NSNumber*) score toLeaderboard:(NSString*) leaderboardID;
/* Percentage is a number between 0 and 100 */
- (void) reportPercentage:(float) percent forAchievementIdentifier: (NSString*) achievementID DEPRECATED_ATTRIBUTE;
- (void) reportAchievementProgress:(float) progress forAchievementIdentifier: (NSString*) achievementID;

-(float) localPercentageForAchievementIdentifier:(NSString*) achievementID DEPRECATED_ATTRIBUTE;

/*
 The default behaviour is just to dismiss the modal view controller of the gameCenterViewController
 */
-(void) showLeaderboard:(NSString*) identifier;
-(void) showLeaderboards;
/* Optionally Provide completion block for dimissing the leaderboard controller */
-(void) showLeaderboardsWithCompletionBlock:(void(^)(void)) block;

-(void) loadFriendData;
-(void) friendsScores:(NSRange)range forLeaderboard:(NSString *)identifier completionBlock:(void(^)(NSDictionary *friendsScores, NSError *error)) block;


-(void) showAchievements;
-(void) resetAchievements;

@end
