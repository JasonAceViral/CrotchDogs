//
//  AVAdModManager.h
//  AceViral
//
//  Created by Aron Springfield on 16/07/2013.
//  Copyright 2013 AceViral.com LTD. All rights reserved.
//

#import "AVAdMobBanner.h"
#import "AVAdMobInterstitial.h"

/*!
 @enum AVAdPositionConfiguration
 
 @file AVAdMobManager.h
*/

/*!
 @class AVAdWMobManager
 
 @brief A singleton pattern class which provides access to the AdMobMediation banner adverts.
 
 @sa AVAdMobManager
 
 @author    Aron Springfield
 @author    Aron Springfield (Docs)
 @since     1.0.0
 @date      Created: @p 16-July-2013 @n
 @copyright AceViral.com LTD - All rights reserved.
 */

@interface AVAdMobManager : NSObject {
    
}

/*! @brief Sets the ad configuration (its position from a predefined list) */
//@property (nonatomic, assign)    AVAdPositionConfiguration bannerAdConfiguration;

/*! @brief The key as provided by AdWhirl used to uniquely identify this app. */
@property (nonatomic, copy)      NSString* appKey;

/*!
 @brief Returns the shared instance of this class
 
 @retval AVAdModManager - the shared instance of this class
 */
+(AVAdMobManager*) sharedIntersitialManager;
+(AVAdMobManager*) sharedVideoManager;
+(AVAdMobManager*) sharedBannerManager;

/*!
 @brief Asks the banner to refresh itself
 */
-(void) requestFreshBannerAd;

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

/*!
 @brief Removes the banner from the screen
 */
-(void) removeBanner;

/*!
 @brief Creates the ad banner. This message should only be called once
 */
-(void) createBanner;

-(void) setBannerAdConfiguration:(AVAdPositionConfiguration)config;

/*!
 @brief Creates the interstitial ad. This message should only be called once
 */
-(void) createInterstitial;
-(void) createInterstitialWithIsVideo:(BOOL)isVideo;

/*!
 @brief Checks if the advert is ready
 */
-(bool) isInterstitialReady;

/*!
 @brief Requests a fresh ad to be loaded. 
 Internally this will only invoke if the ad has been viewed already
 */
-(void) requestFreshInterstitialAd;

/*!
 @brief Invokes the interstitial advert to display
*/
-(void) showInterstitialAd;

-(void) stopAutoShowAd;

-(void) setIntersitialIsVideoType;


@end
