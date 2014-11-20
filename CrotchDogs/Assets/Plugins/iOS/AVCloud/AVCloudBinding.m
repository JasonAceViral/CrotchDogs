#import <Foundation/Foundation.h>
#import "AVCloudStorage.h"

// AVCloud interface

extern char* makeAllocatedString(NSString* input);

bool _avIsCloudAvailable()
{
    return [AVCloudStorage sharedInstance].cloudIsAvailable;
}

const char* _avLoadAllDataFromCloud()
{
    return makeAllocatedString([[AVCloudStorage sharedInstance] loadAllData]);
}

const char* _avLoadDataFromCloud(const char *file)
{
    NSString* result = [[NSString stringWithUTF8String:file] retain];
    return makeAllocatedString([[AVCloudStorage sharedInstance] loadDataForKey:result]);
    [result release];
}

void _avSaveDictionaryDataToCloud(const char* data)
{
    NSString* result = [[NSString stringWithUTF8String:data] retain];
    [[AVCloudStorage sharedInstance] saveDictionaryData:result];
    [result release];
}

void _avSaveDataToCloud(const char *file, const char *data)
{
    NSString* fileResult = [[NSString stringWithUTF8String:file] retain];
    NSString* dataResult = [[NSString stringWithUTF8String:data] retain];
    [[AVCloudStorage sharedInstance] saveData:dataResult key:fileResult];
    [fileResult release];
    [dataResult release];
}

void _avSynchronizeCloud()
{
    [[AVCloudStorage sharedInstance] synchronizeData];
}