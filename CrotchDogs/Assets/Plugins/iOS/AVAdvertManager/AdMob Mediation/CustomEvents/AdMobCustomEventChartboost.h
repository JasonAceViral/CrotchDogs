//
//  AdMobCustomEventChartboost.h
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
#import "Chartboost.h"


@interface AdMobCustomEventChartboost : NSObject <GADCustomEventInterstitial, ChartboostDelegate> {
    
    id<GADCustomEventInterstitialDelegate> delegate;
}

@end
