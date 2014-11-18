//
//  AVTwitter.h
//  AceViral
//
//  Created by James Webster on 19/09/2012.
//  Copyright (c) 2012 James Webster. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface AVTwitter : NSObject


+(AVTwitter*) sharedTwitter;

-(BOOL) isTwitterAvailable;

-(void) tweet:(NSString*) message;
-(void) tweet:(NSString*) message withURL:(NSURL*) url;
-(void) tweet:(NSString*) message withImage:(UIImage*) image;

@end
