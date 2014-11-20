//
//  AVAdModManager.m
//  AceViral
//
//  Created by Aron Springfield on 16/07/2013.
//  Copyright 2013 AceViral.com LTD. All rights reserved.
//

#import "AVAdMobManager.h"

static AVAdMobManager* _sharedIntersitialManager = nil;
static AVAdMobManager* _sharedVideoManager = nil;
static AVAdMobManager* _sharedBannerManager = nil;

#pragma mark AVAdModManager -

@implementation AVAdMobManager
{
    AVAdMobBanner* bannerAd;
    AVAdMobInterstitial* interstitialAd;
}

#pragma mark - Initializations

+(AVAdMobManager*) sharedIntersitialManager
{
    if (_sharedIntersitialManager == nil)  _sharedIntersitialManager = [[AVAdMobManager alloc] init];
    return _sharedIntersitialManager;
}

+(AVAdMobManager*) sharedVideoManager
{
    if (_sharedVideoManager == nil)  _sharedVideoManager = [[AVAdMobManager alloc] init];
    return _sharedVideoManager;
}

+(AVAdMobManager*) sharedBannerManager
{
    if (_sharedBannerManager == nil)  _sharedBannerManager = [[AVAdMobManager alloc] init];
    return _sharedBannerManager;
}

- (id)init
{
    self = [super init];
    if (self)
    {
        _appKey = nil;
    }
    return self;
}

- (void)dealloc
{
    [[NSNotificationCenter defaultCenter] removeObserver:self];
    [super dealloc];
}

#pragma mark - Setters and Getters

-(void) setBannerAdConfiguration:(AVAdPositionConfiguration)config
{
    bannerAd.bannerAdConfiguration = config;
    [bannerAd fixupBanner];
}

-(AVAdPositionConfiguration) bannerAdConfiguration
{
    return bannerAd.bannerAdConfiguration;
}

-(void) setAppKey:(NSString *)appKey
{
    _appKey = appKey;
    bannerAd.appKey = appKey;
    interstitialAd.appKey = appKey;
}

#pragma mark - Banner Control

-(void) createBanner
{
    if (bannerAd) return; //The banner ad is already initialized
    
    bannerAd = [[AVAdMobBanner alloc] init];
    bannerAd.appKey = _appKey;
    [bannerAd createBanner];
    [self requestFreshBannerAd];
}

-(void) requestFreshBannerAd
{
    [bannerAd requestFreshAd];
}

-(void) removeBanner
{
    if (bannerAd)
    {
        [bannerAd removeBanner];
    }
}

-(void) addBanner
{
    NSAssert(_appKey != nil, @"AdMobManager::appKey not set");
    NSAssert(bannerAd != nil, @"AdMobManager::banner not initialized");
    
    [bannerAd addBanner];
}

-(void) addBannerWithConfiguration:(AVAdPositionConfiguration)config
{
    [self setBannerAdConfiguration:config];
    [self addBanner];
}


-(void) addBannerWithConfiguration:(AVAdPositionConfiguration)config requestFresh:(BOOL)requestFresh
{
    [self addBannerWithConfiguration:config];
    if (requestFresh) [self requestFreshBannerAd];
}

-(void) addBannerAndRequestFresh
{
    [self addBannerWithConfiguration:self.bannerAdConfiguration requestFresh:YES];
}

#pragma mark - Interstitial / Video Control

-(void) createInterstitial
{
    [self createInterstitialWithIsVideo:false];
}

-(void) createInterstitialWithIsVideo:(BOOL)isVideo
{
    if (interstitialAd) return; //The interstitial ad is already initialized
    
    interstitialAd = [[AVAdMobInterstitial alloc] init];
    if(isVideo) [self setIntersitialIsVideoType];
    interstitialAd.appKey = _appKey;
    [interstitialAd requestFreshAd];
    [self requestFreshInterstitialAd];
}

-(void) stopAutoShowAd
{
    [interstitialAd stopAutoShowAd];
}

-(void) setIntersitialIsVideoType
{
    interstitialAd.isVideoInterstitial = true;
}

-(void) requestFreshInterstitialAd
{
    [interstitialAd requestFreshAd];
}

-(bool) isInterstitialReady
{
    return [interstitialAd interstitialIsReady];
}

-(void) showInterstitialAd
{
    NSAssert(_appKey != nil, @"AdMobManager::interstitial key not set");
    NSAssert(interstitialAd != nil, @"AdMobManager::interstitial not initialized");
    
    [interstitialAd displayInterstitial];
}

@end
