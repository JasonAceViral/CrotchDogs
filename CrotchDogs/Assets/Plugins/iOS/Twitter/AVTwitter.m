//
//  AVTwitter.m
//  AceViral
//
//  Created by James Webster on 19/09/2012.
//  Copyright (c) 2012 James Webster. All rights reserved.
//

#import "AVTwitter.h"
#import <Twitter/Twitter.h>
#import "AVDataManager.h"
#import <Accounts/Accounts.h>

#define letOSHandleLogin NO

static AVTwitter *sharedInstance;

extern UIViewController *UnityGetGLViewController();

#pragma mark -
#pragma mark AVTwitter Implementation
#pragma mark -
@implementation AVTwitter
{
    NSString *versionNumber;
}


#pragma mark -
#pragma mark Initialisations
#pragma mark -

+(AVTwitter*) sharedTwitter
{
    @synchronized ([AVTwitter class])
    {
        if (!sharedInstance) sharedInstance = [[AVTwitter alloc] init];
        return sharedInstance;
    }
}

+(id) alloc
{
    NSAssert(sharedInstance == nil, @"Use shared instance AVTwitter#sharedTwitter");
    return [super alloc];
}

#pragma mark -
#pragma mark Availability
#pragma mark -

-(BOOL) isTwitterAvailable
{
    // Test if device is running iOS 5.0 or higher
    NSString* reqSysVer = @"5.0";
    
    if (versionNumber == nil) versionNumber = [[[UIDevice currentDevice] systemVersion] retain];
    
    BOOL isOSVer50 = ([versionNumber compare:reqSysVer options:NSNumericSearch] != NSOrderedAscending);
    
    return ( (NSClassFromString(@"TWRequest") != nil) && isOSVer50);
}

#pragma mark -
#pragma mark Tweeting
#pragma mark -

-(void) tweet:(NSString*) message
{
    [self tweet:message withURL:nil withImage:nil];
}

-(void) tweet:(NSString*) message withURL:(NSURL*) url
{
    [self tweet:message withURL:url withImage:nil];
}

-(void) tweet:(NSString*) message withImage:(UIImage*) image
{
    [self tweet:message withURL:nil withImage:image];
}

-(void) tweet:(NSString*) message withURL:(NSURL*) url withImage:(UIImage*) img
{
    if (![self isTwitterAvailable]){
        UIAlertView* alert = [[[UIAlertView alloc] initWithTitle:@"Cannot Tweet" message:@"Twitter un-available to users of iOS 4 or under. Please update to at least iOS 5 before using this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil] autorelease];
         [alert show];
        return;
    }
    
    // Grab Unity view controller
    UIViewController *rootViewController = [[AVDataManager sharedInstance] getViewController];    
    
    //Create the tweet sheet
    TWTweetComposeViewController *tweetSheet = [[TWTweetComposeViewController alloc] init];
    
    //Customize the tweet sheet here
    //Add a tweet message
    [tweetSheet setInitialText:message];
    
    //Add an image
    if (img)
    {
        //BOOL addedImage = 
        [tweetSheet addImage:img];
        //if (!addedImage) AVWARNING(@"AVTwitter : image was set, but not added");
    }
    
    //Add a link
    //Don't worry, Twitter will handle turning this into a t.co link
    if (url)
    {
        //BOOL addedURL = 
        [tweetSheet addURL:url];
        //if (!addedURL) AVWARNING(@"AVTwitter : URL was set, but not added");
    }
    
    //Set a blocking handler for the tweet sheet
    tweetSheet.completionHandler = ^(TWTweetComposeViewControllerResult result){
        [rootViewController dismissModalViewControllerAnimated:YES];
        if(result == SLComposeViewControllerResultDone)
        {
            UnitySendMessage("AVTwitterObject", "UserSentTweet", "sent");
        }
    };
    
    //Show the tweet sheet!
    [rootViewController presentModalViewController:tweetSheet animated:YES];
}

-(void) followUser:(NSString*)userName
{
    if(![TWTweetComposeViewController canSendTweet])
    {
        [[[UIAlertView alloc] initWithTitle:@"No Twitter Accounts" message:@"There are currently no Twitter accounts configured. You can add or create a Twitter account in Settings" delegate:self cancelButtonTitle:@"Settings" otherButtonTitles:@"Cancel", nil] show];
        return;
    }
    
    ACAccountStore *accountStore = [[ACAccountStore alloc] init];
    
    ACAccountType *accountType = [accountStore accountTypeWithAccountTypeIdentifier:ACAccountTypeIdentifierTwitter];
    
    [accountStore requestAccessToAccountsWithType:accountType withCompletionHandler:^(BOOL granted, NSError *error) {
        if(granted) {
            // Get the list of Twitter accounts.
            NSArray *accountsArray = [accountStore accountsWithAccountType:accountType];
            
            // For the sake of brevity, we'll assume there is only one Twitter account present.
            // You would ideally ask the user which account they want to tweet from, if there is more than one Twitter account present.
            if ([accountsArray count] > 0) {
                // Grab the initial Twitter account to tweet from.
                ACAccount *twitterAccount = [accountsArray objectAtIndex:0];
                
                NSMutableDictionary *tempDict = [[NSMutableDictionary alloc] init];
                [tempDict setValue:userName forKey:@"screen_name"];
                [tempDict setValue:@"true" forKey:@"follow"];
                
                TWRequest *postRequest = [[TWRequest alloc] initWithURL:[NSURL URLWithString:@"https://api.twitter.com/1.1/friendships/create.json"]
                                                             parameters:tempDict
                                                          requestMethod:TWRequestMethodPOST];
                
                
                [postRequest setAccount:twitterAccount];
                
                [postRequest performRequestWithHandler:^(NSData *responseData, NSHTTPURLResponse *urlResponse, NSError *error) {
                    NSString *output = [NSString stringWithFormat:@"HTTP response status: %i", [urlResponse statusCode]];
                    NSLog(@"%@", output);
                    
                    if(!error) UnitySendMessage("AVTwitterObject", "UserHasBeenFollowed", userName.UTF8String);
                }];
            }
        }
    }];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if(buttonIndex == 0)
    {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"prefs:root=TWITTER"]];
    }
}

#pragma mark -
#pragma mark Deallocations
#pragma mark -

-(void) dealloc
{
    [versionNumber release];
    [super dealloc];
}


@end
