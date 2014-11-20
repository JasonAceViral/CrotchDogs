//
//  HouseAdHandler.h
//  Unity-iPhone
//
//  Created by Aron Springfield on 03/04/2014.
//  Copyright 2014 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface HouseAdHandler : NSObject <SKStoreProductViewControllerDelegate> {
    
}

+(HouseAdHandler*) sharedInstance;

-(void) openHouseAdUrl:(NSString*)adUrl;
-(void) openMoreGamesPage;

@end
