//
//  AVCloudStorage.h
//  Unity-iPhone
//
//  Created by Aron Springfield on 11/07/2014.
//  Copyright 2014 AceViral.com LTD. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface AVCloudStorage : NSObject {
    
}

@property (nonatomic, readonly) BOOL cloudIsAvailable;
@property (nonatomic, strong) NSMutableDictionary* cloudDefaults;


+(AVCloudStorage*) sharedInstance;

-(NSString*) loadDataForKey:(NSString*)key;
-(NSString*) loadAllData;
-(void) saveDictionaryData:(NSString*)data;
-(void) saveData:(NSString*)data key:(NSString*)key;
-(void) deleteKey:(NSString*)key;
-(void) synchronizeData;

@end
