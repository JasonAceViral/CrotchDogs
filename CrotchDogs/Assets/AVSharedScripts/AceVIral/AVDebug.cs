using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace AceViral {
    public class AVDebug : MonoBehaviour {

        public static void Log(string output){
            #if UNITY_EDITOR
            //      Debug.Log ("[EDITOR] "+output);
            #endif
        }


        public static void LogUIAction (string output) {
            #if UNITY_EDITOR
            //Debug.Log ("[UI] " + output);
            #endif
        }

        public static void LogIOAction (string output) {
            #if UNITY_EDITOR
            //Debug.Log ("[IO] " + output);
            #endif
        }

        public static void LogNetworkAction (string output) {
            LogNetworkAction (output, false);
        }

        public static void LogNetworkAction (string output, bool error) {
            if (error) {
                Debug.LogWarning ("[Network]" + output);
            } else {
                #if UNITY_EDITOR
                //          Debug.Log ("[Network]" + output);
                #endif
            }
        }

        public static void LogUnitTest (string output) {
            #if UNITY_EDITOR
            Debug.Log("[Unit]" + output);
            #endif
        }

        public static void DumpException( Exception ex )
        {
            Debug.LogException(ex);
            Debug.LogWarning( "--------- Outer Exception Data ---------" );
            WriteExceptionInfo( ex );
            ex = ex.InnerException;                     
            if( null != ex )               
            {                                   
                Debug.LogWarning( "--------- Inner Exception Data ---------" );
                WriteExceptionInfo( ex.InnerException );    
                ex = ex.InnerException;
            }
        }
        public static void WriteExceptionInfo( Exception ex )
        {
            StringBuilder str = new StringBuilder ();
            str.Append ("Message: ");str.Append(ex.Message);str.AppendLine ();
            str.Append( "Message: " );str.Append( ex.Message );str.AppendLine ();
            str.Append ("Exception Type: " );str.Append( ex.GetType ().FullName);str.AppendLine ();
            str.Append( "Source: " );str.Append( ex.Source );str.AppendLine ();
            str.Append( "StrackTrace: " );str.Append( ex.StackTrace );str.AppendLine ();
            str.Append( "TargetSite: " );str.Append( ex.TargetSite );
            Debug.LogError (str.ToString ());
        }
    }
}
