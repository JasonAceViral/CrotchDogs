//
//  AVAdMobBanner.m
//  AceViral
//
//  Created by Aron Springfield on 17/07/2013.
//  Copyright 2013 AceViral.com LTD. All rights reserved.
//

#import "AVAdMobBanner.h"
#import "AVDataManager.h"
#import <UIKit/UIKit.h>
#import "GADBannerView.h"
#import <CoreGraphics/CoreGraphics.h>

#define SCREEN_WIDTH [[UIScreen mainScreen] bounds].size.width
#define SCREEN_HEIGHT [[UIScreen mainScreen] bounds].size.height
#define IS_PAD ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad)




@implementation AVAdMobBanner
{
    GADBannerView* bannerView;
    bool appIsPortrait;
}

-(id) init
{
    self = [super init];
    if (self)
    {
        _appKey = nil;
        _bannerAdConfiguration = eAdConfigTop | eAdConfigCenter;
        
        [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(orientationChanged:)
                name:UIDeviceOrientationDidChangeNotification object:nil];
        
        UIDeviceOrientation* orient = [[UIDevice currentDevice] orientation];
        appIsPortrait = ((NSInteger)orient == (NSInteger)UIDeviceOrientationPortrait || (NSInteger)orient == (NSInteger)UIDeviceOrientationPortraitUpsideDown);
    }
    return self;
}

#pragma mark - Banner Control

-(void) createBanner
{
    if (bannerView) return; //The adview is already initialized
    
    bannerView = [[GADBannerView alloc] initWithAdSize:kGADAdSizeBanner];
    bannerView.adUnitID = _appKey;
    
    bannerView.rootViewController = [[AVDataManager sharedInstance] getViewController];
    [bannerView setDelegate:self];
}


-(void) setAdConfiguration:(AVAdPositionConfiguration)config
{
    _bannerAdConfiguration = config;
    [self fixupBanner];
}

-(void) requestFreshAd
{
    [bannerView loadRequest:[GADRequest request]];
}

-(void) removeBanner
{
    if (bannerView)
    {
        [bannerView removeFromSuperview];
    }
}

-(void) addBanner
{
    NSAssert(_appKey != nil, @"AdMobBanner::appKey not set");
    NSAssert(bannerView != nil, @"AdMobBanner::banner not initialized");
    
    [[[AVDataManager sharedInstance] getViewController].view addSubview:bannerView];
}

-(void) addBannerWithConfiguration:(AVAdPositionConfiguration)config
{
    [self setAdConfiguration:config];
    [self addBanner];
}


-(void) addBannerWithConfiguration:(AVAdPositionConfiguration)config requestFresh:(BOOL)requestFresh
{
    [self addBannerWithConfiguration:config];
    if (requestFresh) [self requestFreshAd];
}

-(void) addBannerAndRequestFresh
{
    [self addBannerWithConfiguration:_bannerAdConfiguration requestFresh:YES];
}

-(void) fixupBanner
{
    if (bannerView)
    {
        CGSize adSize = bannerView.adSize.size;
        NSLog(@"AdMobBanner :: Fixing banner with size: [%.0f, %.0f]", adSize.width, adSize.height);
        
        if (adSize.width < 1 || adSize.height < 1)
        {
            adSize = kGADAdSizeBanner.size;
        }
        
#if AV_DEBUG
        [self printAdConfiguration];
#endif
        
        int w = appIsPortrait ? MIN(SCREEN_WIDTH, SCREEN_HEIGHT) : MAX(SCREEN_WIDTH, SCREEN_HEIGHT);
        int h = appIsPortrait ? MAX(SCREEN_WIDTH, SCREEN_HEIGHT) : MIN(SCREEN_WIDTH, SCREEN_HEIGHT);
        
        float x = 0;
        float y = 0;
        
        if (_bannerAdConfiguration & eAdConfigTop)     y = 0.0f;
        if (_bannerAdConfiguration & eAdConfigBottom)  y = h - adSize.height;
        
        if (_bannerAdConfiguration & eAdConfigLeft)    x = 0.0f;
        if (_bannerAdConfiguration & eAdConfigRight)   x = w - adSize.width;
        if (_bannerAdConfiguration & eAdConfigCenter)  x = (w * 0.5f) - (adSize.width * 0.5f);
        
        bannerView.transform = CGAffineTransformMakeTranslation(x, y);
    }
}

-(void) orientationChanged:(UIDeviceOrientation*)orientation
{
    NSInteger orient = ((NSInteger)[[UIDevice currentDevice] orientation]);
    bool nowPortait = (orient == (NSInteger)UIDeviceOrientationPortrait || orient == (NSInteger)UIDeviceOrientationPortraitUpsideDown);
    
    if(nowPortait != appIsPortrait)
    {
        appIsPortrait = nowPortait;
        [self fixupBanner];
    }
}

#pragma mark - Debug

-(void) printAdConfiguration
{
    printf("---------------------------CRLBT");
    //PRINT_INT_AS_BINARY(self.bannerAdConfiguration);
}


#pragma mark - Delegate

- (void)adViewDidReceiveAd:(GADBannerView *)bannerView
{
    NSLog(@"AdMobBanner :: did receive new ad");
}

- (void)adView:(GADBannerView *)bannerView didFailToReceiveAdWithError:(GADRequestError *)error
{
    NSLog(@"AdMobBanner :: did failed to receive ad with error: %@", error);
}

- (void)adViewWillPresentScreen:(GADBannerView *)bannerView
{
    NSLog(@"AdMobBanner :: Ad view will present screen");
}

- (void)adViewDidDismissScreen:(GADBannerView *)bannerView
{
    NSLog(@"AdMobBanner :: Ad view did dismiss screen");
}

- (void)adViewWillDismissScreen:(GADBannerView *)bannerView
{
    NSLog(@"AdMobBanner :: Ad view will dismiss screen");
}

- (void)adViewWillLeaveApplication:(GADBannerView *)bannerView
{
    NSLog(@"AdMobBanner :: Ad view will leave application");
}

@end
