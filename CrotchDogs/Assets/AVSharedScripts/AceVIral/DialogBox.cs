using UnityEngine;
 
namespace AceViral {
    public delegate void YesNoDialogFunction(bool yes);

    public class DialogBox : MonoBehaviour {
        private YesNoDialogFunction m_YesNoFunction = null;

        public static void showConfirmBox(string title, string message) {
            MsgBox.Show(title, message);
        }

        public static void CreateYesNoBox(string title, string msg, YesNoDialogFunction func) {

        }

        public void YesNoCallback(string msg) {
            m_YesNoFunction(msg == "true");
            Destroy(this.gameObject);
        }
    }
}