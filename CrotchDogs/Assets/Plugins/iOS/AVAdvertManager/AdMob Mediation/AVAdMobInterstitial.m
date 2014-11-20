//
//  AVAdMobInterstitial.m
//  AceViral
//
//  Created by Aron Springfield on 17/07/2013.
//  Copyright 2013 AceViral.com LTD. All rights reserved.
//

#import "AVAdMobInterstitial.h"
#import "AVDataManager.h"

@implementation AVAdMobInterstitial
{
    GADInterstitial* interstitial;
    bool adHasRefreshedSinceViewed;
    bool adHasLoaded, adIsLoading;
    int failedLoadAttempts;
    bool waitingToSeeAd;
    
    UIActivityIndicatorView* activitySpinner;
}

-(id) init
{
    self = [super init];
    if (self)
    {
        _appKey = nil;
        adHasRefreshedSinceViewed = NO;
        adHasLoaded = NO;
        failedLoadAttempts = 0;
        waitingToSeeAd = false;
    }
    return self;
}

-(void) createInterstitial
{
    if (interstitial) return; //The ad is already initialized
    
    interstitial = [[GADInterstitial alloc] init];
    interstitial.adUnitID = _appKey;
    [interstitial setDelegate:self];
}

-(void) requestFreshAd
{
    if(!adHasRefreshedSinceViewed)
    {
        interstitial = nil;
        [self createInterstitial];
        GADRequest* req = [GADRequest request];
        //req.testDevices = [NSArray arrayWithObjects:@"9bfd561e0231345ec5b24c0f30da48ab", nil];
        [interstitial loadRequest:req];
        adIsLoading = YES;
        
        if(_isVideoInterstitial)
            UnitySendMessage("AVAdvertisingManager", "VideoInterstitialIsLoading", "no msg");
    }
}

-(void) stopAutoShowAd
{
    waitingToSeeAd = false;
}

-(void) displayInterstitial
{
    if(adHasLoaded && adHasRefreshedSinceViewed)
    {
        NSLog(@"AVAdMobInterstitial :: %@ is loaded. Attempting to present.", (_isVideoInterstitial ? @"VideoInterstitial" : @"Interstitial"));
        [interstitial presentFromRootViewController:[[AVDataManager sharedInstance] getViewController]];
        adHasRefreshedSinceViewed = NO;
    }
    else
    {
        waitingToSeeAd = true;
        [self removeSpinnerIfPresent];
        
        if(_isVideoInterstitial)
        {
            activitySpinner = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
            activitySpinner.hidesWhenStopped = YES;
            
            CGSize screenSize = [[UIScreen mainScreen] bounds].size;
            activitySpinner.center = CGPointMake(screenSize.width * 0.5, screenSize.height * 0.5);
            [activitySpinner startAnimating];
            
            [[[[AVDataManager sharedInstance] getViewController] view] addSubview:activitySpinner];
        }

        NSLog(@"AVAdMobInterstitial :: %@ not loaded. SHOW LOADING THINGMY.. but is it loading? %d", (_isVideoInterstitial ? @"VideoInterstitial" : @"Interstitial"), adIsLoading);
        
        if(!adIsLoading)
        {
            [self requestFreshAd];
        }
    }
}

-(bool) interstitialIsReady
{
    return interstitial.isReady;
}

# pragma mark Delegate

- (void)interstitialWillPresentScreen:(GADInterstitial *)ad
{
    if(_isVideoInterstitial)
        UnitySendMessage("AVAdvertisingManager", "VideoInterstitialWillPresentScreen", "no msg");
    else UnitySendMessage("AVAdvertisingManager", "InterstitialWillPresentScreen", "no msg");
}

// Sent when an interstitial ad request succeeded.  Show it at the next
// transition point in your application such as when transitioning between view
// controllers.
- (void)interstitialDidReceiveAd:(GADInterstitial *)ad
{
    NSLog(@"AdMobInterstitial :: did receive new ad");
    adHasLoaded = YES;
    adIsLoading = NO;
    adHasRefreshedSinceViewed = YES;
    failedLoadAttempts = 0;
    
    [self removeSpinnerIfPresent];
    
    if(waitingToSeeAd)
    {
        waitingToSeeAd = false;
        
        [activitySpinner stopAnimating];
        [activitySpinner removeFromSuperview];
        activitySpinner = nil;
        
        dispatch_async(dispatch_get_main_queue(), ^{ [self displayInterstitial]; });
    }

}

// Sent when an interstitial ad request completed without an interstitial to
// show.  This is common since interstitials are shown sparingly to users.
- (void)interstitial:(GADInterstitial *)ad didFailToReceiveAdWithError:(GADRequestError *)error
{
    NSLog(@"AdMobInterstitial :: %@ did fail to receive ad with error: %@", (_isVideoInterstitial ? @"VideoInterstitial" : @"Interstitial"), error);
    
    if(_isVideoInterstitial)
    {
        UnitySendMessage("AVAdvertisingManager", "VideoInterstitialIsNotReady", "no msg");
        
        if(waitingToSeeAd) // If user is waiting to see an ad, give an error message
        {
            if(error.code == kGADErrorNoFill ||error.code == kGADErrorMediationNoFill)
                UnitySendMessage("AVAdvertisingManager", "VideoInterstitialHasNoFill", "no msg");
        }
    }

    adIsLoading = NO;
    failedLoadAttempts++;
    
    if(failedLoadAttempts <= 2)
    {
        [self requestFreshAd];
    }
    else
    {
        [self removeSpinnerIfPresent];
        if(_isVideoInterstitial) UnitySendMessage("MsgBoxManager", "ShowOkDialogFromExternalRequest", "Video Ad#Failed to load advert");
    }
}

- (void)interstitialDidDismissScreen:(GADInterstitial *)ad
{
    //NSLog(@"AdMobInterstitial :: Ad view will dismiss screen");
    [self requestFreshAd];
    
    if(_isVideoInterstitial)
        UnitySendMessage("AVAdvertisingManager", "VideoInterstitialWillDismiss", "no msg");
    else UnitySendMessage("AVAdvertisingManager", "InterstitialWillDismiss", "no msg");
}

-(void) removeSpinnerIfPresent
{
    if(activitySpinner)
    {
        [activitySpinner stopAnimating];
        [activitySpinner removeFromSuperview];
        activitySpinner = nil;
    }
}

@end
