//
//  AVPluginManager.m
//  AceViral
//
//  Created by Phil Smith on 24/07/2013.
//  Copyright (c) 2013 Phil Smith. All rights reserved.

#import "AVPluginManager.h"
#import "AVDataManager.h"
#import "AVGameCenterManager.h"
#import <AdSupport/ASIdentifierManager.h>


static AVPluginManager *sharedInstance;

@implementation AVPluginManager {}

+(AVPluginManager*) sharedInstance
{
    @synchronized ([AVPluginManager class])
    {
        if (!sharedInstance) sharedInstance = [[AVPluginManager alloc] init];
        return sharedInstance;
    }
}

+(id) alloc
{
    NSAssert(sharedInstance == nil, @"Use shared instance AVPluginManager#sharedInstance");
    return [super alloc];
}


-(void) initialize:(UIViewController*) controller delegate:(id)delegate{
    [[AVDataManager sharedInstance] setViewController:controller];
    [[AVGameCenterManager sharedManager] setDelegate:delegate];
    NSLog(@"%@ [%@]", @"Advertising ID", [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString]);
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation {
    m_URL = [NSString stringWithString:[url absoluteString]];
    if(m_URL){
        UnitySendMessage("AVGiftManager", "OnIntentDataReceived", m_URL.UTF8String);
    }
    return true;
}

-(NSString*) sendHandledUrl
{
    if(m_URL){
        UnitySendMessage("AVGiftManager", "OnIntentDataReceived", m_URL.UTF8String);
    }
    return @"null";
}

-(void)Log:(NSString*)tag msg:(NSString*)msg{
    NSLog(@"%@ [%@]", tag, msg);
}

-(void)LogError:(NSString*)tag msg:(NSString*)msg{
    NSLog(@"ERROR:%@ [%@]", tag, msg);
    //[Flurry logError:[NSString stringWithFormat:@"AV LOG ERROR:%@ [%@]", tag, msg] message:@"" error:nil];
}

-(void) dealloc
{
    [super dealloc];
}

@end
