//
//  AVInAppPurchaseManager.h
//  ChavBuster
//
//  Created by James Webster on 17/11/2011.
//  Copyright (c) 2011 aceviral. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface AVInAppPurchaseManager : NSObject <SKProductsRequestDelegate, SKPaymentTransactionObserver, UIAlertViewDelegate>
{
    UIAlertView *connectingMessage;
   // void (^completionBlock)(SKPaymentTransaction *transaction, BOOL successful);
    
    NSMutableDictionary *completionBlocks;
    BOOL userCancelledRequest;
}

+(AVInAppPurchaseManager *) sharedManager;

-(void) checkForIncompletePurchasesWithCompletionBlock:(void(^)(SKPaymentTransaction *transaction, BOOL successful)) block;
-(void) requestProductWithIdentifier:(NSString *) identifier 
                     completionBlock:(void(^)(SKPaymentTransaction *transaction, BOOL successful)) block;

@end
