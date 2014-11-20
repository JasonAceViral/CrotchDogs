//
//  AVInAppPurchaseManagerBinding.m
//  AVInAppPurchases
//
//  Created by James Webster on 1/27/11.
//  Copyright 2012 AceViral.com LTD. All rights reserved.
//

#import "AVInAppPurchaseManager.h"
#import "AVInAppPurchaseInformation.h"
#import "AVReachability.h"
#import "AVPluginManager.h"

#define MAX_NUMBER_OF_PURCHASES 20

#define IAP_SUCCEEDED       2
#define IAP_FAILED          1
#define IAP_NO_RESPONSE     0

#define IAP_RESTORE_SUCCEEDED   2
#define IAP_RESTORE_FAILED      1
#define IAP_RESTORE_NO_RESPONSE 0


#define IAP_REQUEST_INFORMATION_SUCCEEDED   2
#define IAP_REQUEST_INFORMATION_FAILED      1
#define IAP_REQUEST_INFORMATION_NO_RESPONSE 0

static int successStatus = 0;
static int restoreStatus = 0;
static int requestInformationStatus = 0;
static int returnedProductIndex = 0;


static NSString* restoredPurchases[MAX_NUMBER_OF_PURCHASES] = {@""};
static NSString* informationIdentifiers[MAX_NUMBER_OF_PURCHASES] = {@""};
static NSString* returnedInformationPrices[MAX_NUMBER_OF_PURCHASES] = {@""};

//extern void UnitySendMessage(const char *, const char *, const char *);

void _avNotifyUnityOfPurchase(const char * transactionID)
{
    //NSLog(@"Notifying unity of transaction");
    UnitySendMessage("AVInAppUnity", "PurchaseNotification", transactionID);
}

void _avNotifyUnityOfFailure(const char * transactionID)
{
    //NSLog(@"Notifying unity of transaction");
    UnitySendMessage("AVInAppUnity", "PurchaseFailed", transactionID);
}

void _avInAppPurchaseManagerRequestProduct( const char *itemIdentifer )
{
    if(![AVReachability DeviceHasInternet]){
       //[[AVFacebookUnity sharedInstance] showAlert:@"In App Purchases" msg:@"Your device is not connected to the internet - please activate and try again later."];
        _avNotifyUnityOfFailure(itemIdentifer);
        return;
    }
    
    NSString *itemIdentiferString = [NSString stringWithUTF8String:itemIdentifer];
    
    successStatus = 0;
    [[AVPluginManager sharedInstance] Log:@"IAP" msg:[NSString stringWithFormat:@"Requesting product %@", itemIdentiferString]];
    
    [[AVInAppPurchaseManager sharedManager]
     requestProductWithIdentifier:itemIdentiferString
     completionBlock:^(SKPaymentTransaction *transaction, BOOL success)
     {
         successStatus = (success ? IAP_SUCCEEDED : IAP_FAILED);
         if(success){
             //[[AVPluginManager sharedInstance] Log:@"IAP" msg:[NSString stringWithFormat:@"Product purchase success: %@", transaction.payment.productIdentifier]];
             _avNotifyUnityOfPurchase(transaction.payment.productIdentifier.UTF8String);
         } else {
             //[[AVPluginManager sharedInstance] Log:@"IAP" msg:[NSString stringWithFormat:@"Product purchase failed: %@", transaction.payment.productIdentifier]];
             _avNotifyUnityOfFailure(transaction.payment.productIdentifier.UTF8String);
         }
     }];
    
}

void _avInAppPurchaseManagerRestorePurchases()
{
    if(![AVReachability DeviceHasInternet]){
        //[[AVFacebookUnity sharedInstance] showAlert:@"In App Purchases" msg:@"Your device is not connected to the internet - please activate and try again later."];
        return;
    }
    
    restoreStatus = IAP_RESTORE_NO_RESPONSE;
    
    for (int i = 0; i < MAX_NUMBER_OF_PURCHASES; i++)
    {
        restoredPurchases[i] = nil;
    }
    
    __block int currentIndex = 0;
    
    
    
    [[AVInAppPurchaseManager sharedManager] checkForIncompletePurchasesWithCompletionBlock:^(SKPaymentTransaction *transaction, BOOL success)
     {
         if (success) {
             restoredPurchases[++currentIndex] = transaction.transactionIdentifier;
             _avNotifyUnityOfPurchase(transaction.payment.productIdentifier.UTF8String);
         }
     }];
}

/*
 0 - no response yet
 1 - didn't succeed
 2 - succeeded
 */
int _avGetSuccessStatus()
{
    return successStatus;
}

int _avGetRestoreSuccessStatus()
{
    return restoreStatus;
}

int _avGetRequestInformationStatus()
{
    return requestInformationStatus;
}

NSString** _avGetRestoredPurchases()
{
    return restoredPurchases;
}

NSString** _avGetPrices()
{
    return returnedInformationPrices;
}

void _avAddIdentifierForInformationRequest(const char * identifier, int arrayIndex)
{
    informationIdentifiers[arrayIndex] = [NSString stringWithUTF8String:identifier];
}

void _avRequestAppInformation()
{
    //[[AVPluginManager sharedInstance] Log:@"IAP" msg:@"Requested IAP Information"];
    NSMutableArray *requestArray = [NSMutableArray array];
    for (int i = 0; i < MAX_NUMBER_OF_PURCHASES; i++)
    {
        if (informationIdentifiers[i])
            [requestArray addObject:informationIdentifiers[i]];
    }
    
    NSSet *requestSet = [NSSet setWithArray:requestArray];
    
    [[AVInAppPurchaseInformation sharedInformationManager] requestProductWithIdentifiers:requestSet completionBlock:^(NSArray *responseArray, BOOL successful)
     {
         returnedProductIndex = 0;
         for (SKProduct *p in responseArray)
         {
             NSNumberFormatter *formatter = [[[NSNumberFormatter alloc] init] autorelease];
             [formatter setNumberStyle:NSNumberFormatterCurrencyStyle];
             formatter.locale = p.priceLocale;
             NSString *formattedOutput = [formatter stringFromNumber:[NSNumber numberWithDouble:[p.price doubleValue]]];
             NSString *code = [p.priceLocale objectForKey:NSLocaleCurrencyCode];
             UnitySendMessage("AVInAppUnity", "OnCurrencyCodeReceived", code.UTF8String);
             //NSLog(@"Currency code: %@", code);
  
             returnedInformationPrices[returnedProductIndex] = [formattedOutput retain];
             returnedProductIndex += 1;
             
             //[[AVPluginManager sharedInstance] Log:@"IAP" msg:[p.productIdentifier stringByAppendingString:[@"#" stringByAppendingString:[formattedOutput retain]]]];
             UnitySendMessage("AVInAppUnity", "OnPurchaseDataReceived", [p.productIdentifier stringByAppendingString:[@"#" stringByAppendingString:[formattedOutput retain]]].UTF8String);
         }
         
         requestInformationStatus = (successful ? IAP_REQUEST_INFORMATION_SUCCEEDED : IAP_REQUEST_INFORMATION_FAILED);
     }];
}



