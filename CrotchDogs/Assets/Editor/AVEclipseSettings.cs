using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Xml;

namespace AVSharedScripts {
    class AVEclipseSettings : EditorWindow
    {
        private static string _eclipseLocation;

        public static string EclipseLocation
        {
            get
            {
                ReadXMLData();
                return _eclipseLocation;
            }
            protected set { _eclipseLocation = value; }
        }

        [MenuItem("AceViral/Build/Eclipse Settings..")]
        public static void ShowWindow()
        {
            ReadXMLData();
            EditorWindow.GetWindow(typeof(AVEclipseSettings));
        }

        private static void ReadXMLData()
        {
            _eclipseLocation = null;
            
            try
            {
                using (XmlReader reader = XmlReader.Create("eclipseSettings.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "settings")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "eclipseLocation")
                                {
                                    _eclipseLocation = reader.ReadString();
                                    break;
                                }
                            }
                            if (_eclipseLocation != null)
                                break;
                        }
                    }       
                }       
            }
            catch (FileNotFoundException e)
            {
                Debug.LogWarning("No eclipse settings found: " + e.FileName);
            }
            
        }

        void OnGUI()
        {
            GUILayout.Label("Eclipse Location", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Eclipse Location", _eclipseLocation);
            if (GUILayout.Button("Select", GUILayout.Width(80.0f)))
            {
                _eclipseLocation = EditorUtility.OpenFilePanel("Where is Eclipse?", "", "app");
                
                XmlDocument settings = new XmlDocument();
                XmlElement xml = (XmlElement)settings.AppendChild(settings.CreateElement("settings"));
            
                AddXMLElement(settings, xml, "eclipseLocation", _eclipseLocation);                

                settings.Save("eclipseSettings.xml");
            }
            EditorGUILayout.EndHorizontal();
        }

        static void AddXMLElement(XmlDocument doc, XmlElement parent, string elementType, string value)
        {
            XmlElement str = doc.CreateElement(elementType);
            str.InnerText = value;
            parent.AppendChild(str);
        }
    }
}
