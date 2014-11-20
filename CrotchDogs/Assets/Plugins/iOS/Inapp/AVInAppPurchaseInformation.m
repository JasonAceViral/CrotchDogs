//
//  AVInAppPurchaseInformation.m
//  NinjaFrogRun
//
//  Created by James Webster on 14/08/2012.
//  Copyright (c) 2012 AceViral.com LTD. All rights reserved.
//

#import "AVInAppPurchaseInformation.h"

#define kInfoRequestTimeoutSeconds 60
static AVInAppPurchaseInformation *sharedInstance;

@implementation AVInAppPurchaseInformation

+(AVInAppPurchaseInformation *) sharedInformationManager
{
    @synchronized([AVInAppPurchaseInformation class])
    {
        if (sharedInstance == nil) sharedInstance = [[self alloc] init];
        return sharedInstance;
    }
}

+(id) alloc
{
    @synchronized([AVInAppPurchaseInformation class])
    {
        NSAssert(sharedInstance == nil, @"Use shared instance");
        return [super alloc];
    }
}

#pragma mark -
#pragma mark Utility Methods
#pragma mark -

- (BOOL)canMakePurchases
{
    return [SKPaymentQueue canMakePayments];
}

#pragma mark -
#pragma mark Start request
#pragma mark -

-(void) requestProductWithIdentifiers:(NSSet *)identifiers completionBlock:(void(^)(NSArray *responseArray, BOOL successful)) block
{
    if (![self canMakePurchases])
    {
        NSString *message = [NSString stringWithFormat:@"Could not connect to In App Purchase server. Parental controls are likely in place"];
        [[[[UIAlertView alloc] initWithTitle:@"Error" message:message delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
        return;
    }
    
    completionBlock = Block_copy(block);
    
    [self performSelector:@selector(timeout) withObject:nil afterDelay:kInfoRequestTimeoutSeconds];
    
	SKProductsRequest* productRequest= [[SKProductsRequest alloc] initWithProductIdentifiers:identifiers];
	productRequest.delegate = self;
	[productRequest start];
}

#pragma mark -
#pragma mark Process request
#pragma mark -
-(void) productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
    //cancel timeout
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(timeout) object:nil];
    if (request != nil)
    {
        [request release];
        request = nil;
    }
    self.responseArray = response.products;
    [self finish:YES];
}

-(void) request:(SKRequest *)request didFailWithError:(NSError *)error
{
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(timeout) object:nil];
    if (request != nil)
    {
        [request release];
        request = nil;
    }
    self.responseArray = nil;
    [self finish:NO];
    
}



#pragma mark -
#pragma mark End request
#pragma mark -

-(void) timeout
{
    //[[[[UIAlertView alloc] initWithTitle:@"Timed Out" message:@"Unable to reach the In App Purchase Server." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
    [self finish:NO];
}


-(void) finish:(BOOL) success
{
    completionBlock(self.responseArray, success);
    Block_release(completionBlock);
}

@end
