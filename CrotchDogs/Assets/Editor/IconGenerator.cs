using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

class IconGenerator
{
    enum eIconTypes
    {
        AndroidIcons = 0,
        iPhoneIcons,
        WP8Icons, 
        AllIcons
    }

    [UnityEditor.MenuItem("AceViral/Generate Icons/Android")]
    static void GenerateIconsForAndroid()
    { 
        GenerateIconsForPlatform(eIconTypes.AndroidIcons);
    }

    [UnityEditor.MenuItem("AceViral/Generate Icons/iPhone")]
    static void GenerateIconsForIPhone()
    { 
        GenerateIconsForPlatform(eIconTypes.iPhoneIcons);
    }

    [UnityEditor.MenuItem("AceViral/Generate Icons/WP8")]
    static void GenerateIconsForWP8()
    { 
        GenerateIconsForPlatform(eIconTypes.WP8Icons);
    }

    [UnityEditor.MenuItem("AceViral/Generate Icons/All")]
    static void GenerateIconsForAllPlatforms()
    { 
        GenerateIconsForPlatform(eIconTypes.AllIcons);
    }

    static void GenerateIconsForPlatform(eIconTypes type)
    {
        Texture2D texture = null;

        if(Selection.activeObject != null && Selection.activeObject.GetType() == typeof(Texture2D))
            texture = (Texture2D)Selection.activeObject;

        // If texture selected, give user option to use this image
        if (texture != null)
        {

            int returnType = EditorUtility.DisplayDialogComplex(
                "Generate Icons",
                "Would you like to use the selected image?",
                "Use Selected",
                "Cancel",
                "Load From File");

            if (returnType == 0) // Pressed use selected image
            {
                GenerateAndroidIcons(texture);
                return;
            }
            else if (returnType == 1) // Pressed cancel
            {
                return;
            }
            else if (returnType == 2) // Pressed load other
            {
                LoadFileForPlatform(type);
                return;
            }
        }

        // If no texture selected, load from file
        LoadFileForPlatform(type);
    }

    static void LoadFileForPlatform(eIconTypes type)
    {
        string path = EditorUtility.OpenFilePanel(
                          "Select Icon image",
                          "",
                          "png");
        if (path.Length != 0)
        {
            WWW www = new WWW("file:///" + path);
            Texture2D texture = new Texture2D(0, 0);

            www.LoadImageIntoTexture(texture);

            if (type == eIconTypes.AndroidIcons)
            {
                GenerateAndroidIcons(texture);
            }
            else if (type == eIconTypes.iPhoneIcons)
            {
                GenerateIPhoneIcons(texture);
            }
            else if (type == eIconTypes.WP8Icons)
            {
                GenerateWP8Icons(texture);
            }
            else if (type == eIconTypes.AllIcons)
            {
                GenerateAndroidIcons(texture);
                GenerateIPhoneIcons(texture);
                GenerateWP8Icons(texture);
            }
        }
    }

    static Texture2D GetResizedTexture(Texture2D texture, int width, int height)
    {
        // Convert the texture to a format compatible with TextureScale
        if (texture.format != TextureFormat.RGBA32 && texture.format != TextureFormat.RGB24)
        {
			Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            newTexture.SetPixels(texture.GetPixels(0), 0);
            texture = newTexture;
        }

		Texture2D resized = new Texture2D(texture.width, texture.height, texture.format, false);
        resized.SetPixels(texture.GetPixels(0), 0);
        TextureScale.Bilinear (resized, width, height);

        return resized;
    }

    static void SaveTextureToPath(Texture2D texture, string path)
    {
        // Convert the texture to a format compatible with EncodeToPNG
        if (texture.format != TextureFormat.ARGB32 && texture.format != TextureFormat.RGB24)
        {
			Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            newTexture.SetPixels(texture.GetPixels(0), 0);
            texture = newTexture;
        }

        File.Delete(path);

        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
            File.WriteAllBytes(path, pngData);
    }

    static void GenerateAndroidIcons(Texture2D texture)
    {
        string resPath = Application.dataPath + "/Plugins/Android/res/";

        Texture2D tex72 = GetResizedTexture(texture, 72, 72);
        Texture2D tex96 = GetResizedTexture(texture, 96, 96);
        Texture2D tex114 = GetResizedTexture(texture, 144, 144);

        // 144 pixels
        SaveTextureToPath(tex114, resPath + "drawable/ic_launcher.png");
        SaveTextureToPath(tex114, resPath + "drawable/ic_notification.png");

        // 96 pixels
        SaveTextureToPath(tex96, resPath + "drawable-hdpi/ic_launcher.png");
        SaveTextureToPath(tex96, resPath + "drawable-hdpi/ic_notification.png");

        // 72 pixels
        SaveTextureToPath(tex72, resPath + "drawable-ldpi/ic_launcher.png");
        SaveTextureToPath(tex72, resPath + "drawable-ldpi/ic_notification.png");

        // 72 pixels
        SaveTextureToPath(tex72, resPath + "drawable-mdpi/ic_launcher.png");
        SaveTextureToPath(tex72, resPath + "drawable-mdpi/ic_notification.png");

        // 144 pixels
        SaveTextureToPath(tex114, resPath + "drawable-nodpi/ic_launcher.png");
        SaveTextureToPath(tex114, resPath + "drawable-nodpi/ic_notification.png");

        // 144 pixels
        SaveTextureToPath(tex114, resPath + "drawable-xhdpi/ic_launcher.png");
        SaveTextureToPath(tex114, resPath + "drawable-xhdpi/ic_notification.png");

        Debug.Log("Andriod Icons Generated.");
    }

    static void GenerateIPhoneIcons(Texture2D texture)
    {
        string iconPath = Application.dataPath + "/Plugins/iOS/Icons/";

        if (!File.Exists(iconPath))
        {
            Directory.CreateDirectory(iconPath);
        }

        SaveTextureToPath(GetResizedTexture(texture, 72, 72), iconPath + "Icon-72.png");
        SaveTextureToPath(GetResizedTexture(texture, 144, 144), iconPath + "Icon-72@2x.png");
        SaveTextureToPath(GetResizedTexture(texture, 76, 76), iconPath + "Icon-76.png");
        SaveTextureToPath(GetResizedTexture(texture, 152, 152), iconPath + "Icon-76@2x.png");
        SaveTextureToPath(GetResizedTexture(texture, 120, 120), iconPath + "Icon-120.png");
        SaveTextureToPath(GetResizedTexture(texture, 100, 100), iconPath + "Icon-iPad-Spotlight-iOS7@2x.png");
        SaveTextureToPath(GetResizedTexture(texture, 50, 50), iconPath + "Icon-Small-50.png");
        SaveTextureToPath(GetResizedTexture(texture, 100, 100), iconPath + "Icon-Small-50@2x.png");
        SaveTextureToPath(GetResizedTexture(texture, 29, 29), iconPath + "Icon-Small.png");
        SaveTextureToPath(GetResizedTexture(texture, 58, 58), iconPath + "Icon-Small@2x.png");
        SaveTextureToPath(GetResizedTexture(texture, 40, 40), iconPath + "Icon-Spotlight-iOS7.png");
        SaveTextureToPath(GetResizedTexture(texture, 80, 80), iconPath + "Icon-Spotlight-iOS7@2x.png");
        SaveTextureToPath(GetResizedTexture(texture, 57, 57), iconPath + "Icon.png");
        SaveTextureToPath(GetResizedTexture(texture, 114, 114), iconPath + "Icon@2x.png");

        Debug.Log("iPhone Icons Generated.");
    }

    static void GenerateWP8Icons(Texture2D texture)
    {
		string iconPath = Application.dataPath + "/Plugins/WP8/Icons/";

        if (!File.Exists(iconPath))
        {
            Directory.CreateDirectory(iconPath);
        }

		SaveTextureToPath(GetResizedTexture(texture, 126, 126), iconPath + "Square 126.png");
		SaveTextureToPath(GetResizedTexture(texture, 98, 98), iconPath + "Square 98.png");
		SaveTextureToPath(GetResizedTexture(texture, 70, 70), iconPath + "Square 70.png");
		SaveTextureToPath(GetResizedTexture(texture, 56, 56), iconPath + "Square 56.png");
		SaveTextureToPath(GetResizedTexture(texture, 270, 270), iconPath + "Square 270.png");
		SaveTextureToPath(GetResizedTexture(texture, 210, 210), iconPath + "Square 210.png");
		SaveTextureToPath(GetResizedTexture(texture, 150, 150), iconPath + "Square 150.png");
		SaveTextureToPath(GetResizedTexture(texture, 120, 120), iconPath + "Square 120.png");
		SaveTextureToPath(GetResizedTexture(texture, 558, 558), iconPath + "Square 558.png");
		SaveTextureToPath(GetResizedTexture(texture, 434, 434), iconPath + "Square 434.png");
		SaveTextureToPath(GetResizedTexture(texture, 310, 310), iconPath + "Square 310.png");
		SaveTextureToPath(GetResizedTexture(texture, 248, 248), iconPath + "Square 248.png");
		SaveTextureToPath(GetResizedTexture(texture, 54, 54), iconPath + "Square 54.png");
		SaveTextureToPath(GetResizedTexture(texture, 42, 42), iconPath + "Square 42.png");
		SaveTextureToPath(GetResizedTexture(texture, 30, 30), iconPath + "Square 30.png");
		SaveTextureToPath(GetResizedTexture(texture, 24, 24), iconPath + "Square 24.png");
		SaveTextureToPath(GetResizedTexture(texture, 256, 256), iconPath + "Square 256.png");
		SaveTextureToPath(GetResizedTexture(texture, 48, 48), iconPath + "Square 48.png");
		SaveTextureToPath(GetResizedTexture(texture, 32, 32), iconPath + "Square 32.png");
		SaveTextureToPath(GetResizedTexture(texture, 16, 16), iconPath + "Square 16.png");
		SaveTextureToPath(GetResizedTexture(texture, 90, 90), iconPath + "Square 90.png");
		SaveTextureToPath(GetResizedTexture(texture, 50, 50), iconPath + "Square 50.png");
		SaveTextureToPath(GetResizedTexture(texture, 43, 43), iconPath + "Square 43.png");
		SaveTextureToPath(GetResizedTexture(texture, 33, 33), iconPath + "Square 33.png");
		SaveTextureToPath(GetResizedTexture(texture, 24, 24), iconPath + "Square 24.png");

		string path = EditorUtility.OpenFilePanel(
			"Select 558 x 270 Icon image",
			"",
			"png");
		if (path.Length != 0) {
			WWW www = new WWW ("file:///" + path);
			texture = new Texture2D (0, 0);

			www.LoadImageIntoTexture (texture);

			SaveTextureToPath(GetResizedTexture(texture, 558, 270), iconPath + "Wide 558x270.png");
			SaveTextureToPath(GetResizedTexture(texture, 434, 210), iconPath + "Wide 434x210.png");
			SaveTextureToPath(GetResizedTexture(texture, 310, 150), iconPath + "Wide 310x150.png");
			SaveTextureToPath(GetResizedTexture(texture, 248, 120), iconPath + "Wide 248x120.png");

			Debug.Log("WP8 Icons Generated.");
		}
    }
}
