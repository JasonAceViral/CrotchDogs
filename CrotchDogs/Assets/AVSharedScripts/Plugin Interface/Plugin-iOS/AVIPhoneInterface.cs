using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

class AVIPhoneInterface {

    #if !UNITY_EDITOR && UNITY_IPHONE
    [DllImport ("__Internal")] private static extern bool _avDeviceIsIPad( );
    #endif

    public static bool iOSDeviceIsIpad()
    {
        #if !UNITY_EDITOR && UNITY_IPHONE
        return _avDeviceIsIPad();
        #endif

        return false;
    }
}
