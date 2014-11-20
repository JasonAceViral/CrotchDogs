//
//  AdMobCustomEventChartboost.m
//  Unity-iPhone
//
//  Created by Aron Springfield on 24/07/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import "AdMobCustomEventChartboost.h"
#import "AVDataManager.h"


@implementation AdMobCustomEventChartboost

@synthesize delegate;

#pragma mark -
#pragma mark - AdMob Invoked Functions

static id<GADCustomEventInterstitialDelegate> staticDelegate;
static bool hasInitialized = false;

-(void) setDelegate:(id<GADCustomEventInterstitialDelegate>)del
{
    delegate = del;
    staticDelegate = del;
}

- (void)requestInterstitialAdWithParameter:(NSString *)serverParameter label:(NSString *)serverLabel request:(GADCustomEventRequest *)customEventRequest
{
    if(!hasInitialized)
    {
        hasInitialized = true;
        [Chartboost startWithAppId:@"53ce92cec26ee433af496b90" appSignature:@"2843d6489caf732f50bf87f9a3b2d306f2ea9ab8" delegate:self];
    }
    
    if([[Chartboost sharedChartboost] hasCachedInterstitial:CBLocationMainMenu])
    {
        NSLog(@"Chartboost Ads : requestInterstitialAdWithParameter has cached ad");
        [staticDelegate customEventInterstitial:self didReceiveAd:[[AVDataManager sharedInstance] getViewController]];
    }
    else
    {
        NSLog(@"Chartboost Ads : requestInterstitialAdWithParameter download ad");
        [[Chartboost sharedChartboost] cacheInterstitial:CBLocationMainMenu];
    }
}

- (void)presentFromRootViewController:(UIViewController *)rootViewController
{
    NSLog(@"Chartboost Ads : presentFromRootViewController");
    [[Chartboost sharedChartboost] showInterstitial:CBLocationMainMenu];
}

#pragma mark -
#pragma mark - Chartboost Delegates

- (BOOL)shouldRequestInterstitial:(CBLocation)location
{
    NSLog(@"Chartboost Ads : shouldRequestInterstitial");
    return true;
}

- (void)didDisplayInterstitial:(CBLocation)location
{
    NSLog(@"Chartboost Ads : didDisplayInterstitial");
    [staticDelegate customEventInterstitialWillPresent:self];
}

- (BOOL)shouldDisplayInterstitial:(CBLocation)location
{
    //NSLog(@"Chartboost Ads : shouldDisplayInterstitial");
    return true;
}

- (void)didCacheInterstitial:(CBLocation)location
{
    NSLog(@"Chartboost Ads : didCacheInterstitial location: %@", location);
    [staticDelegate customEventInterstitial:self didReceiveAd:[[AVDataManager sharedInstance] getViewController]];
}

- (void)didFailToLoadInterstitial:(CBLocation)location  withError:(CBLoadError)error
{
    NSLog(@"Chartboost Ads : didFailToLoadInterstitial Error: %d", error);
    NSInteger gGADId = kGADErrorInvalidRequest;
    if(error == CBLoadErrorNoAdFound)
        gGADId = kGADErrorMediationNoFill;
    
    NSError* gadError = [NSError errorWithDomain:@"Chartboost Ads" code:gGADId userInfo:@{NSLocalizedDescriptionKey : @"No ad content available."}];
    [staticDelegate customEventInterstitial:self didFailAd:gadError];
}

- (void)didDismissInterstitial:(CBLocation)location
{
    NSLog(@"Chartboost Ads : didDismissInterstitial");
    [staticDelegate customEventInterstitialWillDismiss:self];
}

- (void)didCloseInterstitial:(CBLocation)location
{
    NSLog(@"Chartboost Ads : didCloseInterstitial");
    [staticDelegate customEventInterstitialDidDismiss:self];
}

- (BOOL)shouldRequestInterstitialsInFirstSession
{
    //NSLog(@"Chartboost Ads : shouldRequestInterstitialsInFirstSession");
    return true;
}

/// Called before requesting the more apps view from the back-end
/// Return NO if when showing the loading view is not the desired user experience.
- (BOOL)shouldDisplayLoadingViewForMoreApps
{
    //NSLog(@"Chartboost Ads : shouldDisplayLoadingViewForMoreApps");
    return true;
}

- (BOOL)shouldPrefetchVideoContent
{
    //NSLog(@"Chartboost Ads : shouldPrefetchVideoContent");
    return hasInitialized;
}

/// This delegate method reports when an inplay object has been recieved.
- (void)didLoadInPlay
{
    NSLog(@"Chartboost Ads : didLoadInPlay");
}

//- (void)didFailToRecordClick:(CBLocation)location withError:(CBLoadError)error;
//- (void)didClickInterstitial:(CBLocation)location;
//- (void)didCompleteAppStoreSheetFlow
//- (BOOL)shouldDisplayMoreApps
//- (BOOL)didDisplayMoreApps
//- (void)didCacheMoreApps
//- (void)didDismissMoreApps
//- (void)didCloseMoreApps
//- (void)didClickMoreApps
//- (void)didFailToLoadMoreApps:(CBLoadError)error

@end
