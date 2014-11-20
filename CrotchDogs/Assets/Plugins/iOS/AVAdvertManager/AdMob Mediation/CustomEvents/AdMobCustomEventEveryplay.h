//
//  AdMobCustomEventEveryplay.h
//  Unity-iPhone
//
//  Created by Aron Springfield on 24/07/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GADCustomEventInterstitial.h"
#import "GADCustomEventInterstitialDelegate.h"
#import "GADInterstitial.h"
#import "GADInterstitialDelegate.h"
#import <UnityAds/UnityAds.h>

@interface AdMobCustomEventEveryplay : NSObject <GADCustomEventInterstitial, UnityAdsDelegate> {
    
    id<GADCustomEventInterstitialDelegate> delegate;
}

@end
