#import <Foundation/Foundation.h>
#import "AVTwitter.h"

bool _TwitterIsAvailable() {
    return [[AVTwitter sharedTwitter] isTwitterAvailable];
}

void _TwitterComposeTweet(const char *msg) {
    NSLog([NSString stringWithUTF8String:msg]);
    [[AVTwitter sharedTwitter] tweet:[NSString stringWithUTF8String:msg]];
}