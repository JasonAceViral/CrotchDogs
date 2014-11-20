//
//  AdMobVungleCustomEvent.m
//  Unity-iPhone
//
//  Created by Aron Springfield on 07/07/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import "AdMobCustomEventVungle.h"
#import "AVDataManager.h"

@implementation AdMobCustomEventVungle

@synthesize delegate;

#pragma mark -
#pragma mark - AdMob Invoked Functions

static bool hasInitialized = false;

-(void) setDelegate:(id<GADCustomEventInterstitialDelegate>)del
{
    delegate = del;
}

- (void)requestInterstitialAdWithParameter:(NSString *)serverParameter label:(NSString *)serverLabel request:(GADCustomEventRequest *)customEventRequest
{
    [VungleSDK sharedSDK].delegate = self;
    
    if(!hasInitialized)
    {
        [[VungleSDK sharedSDK] startWithAppId:@"859066300"];
        
        NSMutableDictionary* prefs = [[NSMutableDictionary alloc] init];
        [prefs setValue:[NSNumber numberWithBool:true] forKey:@"incentivized"];
        [prefs setValue:[NSNumber numberWithInt:(int)UIInterfaceOrientationMaskAllButUpsideDown] forKey:@"orientations"];
        [VungleSDK sharedSDK].userData = prefs;
        hasInitialized = true;
    }
    
    double delayInSeconds = 0.2;
    dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, delayInSeconds * NSEC_PER_SEC);
    __unsafe_unretained AdMobCustomEventVungle* safeSelf = self;
    dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
        
        [delegate customEventInterstitial:safeSelf didReceiveAd:[[AVDataManager sharedInstance] getViewController]];
    });
}

- (void)presentFromRootViewController:(UIViewController *)rootViewController
{
    NSLog(@"Play ad please, Vungle");
    [[VungleSDK sharedSDK] playAd:rootViewController];
}

#pragma mark -
#pragma mark - Vungle Delegates

- (void)vungleSDKwillShowAd
{
    [delegate customEventInterstitialWillPresent:self];
}

/**
 * if implemented, this will get called when the SDK closes the ad view, but there might be
 * a product sheet that will be presented. This point might be a good place to resume your game
 * if there's no product sheet being presented. The viewInfo dictionary will contain the
 * following keys:
 * - "completedView": NSNumber representing a BOOL whether or not the video can be considered a
 *               full view.
 * - "playTime": NSNumber representing the time in seconds that the user watched the video.
 * - "didDownlaod": NSNumber representing a BOOL whether or not the user clicked the download
 *                  button.
 */
- (void)vungleSDKwillCloseAdWithViewInfo:(NSDictionary*)viewInfo willPresentProductSheet:(BOOL)willPresentProductSheet
{
    if([[viewInfo objectForKey:@"completedView"] boolValue])
    {
        _avUnitySendMessageVideoAwardReceived(@"gem", 1);
    }
    
    [delegate customEventInterstitialWillDismiss:self];
}

/**
 * if implemented, this will get called when the product sheet is about to be closed.
 */
- (void)vungleSDKwillCloseProductSheet:(id)productSheet
{
    
}

@end
