//
//  UnityAnalyticsBinding.m
//  Unity-iPhone
//
//  Created by Aron Springfield on 11/02/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import "AVPluginManager.h"
#import "GAHelper.h"

void _avSetUpGoogleAnalyticsWithKey(const char * key)
{
    gaSetupGoogleAnalyticsWithKey([NSString stringWithUTF8String:key]);
}

void _avTrackPageView(const char * page)
{
    gaTrackPageView([NSString stringWithUTF8String:page]);
}

void _avTrackEvent(const char * category, const char * action, const char * label, int value)
{
    gaTrackEvent([NSString stringWithUTF8String:category], [NSString stringWithUTF8String:action], [NSString stringWithUTF8String:label], [NSNumber numberWithInteger:value]);
}

void _avDispathAnalytics()
{
    gaDispath();
}