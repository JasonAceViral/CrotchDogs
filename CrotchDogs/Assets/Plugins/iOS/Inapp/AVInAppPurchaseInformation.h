//
//  AVInAppPurchaseInformation.h
//  NinjaFrogRun
//
//  Created by James Webster on 14/08/2012.
//  Copyright (c) 2012 AceViral.com LTD. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface AVInAppPurchaseInformation : NSObject <SKProductsRequestDelegate>
{
    void (^completionBlock)(NSArray *responseArray, BOOL successful);
}

@property (nonatomic, retain) NSArray *responseArray;

+(AVInAppPurchaseInformation *) sharedInformationManager;
-(void) requestProductWithIdentifiers:(NSSet *)identifiers completionBlock:(void(^)(NSArray *responseArray, BOOL successful)) block;

@end
