#import <Foundation/Foundation.h>
#import "AVTwitter.h"

bool _avTwitterIsAvailable() {
    return [[AVTwitter sharedTwitter] isTwitterAvailable];
}

void _avTwitterComposeTweet(const char *msg) {
    //NSLog([NSString stringWithUTF8String:msg]);
    [[AVTwitter sharedTwitter] tweet:[NSString stringWithUTF8String:msg]];
}

void _avTwitterFollowUser(const char *user) {
    [[AVTwitter sharedTwitter] followUser:[NSString stringWithUTF8String:user]];
}