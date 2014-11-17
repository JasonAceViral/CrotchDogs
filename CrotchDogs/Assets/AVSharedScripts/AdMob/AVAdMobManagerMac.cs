using UnityEngine;
using System.Collections;

namespace AVHiddenInterface {
    public class AVAdMobManagerMac : AceViral.AVAdMobManager {
		
	    public override void ShowBanner ()
	    {
	    }

        public override void LoadNewBanner()
        {
        }

	    public override void HideBanner ()
		{
		}
           

		/// <summary>
		/// ////////
		/// </summary>
		/// <returns>The advert height.</returns>

		#region implemented abstract members of AVAdwhirlManager
		public override int GetAdvertHeight ()
		{
			return 0;
		}
		#endregion

		#region implemented abstract members of AVAdwhirlManager
		public override void OnStart ()
		{
		}
		#endregion
	}
}
