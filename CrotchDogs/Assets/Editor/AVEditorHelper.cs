using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System;
using AceViral;

namespace AVSharedScripts {
    class AVEditorHelper
    {
        [UnityEditor.MenuItem("AceViral/Build/Android Project")]
        static void BuildAndroidProject()
        {
						string pluginFolder = (AppConstants.CompileForAmazonAppStore ? "AmazonBuild" : "AndroidBuild");
            string[] levels = GetBuildScenes ();

            XmlDocument keys = new XmlDocument();
            XmlElement xml = (XmlElement)keys.AppendChild(keys.CreateElement("resources"));

            AddXMLElement(keys, xml, "string", "ga_trackingId", AppConstants.Android.GoogleAnalyticsId);
            AddXMLElement(keys, xml, "bool", "ga_autoActivityTracking", "true");
            AddXMLElement(keys, xml, "bool", "ga_reportUncaughtExceptions", "true");
            //        AddXMLElement(keys, xml, "string", "adcolonyappid", AVAppConstants.AndroidAdColonyAppID);
            //        AddXMLElement(keys, xml, "string", "adcolonyzoneid", AVAppConstants.AndroidAdColonyZoneID);
            //        AddXMLElement(keys, xml, "string", "nativexappid", AVAppConstants.AndroidNativeXAppID);
            //        AddXMLElement(keys, xml, "string", "vungleappid", AVAppConstants.AndroidVungleAppID);
            //        AddXMLElement(keys, xml, "string", "everyplayappid", AVAppConstants.AndroidEveryplayAppID);
            //        AddXMLElement(keys, xml, "string", "everyplayplacementid", AVAppConstants.AndroidEveryplayPlacementID);
            //        AddXMLElement(keys, xml, "string", "chartboostappid", AVAppConstants.AndroidChartboostAppID);
            //        AddXMLElement(keys, xml, "string", "chartboostappsignature", AVAppConstants.AndroidChartboostAppSignature);
            //        AddXMLElement(keys, xml, "string", "plusoneurl", AVAppConstants.AndroidPlusOneURL);


            SafeOverwrite (GetBuildPath(pluginFolder) + "project.properties", pluginFolder + "/project.properties");
            SafeOverwrite (GetBuildPath(pluginFolder) + "AndroidManifest.xml", pluginFolder + "/AndroidManifest.xml");

            BuildPipeline.BuildPlayer(levels, pluginFolder, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);

            SafeOverwrite (pluginFolder + "/project.properties", GetBuildPath(pluginFolder) + "project.properties");
            SafeOverwrite (pluginFolder + "/AndroidManifest.xml", GetBuildPath(pluginFolder) + "AndroidManifest.xml");
            SafeDelete(pluginFolder + "/project.properties");
            SafeDelete(pluginFolder + "/Androidmanifest.xml");

            keys.Save(GetBuildPath(pluginFolder) + "res/values/avkeys.xml");
            XmlDocument versions = new XmlDocument();
            xml = (XmlElement)versions.AppendChild(versions.CreateElement("resources"));

            AddXMLElement(versions, xml, "string", "google_play_app_id", AppConstants.Android.GooglePlayId);
            AddXMLElement(versions, xml, "string", "applicationId", AppConstants.Facebook.AppId);
            //AddXMLElement(versions, xml, "string", "google_play_services_version", "5077000");


            versions.Save(GetBuildPath(pluginFolder) + "res/values/version.xml");

            string classContents = AVAndroidStrings.GetClassToReplaceWith(AppConstants.Android.PackageName);
            File.WriteAllText(GetBuildPath(pluginFolder) + "src/" + AppConstants.Android.PackageId + "UnityPlayerNativeActivity.java", classContents);

            //string properties = "target=android-20\nandroid.library.reference.1=../../Android base/AVBaseUnityProject";
            //File.WriteAllText(pluginFolder + "/" + pluginFolder + "/project.properties", properties);

            UnityEngine.Debug.Log("Finished building: " + GetBuildPath(pluginFolder) + ".");

            if (AVEclipseSettings.EclipseLocation != null)
            {
                UnityEngine.Debug.Log("Switching to Eclipse.");
                Process.Start(AVEclipseSettings.EclipseLocation);
            }
            else
            {
                UnityEngine.Debug.Log("Add Eclipse path to automatically switch to Eclipse after build.");
            }

        }

        static void AddXMLElement(XmlDocument doc, XmlElement parent, string elementType, string name, string value)
        {
            XmlElement str = doc.CreateElement(elementType);
            str.SetAttribute("name", name);
            str.InnerText = value;
            parent.AppendChild(str);
        }

        static string[] GetBuildScenes ()
        {
            List<string> names = new List<string> ();

            foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes) {
                if (e == null)
                    continue;

                if (e.enabled)
                    names.Add (e.path);
            }
            return names.ToArray ();
        }

        [UnityEditor.MenuItem("AceViral/Player Prefs/Reset")]
        static void ResetPlayerPrefs()
        { 
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            UnityEngine.Debug.Log("Player Prefs reset.");
        }

        private static string GetBuildPath(string path){
            return path + "/" + PlayerSettings.productName + "/";
        }

        private static void SafeOverwrite(string src, string dest){
            if (!File.Exists (src)) {
                return;
            }
            SafeDelete (dest);
            File.Copy(src, dest);
        }

        private static void SafeDelete(string src){
            if(File.Exists(src)) {
                File.Delete(src);
            }
        }
    }
}
