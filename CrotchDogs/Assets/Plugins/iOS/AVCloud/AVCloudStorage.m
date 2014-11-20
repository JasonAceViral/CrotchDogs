//
//  AVCloudStorage.h
//  Unity-iPhone
//
//  Created by Aron Springfield on 11/07/2014.
//  Copyright 2014 AceViral.com LTD. All rights reserved.
//

#import "AVCloudStorage.h"

static NSString* kCloudArrayKey = @"AV_KEYS";

@implementation AVCloudStorage

static AVCloudStorage *_sharedInstance;

+(AVCloudStorage*) sharedInstance
{
    @synchronized([AVCloudStorage class])
    {
        if (_sharedInstance == nil)
        {
            _sharedInstance = [[AVCloudStorage alloc] init];
        }
        return _sharedInstance;
    }
}

-(id) init
{
    if( (self=[super init] ))
    {
        NSURL *ubiq = [[NSFileManager defaultManager] URLForUbiquityContainerIdentifier:nil];
        _cloudIsAvailable = (ubiq != NULL);
        
        if(_cloudIsAvailable)
        {
            _cloudDefaults = [[[NSUbiquitousKeyValueStore defaultStore] dictionaryForKey:kCloudArrayKey] mutableCopy];
            if (!_cloudDefaults)
            {
                _cloudDefaults = [[NSMutableDictionary alloc] init];
                [[NSUbiquitousKeyValueStore defaultStore] setDictionary:_cloudDefaults forKey:kCloudArrayKey];
            }
            
            [[NSUbiquitousKeyValueStore defaultStore] synchronize];
            
            //  Observer to catch changes from iCloud
            [[NSNotificationCenter defaultCenter] addObserver:self
                                                     selector:@selector(onCloudStorageChange:)
                                                         name:NSUbiquitousKeyValueStoreDidChangeExternallyNotification
                                                       object:[NSUbiquitousKeyValueStore defaultStore]];
        }
    }
    return self;
}

-(NSString*) loadDataForKey:(NSString*)key
{
    return [_cloudDefaults objectForKey:key];
}

-(NSString*) loadAllData
{
    NSMutableString* allData = [[NSMutableString alloc] init];
    
    for(NSString* key in _cloudDefaults.allKeys)
    {
        if(allData.length > 0)
            [allData appendString:@"|_|"];
        
        NSString* object = [_cloudDefaults objectForKey:key];
        [allData appendFormat:@"%@|-|%@", key, object];
    }
    
    return allData;
}

-(void) saveDictionaryData:(NSString*)data
{
    NSArray* keyData = [data componentsSeparatedByString:@"|_|"];
    
    for(int i=0; i<keyData.count; i++)
    {
        NSArray* object = [keyData[i] componentsSeparatedByString:@"|-|"];
        if(object.count == 2)
        {
            [_cloudDefaults setObject:object[1] forKey:object[0]];
        }
    }
    
    // Update data on the iCloud
    [[NSUbiquitousKeyValueStore defaultStore] setDictionary:_cloudDefaults forKey:kCloudArrayKey];
}

-(void) saveData:(NSString*)data key:(NSString*)key
{
    [_cloudDefaults setObject:data forKey:key];
    
    // Update data on the iCloud
    [[NSUbiquitousKeyValueStore defaultStore] setDictionary:_cloudDefaults forKey:kCloudArrayKey];
}

-(void) deleteKey:(NSString*)key
{
    [_cloudDefaults removeObjectForKey:key];
    [[NSUbiquitousKeyValueStore defaultStore] setDictionary:_cloudDefaults forKey:kCloudArrayKey];
}

-(void) synchronizeData
{
    [[NSUbiquitousKeyValueStore defaultStore] setDictionary:_cloudDefaults forKey:kCloudArrayKey];
    [[NSUbiquitousKeyValueStore defaultStore]  synchronize];
}

#pragma mark - Observer

- (void)onCloudStorageChange:(NSNotification *)notification
{
    // Retrieve the changes from iCloud
    NSMutableDictionary* dict = [[[NSUbiquitousKeyValueStore defaultStore] dictionaryForKey:kCloudArrayKey] mutableCopy];
    
    if(dict)
    {
        _cloudDefaults = dict;
    
        [self informUnityGameObjectOfCloudUpdateAvailable];
    }
}

-(void) informUnityGameObjectOfCloudUpdateAvailable
{
    UnitySendMessage("AVCloudManager", "CloudUpdateAvailable", "");
}


@end
