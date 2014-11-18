using System;

public class DeviceProfiles
{
	public class Device{
		public Device(string name, float dpi){Name=name;DPI=dpi;}
		public Device(string name, float dpi, bool highEnd){Name=name;DPI=dpi;IsDeviceHighEnd=highEnd;}
		public Device(string name, float dpi, bool highEnd, bool throttleFPS){Name=name;DPI=dpi;IsDeviceHighEnd=highEnd; ThrottleLowFrameRate=throttleFPS;}
		public string Name;
		public float DPI;
		public bool IsDeviceHighEnd = false;
		public bool ThrottleLowFrameRate = false;
	}

	public static Device[] Profiles = {
		new Device("Macmini5,1", 0, true),
		new Device("asus Nexus 7", 75.0f, true),
		new Device("Sony Ericsson ST25i", 282.622f),
		new Device("samsung GT-I9000", 235.3703f),
		new Device("motorola MotoA953", 144.0f,false,true),
		new Device("samsung GT-I9300", 200.0f, true),
		new Device("samsung GT-I9100", 300.0f, true)
	};
	
	public static bool IsCurrentDeviceHighEnd()
	{
		foreach(Device device in Profiles){
			if(device.Name == UnityEngine.SystemInfo.deviceModel){
				return device.IsDeviceHighEnd;	
			}
		}
		return false;
	}
	
	public static bool IsCurrentDeviceRequiringFPSThrottle()
	{		
		foreach(Device device in Profiles){
			if(device.Name == UnityEngine.SystemInfo.deviceModel){
				return device.ThrottleLowFrameRate;	
			}
		}
		return false;
	}
	
	public static float GetDeviceDPI()
	{
		foreach(Device device in Profiles){
			if(device.Name == UnityEngine.SystemInfo.deviceModel){
				return device.DPI;	
			}
		}
		return UnityEngine.Screen.dpi;
	}
	
}

