//
//  AVTwitter.m
//  AceViral
//
//  Created by James Webster on 19/09/2012.
//  Copyright (c) 2012 James Webster. All rights reserved.
//

#import "AVDataManager.h"


static AVDataManager *sharedInstance;

@implementation AVDataManager
{
    
}

#pragma mark -
#pragma mark Initialisations
#pragma mark -

+(AVDataManager*) sharedInstance
{
    @synchronized ([AVDataManager class])
    {
        if (!sharedInstance) sharedInstance = [[AVDataManager alloc] init];
        return sharedInstance;
    }
}

- (instancetype)init
{
    self = [super init];
    if (self)
    {
        NSString *reqSysVer = @"6.0";
        NSString *versionNumber = [[UIDevice currentDevice] systemVersion];
        
        BOOL isOSVer60 = ([versionNumber compare:reqSysVer options:NSNumericSearch] != NSOrderedAscending);
        _isiOS6orHigher = (NSClassFromString(@"SKStoreProductViewController") != nil) && isOSVer60;
    }
    return self;
}

+(id) alloc
{
    NSAssert(sharedInstance == nil, @"Use shared instance AVDataManager#sharedInstance");
    return [super alloc];
}


-(void) setViewController:(UIViewController *)controller
{
    unityViewController = controller;
}

-(UIViewController*) getViewController
{
    if(unityViewController == nil){
        NSLog(@"AVDataManager.getViewController: View controller was null when getting. Make sure you've added the relevent code to UnityAppController.mm");
    }
    return unityViewController;
}

#pragma mark -
#pragma mark Deallocations
#pragma mark -

-(void) dealloc
{
    [super dealloc];
}

@end
