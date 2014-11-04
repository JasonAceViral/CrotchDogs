#import "AVGameCenterManager.h"

void _avGameCenterAuthenticate()
{
    [[AVGameCenterManager sharedManager] authenticate];
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