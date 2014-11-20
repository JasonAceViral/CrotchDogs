//
//  AdMobVungleCustomEvent.h
//  Unity-iPhone
//
//  Created by Aron Springfield on 07/07/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GADCustomEventInterstitial.h"
#import "GADCustomEventInterstitialDelegate.h"
#import "GADInterstitial.h"
#import "GADInterstitialDelegate.h"
#import <VungleSDK/VungleSDK.h>

@interface AdMobCustomEventVungle : NSObject <GADCustomEventInterstitial, VungleSDKDelegate> {
    
    id<GADCustomEventInterstitialDelegate> delegate;
}

@end
