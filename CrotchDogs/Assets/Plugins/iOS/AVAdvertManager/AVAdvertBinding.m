//
//  AVPluginBinding.m
//  Unity-iPhone
//
//  Created by Philip Smith on 24/07/2013.
//
//

#import "AVPluginManager.h"
#import "AVAdMobManager.h"
#import "HouseAdHandler.h"

// Banner Ads

void _avCreateAdMobBannerWithKey(const char * key){
    [[AVAdMobManager sharedBannerManager] setAppKey:[NSString stringWithUTF8String:key]];
    [[AVAdMobManager sharedBannerManager] createBanner];
}

void _avShowAdMobBanner(const int config){
    [[AVAdMobManager sharedBannerManager] addBannerWithConfiguration:config requestFresh:false];
}

void _avRefreshAdMobBanner(){
    [[AVAdMobManager sharedBannerManager] requestFreshBannerAd];
}

void _avSetBannerConfig(const int config) {
    [[AVAdMobManager sharedBannerManager] setBannerAdConfiguration:config];
}

void _avRemoveAdMobBanner(){
    [[AVAdMobManager sharedBannerManager] removeBanner];
}


// Video Interstitial Adverts

void _avCreateVideoInterstitialWithKey(const char* adKey)
{
    [[AVAdMobManager sharedVideoManager] setAppKey:[NSString stringWithUTF8String:adKey]];
    [[AVAdMobManager sharedVideoManager] createInterstitialWithIsVideo:true];
}

void _avShowVideoInterstitial()
{
    [[AVAdMobManager sharedVideoManager] showInterstitialAd];
}

void _avLoadVideoInterstitial()
{
    [[AVAdMobManager sharedVideoManager] requestFreshInterstitialAd];
}

bool _avIsVideoInterstitialReady()
{
    return [[AVAdMobManager sharedVideoManager] isInterstitialReady];
}

void _avCancelAutoShowVideoInterstitial()
{
    [[AVAdMobManager sharedVideoManager] stopAutoShowAd];
}

// Regular Interstitial Adverts

void _avCreateInterstitialWithKey(const char* adKey)
{
    [[AVAdMobManager sharedIntersitialManager] setAppKey:[NSString stringWithUTF8String:adKey]];
    [[AVAdMobManager sharedIntersitialManager] createInterstitial];
}

void _avShowInterstitial()
{
    [[AVAdMobManager sharedIntersitialManager] showInterstitialAd];
}

void _avLoadInterstitial()
{
    [[AVAdMobManager sharedIntersitialManager] requestFreshInterstitialAd];
}

bool _avIsInterstitialReady()
{
    return [[AVAdMobManager sharedIntersitialManager] isInterstitialReady];
}

void _avCancelAutoShowInterstitial()
{
    [[AVAdMobManager sharedIntersitialManager] stopAutoShowAd];
}

// House Ads

void _avOpenMoreGamesPage () {
    [[HouseAdHandler sharedInstance] openMoreGamesPage];
}

void _avOpenHouseAdLink (const char *adUrl) {
    [[HouseAdHandler sharedInstance] openHouseAdUrl:[NSString stringWithUTF8String:adUrl]];
}

