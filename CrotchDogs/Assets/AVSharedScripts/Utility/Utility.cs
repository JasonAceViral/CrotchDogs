using System;
using System.Text;
using UnityEngine;

namespace AceViral {
    public class Utility : MonoBehaviour {
        // Some default Quaternions (since Quaternion.Euler can sometimes be inefficient)
        public static Quaternion Quaternion90DegreesY = new Quaternion(0.0f, 0.7f, 0.0f, 0.7f);
        public static Quaternion Quaternion180DegreesY = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
        public static Quaternion Quaternion270DegreesY = new Quaternion(0.0f, 0.7f, 0.0f, -0.7f);

        public static GameObject CreatePlaneWithTexture(string textureName) {
            try {
                GameObject planeObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                planeObject.renderer.material.mainTexture = Resources.Load(textureName) as Texture;
                return planeObject;
            } catch (Exception e) {
                Debug.LogError("AVUtility: CreatePlaneWithTexture(" + textureName + ") - * Exception occurred:\n" + e.Message + "*");
            }
            return null;
        }

        public static GameObject Instantiate(GameObject prefab) {
            return (GameObject)GameObject.Instantiate(prefab);
        }

        public static void TweenPositionEaseInOut(GameObject obj, float duration, Vector3 position) {
            //TweenPosition posTween = TweenPosition.Begin(obj, duration, position);
            //posTween.method = UITweener.Method.EaseInOut;
        }

        public static void TweenScaleEaseInOut(GameObject obj, float duration, Vector3 position) {
            //TweenScale scaleTween = TweenScale.Begin(obj, duration, position);
            //scaleTween.method = UITweener.Method.EaseInOut;
        }

        /// <summary>
        /// Aligns the user interface object to center in a really hacky way.
        /// Useful for scaling up and down from the center
        /// </summary>
        /// <param name='obj'>
        /// Object to align
        /// </param>
        //	public static void AlignUIObjectCenter(UIObject obj){
        //		
        //		UIObject objParent = obj.parentUIObject;
        //		
        //		obj.parentUIObject = null;
        //		
        //		UIObject tempObj = new UIObject();
        //		tempObj.parentUIObject = obj;
        //		tempObj.positionCenter();
        //		tempObj.parentUIObject = null;
        //		
        //		
        //		obj.parentUIObject = tempObj;
        //		obj.positionCenter();
        //		
        //		obj.parentUIObject = objParent;
        //		
        //		Destroy(tempObj.client);
        //		tempObj = null;
        //	}

        private const float TARGET_DPI = 300.0f;

        private static string m_strIntValue;
        //private static int commaIndex = 3;
        private static string m_strReturnVal;
        public static string NumberWithCommas(int val) {
            return val.ToString("N0");
            //#if UNITY_FLASH
            //		return val.ToString();	
            //#endif
            //		m_strIntValue = val.ToString();
            //		m_strReturnVal = "";
            //		commaIndex = 3;
            //		for (int i = m_strIntValue.Length-1; i >= 0; i--) {
            //			m_strReturnVal = m_strIntValue [i] + m_strReturnVal;
            //			commaIndex--;
            //			if (commaIndex == 0) {
            //				commaIndex = 3;
            //				if (i != 0) {
            //					m_strReturnVal = ',' + m_strReturnVal;
            //				}
            //			}
            //		}
            //		return m_strReturnVal;
        }

        public static int RoundToNumber(int val, int target) {
            int ret = 0;
            while (val >= target) {
                ret += target;
                val -= target;
            }
            return ret;
        }

        public static string ShortenName(string name, int length) {
            string retVal = name;
            if (retVal.Length > length) {
                string[] split = name.Split(' ');
                retVal = split[0] + " " + split[1][0] + ".";
            }
            if (retVal.Length > length) {
                retVal = name.Substring(0, length);
            }
            return retVal;
        }

        public static int RoundUp(float val) {
            int roundedDown = (int)val;
            float remainder = val - (float)roundedDown;
            if (remainder > 0.0f) {
                return roundedDown + 1;
            }
            return roundedDown;
        }

        public static int RoundFloatToInt(float val) {
            int roundedDown = (int)val;
            float remainder = val - (float)roundedDown;
            if (remainder > 0.5f) {
                return roundedDown + 1;
            }
            return roundedDown;
        }

        public static double Round(double value, int digits) {
            if ((digits < -15) || (digits > 15))
                throw new ArgumentOutOfRangeException("digits", "Rounding digits must be between -15 and 15, inclusive.");

            if (digits >= 0)
                return Math.Round(value, digits);

            double n = Math.Pow(10, -digits);
            return Math.Round(value / n, 0) * n;
        }

        //public static decimal Round(decimal d, int decimals)
        //{
        //    if ((decimals < -28) || (decimals > 28))
        //        throw new ArgumentOutOfRangeException("decimals", "Rounding decimals must be between -28 and 28, inclusive.");

        //    if (decimals >= 0)
        //        return decimal.Round(d, decimals);

        //    decimal n = (decimal)Math.Pow(10, -decimals);
        //    return decimal.Round(d / n, 0) * n;
        //}


        public static string ScrambleString(string src) {
            string endString = "";
            char[] c = src.ToCharArray();
            bool front = false;

            for (int i = 0; i < c.Length; i++) {
                c[i] = Convert.ToChar((int)c[i] + ((front) ? 4 : -3));
                if (c[i] < '!')
                    c[i] = '!';
                if (c[i] > 'z')
                    c[i] = 'z';
                if (front)
                    endString = c[i] + endString;
                else
                    endString = endString + c[i];
            }
            return endString;
        }

        public static string ScrambleStringFlip(string src) {
            string endString = "";
            char[] c = src.ToCharArray();
            bool front = false;

            for (int i = 0; i < c.Length; i++) {
                c[i] = Convert.ToChar((int)c[i] + ((front) ? 4 : -3));
                if (c[i] < '!')
                    c[i] = '!';
                if (c[i] > 'z')
                    c[i] = 'z';
                if (front)
                    endString = c[i] + endString;
                else
                    endString = endString + c[i];

                front = !front;
            }
            return endString;
        }

        public static GameObject CreatePlane(float size) {
            Mesh m = new Mesh();
            m.name = "Scripted_Plane_New_Mesh";
            m.vertices = new Vector3[] { new Vector3(-size, -size, 0.01f), new Vector3(size, -size, 0.01f), new Vector3(size, size, 0.01f), new Vector3(-size, size, 0.01f) };
            m.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
            m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            m.RecalculateNormals();
            GameObject obj = new GameObject("New_Plane_Fom_Script");
            obj.AddComponent<MeshFilter>().mesh = m;
            obj.AddComponent<MeshRenderer>();
            return obj;
        }

        public static GameObject CreateBillboardWithMaterial(Material mat, float size) {
            GameObject plane = CreatePlane(size);
            plane.AddComponent<BillboardPlane>().rotated = false;
            plane.renderer.material = mat;
            return plane;
        }

        public static bool IsIOSDeviceLowEnd() {
    #if UNITY_IPHONE
    		if(iPhone.generation == iPhoneGeneration.iPad1Gen || 
    			iPhone.generation == iPhoneGeneration.iPhone || 
    			iPhone.generation == iPhoneGeneration.iPhone3G || 
    			iPhone.generation == iPhoneGeneration.iPhone3GS ||
    			iPhone.generation == iPhoneGeneration.iPodTouch1Gen ||
    			iPhone.generation == iPhoneGeneration.iPodTouch2Gen ||
    			iPhone.generation == iPhoneGeneration.iPodTouch3Gen ||
    			iPhone.generation == iPhoneGeneration.iPodTouch4Gen ||
    			iPhone.generation == iPhoneGeneration.iPhone4 ||
    			Application.platform == RuntimePlatform.OSXEditor){
    			return true;	
    		}
    #endif
            return false;
        }

        public static bool IsIOSDeviceRetina5Inch() {
    #if UNITY_EDITOR
            return false;
    #elif UNITY_IPHONE
    		if(iPhone.generation == iPhoneGeneration.iPhone5 || iPhone.generation == iPhoneGeneration.iPodTouch5Gen || Screen.height == 1136){
    			return true;	
    		}  else {
    			return false;	
    		}
    #else
    		return false;
    #endif
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public static string WordWrap(string text, int width) {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next) {
                // Find end of line
                int eol = text.IndexOf("\n", pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + "\n".Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos) {
                    do {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append("\n");

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                } else sb.Append("\n"); // Empty line
            }
            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max) {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }
}
