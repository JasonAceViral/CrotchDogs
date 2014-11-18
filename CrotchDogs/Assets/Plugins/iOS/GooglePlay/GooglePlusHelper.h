//
//  GooglePlusHeler.h
//  Unity-iPhone
//
//  Created by Aron Springfield on 02/04/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GooglePlus/GooglePlus.h>
#import <GooglePlayGames/GooglePlayGames.h>

@interface GooglePlusHelper : NSObject <GPPSignInDelegate, GPGStatusDelegate, GPGAchievementControllerDelegate, GPGLeaderboardControllerDelegate, GPGLeaderboardsControllerDelegate, UIAlertViewDelegate> {
    
}

@property(nonatomic, readonly) NSString* clientId;
@property(nonatomic, readonly) bool isGoogleGamesSignedIn;


+(GooglePlusHelper*) sharedInstance;

-(void) loginWithClientId:(NSString *)clientId;
-(void) unlockAchievementWithId:(NSString*)achId;
-(void) submitLeaderboardScoreWithId:(NSString*) boardId score:(float)score;
-(void) showAchievements;
-(void) showLeaderboards;
-(void) showLeaderboard:(NSString*) key;

@end
