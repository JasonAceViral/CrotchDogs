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
    };
    
    //Show the tweet sheet!
    [rootViewController presentModalViewController:tweetSheet animated:YES];
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
