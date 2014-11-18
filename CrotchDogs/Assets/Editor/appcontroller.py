    #!/usr/bin/python
import sys
import os

def process_app_controller_wrapper(appcontroller_filename, newContent, methodSignatures, valuesToAppend, positionsInMethod, contentToAppend=None):
    appcontroller = open(appcontroller_filename, 'r')
    lines = appcontroller.readlines()
    appcontroller.close()
    foundWillResignActive = False    
    foundIndex = -1
    for line in lines:         
        #print line
        newContent += line
        for idx, val in enumerate(methodSignatures):
            #print idx, val
            if (line.strip() == val):
                #print "founded match method: " + val
                foundIndex = idx
                foundWillResignActive = True
        if foundWillResignActive :
            if positionsInMethod[foundIndex] == 'begin' and line.strip() == '{':
                #print "add code to resign body"
                newContent += ("\n\t" + valuesToAppend[foundIndex] + "\n\n")
                foundWillResignActive = False
            if 	positionsInMethod[foundIndex] == 'end' and line.strip() == '}':
                newContent = newContent[:-3]
                newContent += ("\n\n\t" + valuesToAppend[foundIndex] + "\n")
                newContent += "}\n"
                foundWillResignActive = False
        if line.strip() == '@end' and (not contentToAppend is None):
                newContent = newContent[:-6]
                newContent += ("\n\n\t" + contentToAppend + "\n")
                newContent += "@end"                            
            
    print "-------done loop close stream and content: " + newContent                    
    appcontroller = open(appcontroller_filename, 'w')    
    appcontroller.write(newContent)
    appcontroller.close()        

def importHeaders():
    return '''
#include "AVPluginManager.h"
#include "AVGameCenterManager.h"
'''

def extraCodeToAddInAppControllerMMFile():
    return '''

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    return [[AVPluginManager sharedInstance] application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
}

-(void) handleLocalNotificationBadge
{
    [[UIApplication sharedApplication] cancelAllLocalNotifications];
    [UIApplication sharedApplication].applicationIconBadgeNumber = 0;
}

-(void) prepareLocalNotification
{
    double TIME_ONE_DAY = 5;
    
    [self notificationWithTimeAndBadge: TIME_ONE_DAY badge:1];
}

-(void) notificationWithTimeAndBadge:(int)secs badge:(int)badge
{
    UILocalNotification *notification = [UILocalNotification new];

    NSCalendar *gregorian = [[NSCalendar alloc] initWithCalendarIdentifier:NSGregorianCalendar];
    [gregorian setLocale:[NSLocale currentLocale]];
    
    NSDateComponents *nowComponents = [gregorian components:NSYearCalendarUnit | NSWeekCalendarUnit | NSHourCalendarUnit | NSMinuteCalendarUnit | NSSecondCalendarUnit fromDate:[NSDate date]];

    [nowComponents setWeekday:7];
    [nowComponents setWeek: [nowComponents week]];
    [nowComponents setHour:11];
    [nowComponents setMinute:0];
    [nowComponents setSecond:0];
    
    notification.fireDate = [gregorian dateFromComponents:nowComponents];
    
    notification.repeatInterval = NSWeekCalendarUnit;
    notification.alertBody = @"Get extra coins and gems in the sale!";
    notification.alertAction = @"Go to the sale!";
    notification.applicationIconBadgeNumber = badge;
    
    [[UIApplication sharedApplication] scheduleLocalNotification:notification];
    
    NSLog(@"Notification sent");
}

    '''
    
def touch_implementation(appcontroller_filename):
    # appcontroller = open(appcontroller_filename, 'w')
    # print(" process AppController.mm add imports header")
    newContent = importHeaders()
     
    #starting line of method {
    methodSignatures = []
    #value to append near }
    valueToAppend = []
	#position to add insert at the beginning o
    positionsInMethod = []
    
    methodSignatures.append('- (void)applicationWillEnterForeground:(UIApplication *)application')
    valueToAppend.append('[[AVPluginManager sharedInstance] initialize:_rootController delegate:self];')
    positionsInMethod.append("end")
    
    methodSignatures.append('- (void)applicationWillEnterForeground:(UIApplication *)application')
    valueToAppend.append('[self handleLocalNotificationBadge];')
    positionsInMethod.append("end")

    methodSignatures.append('- (void) applicationDidBecomeActive:(UIApplication*)application')
    valueToAppend.append('[self handleLocalNotificationBadge];')   
    positionsInMethod.append("end")
    
    methodSignatures.append('- (void)applicationWillTerminate:(UIApplication*)application')
    valueToAppend.append('[self prepareLocalNotification];')   
    positionsInMethod.append("begin")

    process_app_controller_wrapper(appcontroller_filename, newContent, methodSignatures, valueToAppend, positionsInMethod, extraCodeToAddInAppControllerMMFile())    