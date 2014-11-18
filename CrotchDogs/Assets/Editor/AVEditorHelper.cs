using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System;

class AVEditorHelper
{

    [UnityEditor.MenuItem("AceViral/Android APK/Build")]
    static void BuildAndroidProject()
    {
        string path = /*Application.dataPath + "/" +*/ (AVAppConstants.CompileForAmazonAppStore ? "AmazonBuild" : "AndroidBuild");//EditorUtility.SaveFolderPanel("Choose Location of Project", "", "");
        string[] levels = GetBuildScenes ();

        string[] s = Application.dataPath.Split('/');
				string projectName = "Crotch Dogs"; //s[s.Length - 2];
        UnityEngine.Debug.Log("project = " + projectName);

        XmlDocument doc = new XmlDocument();

        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("resources"));

        XmlElement str1 = doc.CreateElement("string");
        str1.SetAttribute("name", "ga_trackingId");
        str1.InnerText = AVAppConstants.AndroidGoogleAnalyticsId;
        el.AppendChild(str1);

        XmlElement str2 = doc.CreateElement("bool");
        str2.SetAttribute("name", "ga_autoActivityTracking");
        str2.InnerText = "true";
        el.AppendChild(str2);

        XmlElement str3 = doc.CreateElement("bool");
        str3.SetAttribute("name", "ga_reportUncaughtExceptions");
        str3.InnerText = "true";
        el.AppendChild(str3);

        XmlElement str7 = doc.CreateElement("string");
        str7.SetAttribute("name", "adcolonyappid");
        str7.InnerText = AVAppConstants.AndroidAdColonyAppID;
        el.AppendChild(str7);

        XmlElement str8 = doc.CreateElement("string");
        str8.SetAttribute("name", "adcolonyzoneid");
        str8.InnerText = AVAppConstants.AndroidAdColonyZoneID;
        el.AppendChild(str8);


       
        if(File.Exists(path+"/"+projectName+"/project.properties"))
        {
            if(File.Exists(path + "/project.properties"))
            {
                File.Delete(path + "/project.properties");
                File.Delete(path + "/AndroidManifest.xml");
            }        
            File.Copy(path+"/"+projectName+"/project.properties", path + "/project.properties");
            File.Copy(path+"/"+projectName+"/AndroidManifest.xml", path + "/AndroidManifest.xml");
        }

        BuildPipeline.BuildPlayer(levels, path, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);

        if(File.Exists(path + "/project.properties"))
        {
            File.Delete(path+"/"+projectName+"/project.properties");
            File.Delete(path+"/"+projectName+"/AndroidManifest.xml");
            File.Copy(path + "/project.properties",path+"/"+projectName+"/project.properties");
            File.Copy(path + "/AndroidManifest.xml",path+"/"+projectName+"/AndroidManifest.xml");
            File.Delete(path + "/Androidmanifest.xml");
        }


        doc.Save(path+"/"+projectName+"/res/values/avkeys.xml");


        XmlDocument doc2 = new XmlDocument();
        el = (XmlElement)doc2.AppendChild(doc2.CreateElement("resources"));
                
        XmlElement str4 = doc2.CreateElement("string");
        str4.SetAttribute("name", "google_play_app_id");
        str4.InnerText = AVAppConstants.AndroidGooglePlayId;
        el.AppendChild(str4);

        XmlElement str5 = doc2.CreateElement("string");//facebook app id
        str5.SetAttribute("name", "applicationId");
        str5.InnerText = AVAppConstants.facebookAppId;
        el.AppendChild(str5);
                
        XmlElement str6 = doc2.CreateElement("integer");
        str6.SetAttribute("name", "google_play_services_version");
        str6.InnerText = "4030500";
        el.AppendChild(str6);

        doc2.Save(path+"/"+projectName+"/res/values/version.xml");
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

    [UnityEditor.MenuItem("AceViral/Android APK/Reset APK Paths")]
    static void ResetInstallPaths()
    { 
        PlayerPrefs.DeleteKey("AVEditor-AdbLocation");
        PlayerPrefs.DeleteKey("AVEditor-AbdScriptLocation");
        PlayerPrefs.Save();
        UnityEngine.Debug.Log("APK Paths reset.");
    }

    [UnityEditor.MenuItem("AceViral/Android APK/Install")]
    static void InstallAndroidAPK()
    { 
        // Recover existing ADB location
        string adbLocation = PlayerPrefs.GetString("AVEditor-AdbLocation");

        if (adbLocation.Length == 0)
        {
            string adbpath = EditorUtility.OpenFilePanel(
                                 "Select ADB Installer",
                                 "",
                                 "");

            if (adbpath.Length != 0)
            {
                adbLocation = adbpath;
                PlayerPrefs.SetString("AVEditor-AdbLocation", adbpath);
                PlayerPrefs.Save();
            }
            else
            {
                UnityEngine.Debug.Log("Install APK failed. Didn't specify the ADB Installer location.");
                return;
            }
        }


        // Recover existing script location
        string scriptLocation = PlayerPrefs.GetString("AVEditor-AbdScriptLocation");

        if (scriptLocation.Length == 0)
        {
            string scriptpath = EditorUtility.OpenFilePanel(
                                        "Select Shell Script",
                                        "",
                                        "sh");

            if (scriptpath.Length != 0)
            {
                scriptLocation = scriptpath;
                PlayerPrefs.SetString("AVEditor-AbdScriptLocation", scriptpath);
                PlayerPrefs.Save();
            }
            else
            {
                UnityEngine.Debug.Log("Install APK failed. Didn't specify the ADB Installer Script location.");
                return;
            }
        }

        string path = EditorUtility.OpenFilePanel(
                          "Select APK",
                          "",
                          "apk");

        if (path.Length != 0)
        {
            string[] splitScriptPath = SplitFileNameFromDirectory(scriptLocation);

            //Arguments
            // 1. ADB Location
            // 2. APK Location

            ProcessStartInfo proc = new ProcessStartInfo()
            {
                FileName = "sh",
                WorkingDirectory = splitScriptPath[0],
                Arguments = splitScriptPath[1] + " '" + adbLocation + "' '" + path + "'",
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                UseShellExecute = false
            }; 

            Process process = Process.Start(proc);

            string output = process.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(output);

            process.WaitForExit();
        }
        else
        {
            UnityEngine.Debug.Log("Install APK failed. Didn't specify the APK location.");
            return;
        }
    }
        
    static string[] SplitFileNameFromDirectory(string fullPath)
    {
        string[] paths = new string[2] { string.Empty, string.Empty };

        for (int i = fullPath.Length - 2; i >= 0; i--)
        {
            if (fullPath[i] == '/')
            {
                paths[1] = fullPath.Substring(i+1);
                paths[0] = fullPath.Remove(i);
                break;
            }
        }
        return paths;
    }
}
