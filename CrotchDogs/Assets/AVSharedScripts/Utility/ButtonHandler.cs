using UnityEngine;
using System.Collections;

namespace AceViral {
    public abstract class ButtonHandler : MonoBehaviour
    {
        abstract public void MouseUp(string buttonName);
    	public virtual void MouseDown(string buttonName) { }
    }
}
