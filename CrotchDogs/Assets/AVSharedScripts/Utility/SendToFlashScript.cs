using UnityEngine;

namespace AceViral {
    public class SendToFlashScript : MonoBehaviour {
        public object responder;

        void Start() {
            gameObject.name = "SendToFlashScript";
        }

        void SetResponder(object o) {
    #if UNITY_FLASH
    			//This sets the responder objcet to the instance as sent by AS3.
    			ActionScript.Statement("SendToFlashScript$responder$ = {0}.responder", o);
    #endif
        }

        public void OpenBox10() {
    #if UNITY_FLASH
    		if(responder != null) {
    			ActionScript.Statement("SendToFlashScript$responder$['unityOpenBox10']({0})",this);
    		}	
    #endif
        }
    }
}