 //
//  GAScene.h
//  AceViral
//
//  Created by Aron Springfield on 15/01/2014.
//  Copyright 2014 AceViral.com LTD. All rights reserved.
//

#import "GAI.h"
#import "GAIFields.h"
#import "GAIDictionaryBuilder.h"


/***************************
 * Google Analytics
 ***************************/

void gaSetupGoogleAnalyticsWithKey(NSString* key)
{
    if(key.length < 1)
    return;
    
    // Optional: automatically send uncaught exceptions to Google Analytics.
    [GAI sharedInstance].trackUncaughtExceptions = YES;
    
    // Optional: set Google Analytics dispatch interval to e.g. 20 seconds.
    [GAI sharedInstance].dispatchInterval = 120;
    
    // Optional: set Logger to VERBOSE for debug information.
    [[[GAI sharedInstance] logger] setLogLevel:kGAILogLevelError];//kGAILogLevelVerbose];//kGAILogLevelError];
    
    // Initialize tracker.
    [[GAI sharedInstance] trackerWithTrackingId:key];

    NSDictionary* infoplist = [[NSBundle mainBundle] infoDictionary];
    [[[GAI sharedInstance] defaultTracker] set:kGAIAppVersion value:[infoplist objectForKey:@"CFBundleVersion"]];
}

void gaTrackPageView(NSString* name)
{
    [[[GAI sharedInstance] defaultTracker] set:kGAIScreenName value:name];
    [[[GAI sharedInstance] defaultTracker] send:[[GAIDictionaryBuilder createAppView] build]];
}

void gaTrackEvent(NSString* category, NSString* action, NSString* label, NSNumber* value)
{
    [[[GAI sharedInstance] defaultTracker] send:[[GAIDictionaryBuilder createEventWithCategory:category     // Event category (required)
                                                                                        action:action             // Event action (required)
                                                                                         label:label              // Event label
                                                                                         value:value] build]];    // Event value
}

void gaDispath()
{
    [[GAI sharedInstance] dispatch];
}
