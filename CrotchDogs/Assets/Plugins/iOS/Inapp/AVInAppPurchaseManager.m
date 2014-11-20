//
//  AVInAppPurchaseManager.m
//  ChavBuster
//
//  Created by James Webster on 17/11/2011.
//  Copyright (c) 2011 aceviral. All rights reserved.
//

#import "AVInAppPurchaseManager.h"

#define kTimeoutSeconds 60

static AVInAppPurchaseManager *sharedInstance;

#define kRestoreCancelled 2
#define kRestoreCancelledAfterGivingBadPassword -5000

#pragma mark -
#pragma mark Private Interface
#pragma mark -

@interface AVInAppPurchaseManager (Private)

-(void) timeout;

-(void) completeTransaction:(SKPaymentTransaction *) transaction;
-(void) failedTransaction:(SKPaymentTransaction *) transaction;
-(void) recordTransaction:(SKPaymentTransaction*) transaction;
-(void) finishTransaction:(SKPaymentTransaction*) transaction wasSuccessful:(BOOL) success;

@end

#pragma mark -
#pragma mark AVInAppPurchaseManager Implementation
#pragma mark -

@implementation AVInAppPurchaseManager


#pragma mark -
#pragma mark Initializations
#pragma mark -

+(AVInAppPurchaseManager *) sharedManager
{
    @synchronized([AVInAppPurchaseManager class])
    {
        if (sharedInstance == nil) sharedInstance = [[self alloc] init];
        return sharedInstance;
    }
}

+(id) alloc
{
    @synchronized([AVInAppPurchaseManager class])
    {
        NSAssert(sharedInstance == nil, @"Use shared instance");
        return [super alloc];
    }
}

-(id) init
{
    self = [super init];
    if (self != nil)
    {
        connectingMessage = [[UIAlertView alloc] initWithTitle:@"Connecting\n\n" message:@"Please wait..." delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:nil];
        
        UIActivityIndicatorView *activityIndicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhite];
		activityIndicator.transform = CGAffineTransformMakeTranslation(130, 45);
		[activityIndicator startAnimating];
		[connectingMessage addSubview:activityIndicator];
		[activityIndicator release];
        
        completionBlocks = [[NSMutableDictionary alloc] initWithCapacity:2];
        
        userCancelledRequest = NO;
        
        [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    }
    return self;
}

#pragma mark -
#pragma mark UIAlertView delegate - responds to cancel button
#pragma mark -
-(void) alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    if (alertView==connectingMessage && buttonIndex==0)
    {
        //user pressed cancel
        userCancelledRequest = YES;
        
        //cancel timeout
        [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(timeout) object:nil];
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
#pragma mark Incomplete Purchases
#pragma mark -

-(void) checkForIncompletePurchasesWithCompletionBlock:(void(^)(SKPaymentTransaction*, BOOL)) block
{
    if ([completionBlocks objectForKey:@"Restore"])
    {
        void (^completionBlock)(SKPaymentTransaction *transaction, BOOL successful) = [completionBlocks objectForKey:@"Restore"];
        [completionBlock release];
    }
    [completionBlocks setObject:[block copy] forKey:@"Restore"];
    
    //completionBlock = Block_copy(block);
    
    //[[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
    connectingMessage.message = @"Checking for incomplete purchases.";
    [connectingMessage show];
}

-(void) paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue *)queue
{
    if ([queue.transactions count] == 0)
    {
        [self finishTransaction:nil wasSuccessful:YES];
    }
    for (SKPaymentTransaction *transaction in queue.transactions)
    {
        [self finishTransaction:transaction wasSuccessful:YES];
    }
}

-(void) paymentQueue:(SKPaymentQueue *)queue restoreCompletedTransactionsFailedWithError:(NSError *)error
{
    if (error != nil)
    {
        if (error.code == kRestoreCancelled || error.code == kRestoreCancelledAfterGivingBadPassword)
        {
            [self finishTransaction:nil wasSuccessful:NO];
            return;
        }
        //NSLog(@"Restore failed with error code: %d", error.code);
        NSString *message = [NSString stringWithFormat:@"There was a problem connecting to the app store. Please try again later. If this problem persists, please email support@aceviral.com with the following information: %@", error.localizedDescription];
        NSLog(@"IAP Error: %@", error.userInfo);
        [[[[UIAlertView alloc] initWithTitle:@"Error" message:message delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
    }
    
    [self finishTransaction:nil wasSuccessful:NO];
}

#pragma mark -
#pragma mark Start request
#pragma mark -

-(void) requestProductWithIdentifier:(NSString *) identifier completionBlock:(void(^)(SKPaymentTransaction *transaction, BOOL successful)) block
{
    
    //NSLog(@"Requesting to buy product: %@", identifier);
    userCancelledRequest = NO; //reset value
    
    //enable cancel button
    //[[[connectingMessage valueForKey:@"_buttons"] objectAtIndex:0] setEnabled:YES];
    
    
    if (![self canMakePurchases])
    {
        NSString *message = [NSString stringWithFormat:@"In-App Purchases are not possible. Parental controls are likely in place"];
        [[[[UIAlertView alloc] initWithTitle:@"Error" message:message delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
        return;
    }
    
    //start time-out func here?
    [self performSelector:@selector(timeout) withObject:nil afterDelay:kTimeoutSeconds];
    
    if ([completionBlocks objectForKey:identifier])
    {
        void (^completionBlock)(SKPaymentTransaction *transaction, BOOL successful) = [completionBlocks objectForKey:identifier];
        [completionBlock release];
    }
    
    [completionBlocks setObject:[block copy] forKey:identifier];
    
    
	SKProductsRequest* productRequest= [[SKProductsRequest alloc] initWithProductIdentifiers:[NSSet setWithObject:identifier]];
	productRequest.delegate = self;
	[productRequest start];
    
    connectingMessage.message = @"Please wait...";
    [connectingMessage show];
}

#pragma mark -
#pragma mark Process request
#pragma mark -
-(void) productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
    //cancel timeout
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(timeout) object:nil];
    
    
    //disable cancel button here - cancel won't do anything at this point
    //[[[connectingMessage valueForKey:@"_buttons"] objectAtIndex:0] setEnabled:NO];
    
    if (!userCancelledRequest) {
        if ([response.products count] > 0)
        {
            for (SKProduct *product in response.products)
            {
                //ONE BLOCK PER PRODUCT RECEIVED
                [[SKPaymentQueue defaultQueue] addPayment:[SKPayment paymentWithProduct:product]];
            }
        }
        else
        {
            [connectingMessage dismissWithClickedButtonIndex:0 animated:YES];
            
            NSString *message = [NSString stringWithFormat:@"There was a problem connecting to the app store. Please try again later (Invalid identifiers: %@) %@", response.invalidProductIdentifiers, response.debugDescription];
            [[[[UIAlertView alloc] initWithTitle:@"Error" message:message delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
            
            [self finishTransaction:nil wasSuccessful:NO];
        }
    }
    else
    {
        for (SKProduct *product in response.products)
        {
            //notify of cancellation
            NSLog(@"Received Response but user cancelled request for product: %@", product.productIdentifier);
        }
    }
    
    if (request != nil)
    {
        [request release];
        request = nil;
    }
}

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions
{
    for (SKPaymentTransaction *transaction in transactions)
    {
        SKPaymentTransactionState state = transaction.transactionState;
        switch (state)
        {
            case SKPaymentTransactionStatePurchasing:
                //Do nothing
                //NSLog(@"SHIT BE PURCHASING YO: %@", transaction.payment.productIdentifier);
                break;
            case SKPaymentTransactionStatePurchased:
                //NSLog(@"SHIT GOT PURCHASED YO: %@", transaction.payment.productIdentifier);
                [self completeTransaction:transaction];
                break;
            case SKPaymentTransactionStateRestored:
            {
                //NSLog(@"SHIT GOT RESTORED YO: %@", transaction.payment.productIdentifier);
                void (^completionBlock)(SKPaymentTransaction *transaction, BOOL successful) = [completionBlocks objectForKey:@"Restore"];
                if (completionBlock)
                {
                    [completionBlocks setObject:[completionBlock copy] forKey:transaction.payment.productIdentifier];
                }
                
                [self completeTransaction:transaction.originalTransaction];
                break;
            }
            case SKPaymentTransactionStateFailed:
                NSLog(@"%@", [transactions valueForKeyPath:@"error"]);
                //NSLog(@"SHIT FAILED YO: %@", transaction.payment.productIdentifier);
                [self failedTransaction:transaction];
                break;
            default:
                break;
        }
    }
}


#pragma mark -
#pragma mark End request
#pragma mark -

-(void) timeout
{
    //NSLog(@"Request for In-App Purchase timed-out!");
    [[[[UIAlertView alloc] initWithTitle:@"Timed Out" message:@"Unable to reach the App-Store. Please try again later. If this problem persists, please email support@aceviral.com." delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
    [self finishTransaction:nil wasSuccessful:NO];
}

-(void) request:(SKRequest *)request didFailWithError:(NSError *)error
{
    NSString *message = [NSString stringWithFormat:@"There was a problem connecting to the app store. Please try again later. If this problem persists, please email support@aceviral.com with the following information: %@", error.localizedDescription];
    
    [[[[UIAlertView alloc] initWithTitle:@"Error" message:message delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
    [self finishTransaction:nil wasSuccessful:NO];
}

-(void) completeTransaction:(SKPaymentTransaction *) transaction
{
    [self recordTransaction:transaction];
    [self finishTransaction:transaction wasSuccessful:YES];
}

-(void) failedTransaction:(SKPaymentTransaction *) transaction
{
    //decrement
    [self finishTransaction:transaction wasSuccessful:NO];
}

-(void) recordTransaction:(SKPaymentTransaction *)transaction
{
    [[NSUserDefaults standardUserDefaults] setValue:transaction.transactionReceipt forKey:transaction.transactionIdentifier];
}

#pragma TODO("Pass a message to the block as to why the transaction has failed. Pass a string or error code?");
-(void) finishTransaction:(SKPaymentTransaction *)transaction wasSuccessful:(BOOL)success
{
    //decrement
    [[NSUserDefaults standardUserDefaults] synchronize];
    NSDictionary *dictionary = [[NSUserDefaults standardUserDefaults] dictionaryRepresentation];
    for (NSString *key in [dictionary allKeys])
    {
        if ([key rangeOfString:@"com.urus.iap" options:NSCaseInsensitiveSearch].location != NSNotFound)
        {
            [[NSUserDefaults standardUserDefaults] removeObjectForKey:key];
            [[NSUserDefaults standardUserDefaults] synchronize];
            [[[[UIAlertView alloc] initWithTitle:@"Error" message:@"We've detected that you are trying to get In App Purchases in an unauthorized way on a jailbroken device. If you don't know what this message means, please email support@aceviral.com" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease] show];
            success = NO;
        }
    }
    
    //cancel timeout
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(timeout) object:nil];
    
    [connectingMessage dismissWithClickedButtonIndex:0 animated:YES];
    
    if (transaction != nil) [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
    
    //Find the block to use
    if (transaction.payment.productIdentifier)
    {
        void (^completionBlock)(SKPaymentTransaction *transaction, BOOL successful) = [completionBlocks objectForKey:transaction.payment.productIdentifier];
        //NSLog(@"Looking for: %@", transaction.payment.productIdentifier);
       // NSLog(@"Found %@", completionBlock);
       //NSLog(@"All completion blocks %@", completionBlocks);
        if (completionBlock)
        {
            //NSLog(@"Calling %@", completionBlock);
            completionBlock(transaction, success);
            //Block_release(completionBlock);
        }
    }
    else
    {
        NSLog(@"No completion block found.");
    }
    
}


#pragma mark -
#pragma mark dealloc
#pragma mark -
- (void) dealloc
{
    [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
	[connectingMessage release];
    
    
    for (void(^block)(SKPaymentTransaction *t, BOOL successful) in completionBlocks)
    {
        [block release];
    }
    
    [completionBlocks release];
    
	[super dealloc];
}
@end