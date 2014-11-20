#import "AVGameCenterManager.h"

bool _avGameCenterIsAvailable()
{
    return [[AVGameCenterManager sharedManager] isAvailableForDevice];
}

void _avGameCenterAuthenticate()
{
    [[AVGameCenterManager sharedManager] authenticate];
}

bool _avGameCenterIsSignedIn()
{
    return [[AVGameCenterManager sharedManager] isAuthenticated];
}

void _avGameCenterSignOut()
{
    // No Function Available!
}

void _avGameCenterShowAchievements()
{
    [[AVGameCenterManager sharedManager] showAchievements];
}

void _avGameCenterShowLeaderboard(const char *leaderboard)
{
    [[AVGameCenterManager sharedManager] showLeaderboard:[NSString stringWithUTF8String:leaderboard]];
}

void _avGameCenterShowLeaderboards()
{
    [[AVGameCenterManager sharedManager] showLeaderboards];
}

void _avGameCenterPostScore(float score, const char *leaderboard)
{
    [[AVGameCenterManager sharedManager] postScore:[NSNumber numberWithFloat:score] toLeaderboard:[NSString stringWithUTF8String:leaderboard]];
}

void _avGameCenterPostAchievement(float progress, const char *achievement)
{
    [[AVGameCenterManager sharedManager] reportAchievementProgress:progress forAchievementIdentifier:[NSString stringWithUTF8String:achievement]];
}