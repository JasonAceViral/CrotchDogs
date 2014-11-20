//
//  AVPluginBinding.m
//  Unity-iPhone
//
//  Created by Philip Smith on 24/07/2013.
//
//

#import "AVPluginManager.h"
#import "AVAdMobManager.h"

char* makeAllocatedString(NSString* string)
{
    if (!string)
        return "";
    
    const char* charString = string.UTF8String;
    
    char* res = (char*)malloc(strlen(charString) + 1);
    strcpy(res, charString);
    return res;
}

void _avUnitySendMessageVideoAwardReceived(NSString* currency, int amount)
{
    UnitySendMessage("AVAdvertisingManager", "ReceivedAdvertAward", makeAllocatedString([NSString stringWithFormat:@"%@:%d", currency, amount]));
}

void _avLog(const char * tag, const char * msg){
    [[AVPluginManager sharedInstance] Log:[NSString stringWithUTF8String:tag] msg:[NSString stringWithUTF8String:msg]];
}

void _avLogError(const char * tag, const char * msg){
    [[AVPluginManager sharedInstance] LogError:[NSString stringWithUTF8String:tag] msg:[NSString stringWithUTF8String:msg]];
}

bool _avDeviceIsIPad() {
    return ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad);
}

void _avOpenGameURL (const char *adUrl) {
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:[NSString stringWithUTF8String:adUrl]]];
}

//const char* _avGetHandledURL(){
//    return [[AVPluginManager sharedInstance] getHandledURL];
//}

