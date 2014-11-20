//
//  HouseAdHandler.m
//  Unity-iPhone
//
//  Created by Aron Springfield on 03/04/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import "HouseAdHandler.h"
#import "AVDataManager.h"

static HouseAdHandler *m_sharedInstance;


@implementation HouseAdHandler

+(HouseAdHandler*) sharedInstance
{
    @synchronized([HouseAdHandler class])
    {
        if (m_sharedInstance == nil)
        {
            m_sharedInstance = [[HouseAdHandler alloc] init];
        }
        return m_sharedInstance;
    }
}

-(void) openHouseAdUrl:(NSString*)adLinkURL
{
    bool canLoad = [AVDataManager sharedInstance].isiOS6orHigher;
    
    if (canLoad) //If can load StoreProductViewController, try
    {
        NSNumber *appID = @([[adLinkURL substringFromIndex:[adLinkURL rangeOfString:@"id"].location + 2] intValue]);
        
        SKStoreProductViewController *popup = [[SKStoreProductViewController alloc] init];
        NSDictionary *d = @{SKStoreProductParameterITunesItemIdentifier : appID};
        
        [popup loadProductWithParameters:d
                         completionBlock:^(BOOL result, NSError *error)
         {
             if (!result)
             {
                 [[UIApplication sharedApplication] openURL:[NSURL URLWithString:adLinkURL]];
                 return;
             }
         }];
        popup.delegate = self;
        [[[AVDataManager sharedInstance] getViewController] presentModalViewController:popup animated:YES];
    }
    if (!canLoad) //If can't load
    {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:adLinkURL]];
    }
}

-(void) openMoreGamesPage
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"http://itunes.com/apps/aceviralcom"]];
}

-(void) productViewControllerDidFinish:(SKStoreProductViewController *)viewController
{
    [[[AVDataManager sharedInstance] getViewController] dismissModalViewControllerAnimated:YES];
}

@end
