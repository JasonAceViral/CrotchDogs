//
//  AVAdMobBanner.h
//  AceViral
//
//  Created by Aron Springfield on 17/07/2013.
//  Copyright 2013 AceViral.com LTD. All rights reserved.
//

#import "GADBannerView.h"

/*!
@brief Allows the positon of the banner advert to be set one of a given valid positions.
In the case of invalid combinations (e.g. eAdConfigTop | eAdConfigBottom), bottom is given
precedence over top, center over right and right over left. The default value is top left.
*/
enum AVAdPositionConfiguration
{
    eAdConfigTop    =   1 << 0,   ///The advert should be at the top of the screen
    eAdConfigBottom =   1 << 1,   ///The advert should be at the bottom of the screen
    
    eAdConfigLeft   =   1 << 2,   ///The advert should be on the left of the screen
    eAdConfigRight  =   1 << 3,   ///The advert should be on the right of the screen
    eAdConfigCenter  =   1 << 4,  ///The advert should be in the center of the screen (horizontally, vertically is not permitted)
    
    eAdConfigBottomLeft = eAdConfigBottom | eAdConfigLeft,      ///The advert should be in the bottom left of the screen
    eAdConfigBottomRight = eAdConfigBottom | eAdConfigRight,    ///The advert should be in the bottom right of the screen
    eAdConfigBottomCenter = eAdConfigBottom | eAdConfigCenter,  ///The advert should be in the bottom center of the screen
    
    eAdConfigTopLeft = eAdConfigTop | eAdConfigLeft,            ///The advert should be in the top left of the screen
    eAdConfigTopRight = eAdConfigTop | eAdConfigRight,          ///The advert should be in the top right of the screen
    eAdConfigTopCenter = eAdConfigTop | eAdConfigCenter,        ///The advert should be in the top center of the screen
};

/*!
 @typedef AVAdPositionConfiguration
 
 @file AVAdWMobManager.h
 
 @brief typedef just allows AVAdPositionConfiguration enum to be used without enum prefix
 */

typedef enum AVAdPositionConfiguration AVAdPositionConfiguration;

@interface AVAdMobBanner : NSObject <GADBannerViewDelegate> {
    
}

/*! @brief Sets the ad configuration (its position from a predefined list) */
@property (nonatomic, assign)    AVAdPositionConfiguration bannerAdConfiguration;

/*! @brief The key as provided by AdWhirl used to uniquely identify this app. */
@property (nonatomic, copy)      NSString* appKey;

/*!
 @brief Asks the banner to refresh itself
 */
-(void) requestFreshAd;

/*!
 @brief Adds the banner to the screen. Its position will be determined by a previously set adConfiguration
 */
-(void) addBanner;

/*!
 @brief Adds the banner to the screen and ask it to request a new advert. Its position will be determined by a previously set adConfiguration
 */
-(void) addBannerAndRequestFresh;

/*!
 @brief Adds the banner to the screen. Its position will be determined by the argued config
 
 @param config - the position of the advert on the screen
 */
-(void) addBannerWithConfiguration:(AVAdPositionConfiguration) config;

/*!
 @brief Adds the banner to the screen and optionally requests a new advert. Its position will be determined by a previously set adConfiguration
 
 @param config - the position of the advert on the screen
 @param requestFresh - YES if a new advert should be requested, NO otherwise
 */
-(void) addBannerWithConfiguration:(AVAdPositionConfiguration) config requestFresh:(BOOL) requestFresh;


-(void) fixupBanner;

/*!
 @brief Removes the banner from the screen
 */
-(void) removeBanner;

/*!
 @brief Creates the ad banner. This message should only be called once
 */
-(void) createBanner;

@end
