//
//  AVAdMobInterstitial.h
//  AceViral
//
//  Created by Aron Springfield on 17/07/2013.
//  Copyright 2013 AceViral.com LTD. All rights reserved.
//

#import "GADInterstitial.h"

@interface AVAdMobInterstitial : NSObject <GADInterstitialDelegate> {
    
}

/*! @brief The key as provided by AdWhirl used to uniquely identify this app. */
@property (nonatomic, copy)      NSString* appKey;
@property (nonatomic, readwrite) bool isVideoInterstitial;


-(void) requestFreshAd;
-(void) displayInterstitial;
-(bool) interstitialIsReady;
-(void) stopAutoShowAd;

@end
