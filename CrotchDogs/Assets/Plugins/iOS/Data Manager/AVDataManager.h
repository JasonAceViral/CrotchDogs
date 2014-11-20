//
//  AVTwitter.h
//  AceViral
//
//  Created by Phil Smith on 21/09/2012.
//  Copyright (c) 2012 Phil Smith. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface AVDataManager : NSObject{
    UIViewController* unityViewController;
}

+(AVDataManager*) sharedInstance;

-(void) setViewController:(UIViewController*) controller;
-(UIViewController*) getViewController;

@property (nonatomic, readonly) bool isiOS6orHigher;

@end
