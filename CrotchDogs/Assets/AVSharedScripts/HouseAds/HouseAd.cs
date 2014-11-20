using System;
using UnityEngine;

namespace AceViral {

    [System.Serializable]

    public class HouseAd
    {
        public bool HD, active, defaultAd;
        public int updateTime;
        public float x, y;
        public string slotId, adURL, imageURL;
        public Texture2D image;

        public void AdvertPressed ()
        {
            HouseAdInterface.OpenHouseAdUrl(adURL, slotId);
        }
    }
}
