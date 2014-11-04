using UnityEngine;

public delegate void YesNoDialogFunction(bool yes);

public class AVDialogBox : MonoBehaviour {
    private YesNoDialogFunction m_YesNoFunction = null;

    public static void showConfirmBox(string title, string message) {
        AVMsgBox.Show(title, message);
    }

    public static void CreateYesNoBox(string title, string msg, YesNoDialogFunction func) {

    }

    public void YesNoCallback(string msg) {
        m_YesNoFunction(msg == "true");
        Destroy(this.gameObject);
    }
}