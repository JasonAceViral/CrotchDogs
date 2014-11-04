package com.aceviral.amazongamecircle;

import java.util.EnumSet;

import android.app.Activity;
import android.content.Intent;

import com.aceviral.GameServicesInterface;
import com.amazon.ags.api.AGResponseCallback;
import com.amazon.ags.api.AGResponseHandle;
import com.amazon.ags.api.AmazonGamesCallback;
import com.amazon.ags.api.AmazonGamesClient;
import com.amazon.ags.api.AmazonGamesFeature;
import com.amazon.ags.api.AmazonGamesStatus;
import com.amazon.ags.api.achievements.AchievementsClient;
import com.amazon.ags.api.achievements.UpdateProgressResponse;
import com.amazon.ags.api.leaderboards.LeaderboardsClient;
import com.amazon.ags.api.leaderboards.SubmitScoreResponse;

public class AmazonGameCircle implements GameServicesInterface
{
	private Activity m_Activity;

	public AmazonGameCircle(Activity m_Activity)
	{
		this.m_Activity = m_Activity;
		AmazonGamesClient.initialize(m_Activity, callback, myGameFeatures);
	}
	
	public void signIn()
	{
		
	}
	
	public void onActivityResult(int error,int i,Intent e)
	{
		
	}
	
	public void onStop()
	{
		
	}
	
	public void onStart()
	{
		
	}

	// reference to the agsClient
	AmazonGamesClient agsClient;
	LeaderboardsClient lbClient;
	AchievementsClient acClient;

	AmazonGamesCallback callback = new AmazonGamesCallback()
	{
		@Override
		public void onServiceNotReady(AmazonGamesStatus status)
		{
			// unable to use service
			System.out.println("was notready " + status.toString());
		}

		@Override
		public void onServiceReady(AmazonGamesClient amazonGamesClient)
		{
			agsClient = amazonGamesClient;
			lbClient = agsClient.getLeaderboardsClient();
			acClient = agsClient.getAchievementsClient();
			System.out.println("was ready ");
			// ready to use GameCircle
		}
	};


	// list of features your game uses (in this example, achievements and
	// leaderboards)
	EnumSet<AmazonGamesFeature> myGameFeatures = EnumSet.of(AmazonGamesFeature.Achievements, AmazonGamesFeature.Leaderboards);


	public void onPause()
	{
		if (agsClient != null)
		{
			agsClient.release();
		}
	}

	public void onResume()
	{
		AmazonGamesClient.initialize(m_Activity, callback, myGameFeatures);

	}

	@Override
	public void signOut()
	{
		// TODO Auto-generated method stub

	}


	@Override
	public void unlockAchievement(String achievementId)
	{
		if(AmazonGamesClient.getInstance() != null)
		{
			if (AmazonGamesClient.getInstance() != null)
			{
				acClient = AmazonGamesClient.getInstance().getAchievementsClient();
			}
		}
		if(acClient != null)
		{

			AGResponseHandle<UpdateProgressResponse> handle = acClient.updateProgress(achievementId, 100.0f);

			// Optional callback to receive notification of success/failure.
			handle.setCallback(new AGResponseCallback<UpdateProgressResponse>()
			{

				@Override
				public void onComplete(UpdateProgressResponse result)
				{
					if (result.isError())
					{
						// Add optional error handling here. Not strictly
						// required
						// since retries and on-device request caching are
						// automatic.
					} else
					{
						// Continue game flow.
					}
				}
			});
		}
	}

	@Override
	public void incrementAchievement(String achievementId, int numSteps)
	{
		// TODO Auto-generated method stub

	}

	@Override
	public void showAchievements()
	{
		System.out.println("show ing achievements " + acClient);
		if (agsClient == null)
		{
			if(AmazonGamesClient.getInstance() != null)
			{
				acClient = AmazonGamesClient.getInstance().getAchievementsClient();
			}
		}
		if (acClient != null)
		{
			acClient.showAchievementsOverlay();
		}
	}

	@Override
	public void updateLeaderboard(String leaderboardId, float score)
	{
		if (lbClient == null)
		{
			if(AmazonGamesClient.getInstance() != null)
			{
				lbClient = AmazonGamesClient.getInstance().getLeaderboardsClient();
			}
			
		}
		if (lbClient != null)
		{
			AGResponseHandle<SubmitScoreResponse> handle = lbClient.submitScore(leaderboardId, (long) score);

			// Optional callback to receive notification of success/failure.
			handle.setCallback(new AGResponseCallback<SubmitScoreResponse>()
			{

				@Override
				public void onComplete(SubmitScoreResponse result)
				{
					if (result.isError())
					{
						// Add optional error handling here. Not strictly
						// required
						// since retries and on-device request caching are
						// automatic.
					} else
					{
						// Continue game flow.
					}
				}
			});
		}
	}

	@Override
	public void showLeaderboard(String leaderboardId)
	{
		// TODO Auto-generated method stub

	}

	@Override
	public void showLeaderboards()
	{

		if (lbClient == null)
		{
			if(AmazonGamesClient.getInstance() != null)
			{
				lbClient = AmazonGamesClient.getInstance().getLeaderboardsClient();
			}
			
		}
		if (lbClient != null)
		{
			lbClient.showLeaderboardsOverlay();
		}
	}

	@Override
	public boolean isSignedIn()
	{
		return agsClient != null;
	}


}
