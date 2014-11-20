//
//  AVPluginManager.h
//  AceViral
//
//  Created by Phil Smith on 24/07/2013.
//  Copyright (c) 2013 Phil Smith. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface AVPluginManager : NSObject{
    NSString* m_URL;
}

+(AVPluginManager*) sharedInstance;

-(void) initialize:(UIViewController*) controller delegate:(id)delegate;
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;
-(NSString*) sendHandledUrl;

-(void)Log:(NSString*)tag msg:(NSString*)msg;
-(void)LogError:(NSString*)tag msg:(NSString*)msg;
@end
