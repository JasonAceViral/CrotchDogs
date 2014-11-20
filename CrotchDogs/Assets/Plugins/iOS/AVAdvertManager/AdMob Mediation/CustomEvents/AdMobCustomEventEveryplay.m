//
//  AdMobCustomEventEveryplay.m
//  Unity-iPhone
//
//  Created by Aron Springfield on 24/07/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import "AdMobCustomEventEveryplay.h"
#import "AVDataManager.h"


@implementation AdMobCustomEventEveryplay

@synthesize delegate;

#pragma mark -
#pragma mark - AdMob Invoked Functions

static bool hasInitialized = false;
static bool hasFailed = false;

-(void) setDelegate:(id<GADCustomEventInterstitialDelegate>)del
{
    delegate = del;
}

- (void)requestInterstitialAdWithParameter:(NSString *)serverParameter label:(NSString *)serverLabel request:(GADCustomEventRequest *)customEventRequest
{
    if(hasFailed)
    {
        NSError* error =[NSError errorWithDomain:@"Everyplay Ads" code:kGADErrorMediationAdapterError userInfo:@{NSLocalizedDescriptionKey : @"No ad content available."}];
        [delegate customEventInterstitial:self didFailAd:error];
        
        return;
    }
    
    if(!hasInitialized)
    {
        [[UnityAds sharedInstance] startWithGameId:@"15120" andViewController:[[AVDataManager sharedInstance] getViewController]];
        hasInitialized = true;
    }
    [[UnityAds sharedInstance] setDelegate:self];
    
    NSLog(@"Everyplay Ads : requestInterstitialAdWithParameter Can play?: %d", [[UnityAds sharedInstance] canShowAds]);
    
    if ([[UnityAds sharedInstance] canShowAds])
    {
        [delegate customEventInterstitial:self didReceiveAd:[[AVDataManager sharedInstance] getViewController]];
    }
    
//    double delayInSeconds = 0.2;
//    dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, delayInSeconds * NSEC_PER_SEC);
//    __unsafe_unretained AdMobCustomEventEveryplay* safeSelf = self;
//    dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
//        
//        [delegate customEventInterstitial:safeSelf didReceiveAd:[[AVDataManager sharedInstance] getViewController]];
//    });
}

- (void)presentFromRootViewController:(UIViewController *)rootViewController
{
    NSLog(@"Everyplay Ads : presentFromRootViewController Can play?: %d", [[UnityAds sharedInstance] canShowAds]);
    
    if ([[UnityAds sharedInstance] canShowAds])
    {
        // Show an offer somewhere in the game UI
        [[UnityAds sharedInstance] setZone:@"15120-1406046421"];
        [[UnityAds sharedInstance] show];
    }
}

#pragma mark -
#pragma mark - Everyplay Delegates

extern void _avUnitySendMessageVideoAwardReceived(NSString* currency, int amount);

- (void)unityAdsVideoCompleted:(NSString *)rewardItemKey skipped:(BOOL)skipped
{
    NSLog(@"Everyplay Ads : unityAdsVideoCompleted Skipped?: %d", skipped);
    if(!skipped)
    {
        _avUnitySendMessageVideoAwardReceived(@"gem", 1);
    }
}

- (void)unityAdsWillShow
{
    NSLog(@"Everyplay Ads : unityAdsWillShow");
    [delegate customEventInterstitialWillPresent:self];
}

- (void)unityAdsWillHide
{
    NSLog(@"Everyplay Ads : unityAdsWillHide");
    [delegate customEventInterstitialWillDismiss:self];
}

- (void)unityAdsDidHide
{
    NSLog(@"Everyplay Ads : unityAdsDidHide");
    [delegate customEventInterstitialDidDismiss:self];
}

- (void)unityAdsWillLeaveApplication
{
    NSLog(@"Everyplay Ads : unityAdsWillLeaveApplication");
    [delegate customEventInterstitialWillLeaveApplication:self];
}

- (void)unityAdsFetchCompleted
{
    NSLog(@"Everyplay Ads : unityAdsFetchCompleted");
    hasFailed = false;
    [delegate customEventInterstitial:self didReceiveAd:[[AVDataManager sharedInstance] getViewController]];
}

- (void)unityAdsFetchFailed
{
    NSLog(@"Everyplay Ads : unityAdsFetchFailed");
    hasFailed = true;
    NSError* error =[NSError errorWithDomain:@"Everyplay Ads" code:kGADErrorMediationNoFill userInfo:@{NSLocalizedDescriptionKey : @"No ad content available."}];
    [delegate customEventInterstitial:self didFailAd:error];
}

//- (void)unityAdsDidShow {}
//- (void)unityAdsVideoStarted {}

@end
