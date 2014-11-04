using UnityEngine;
using System.Collections;

public abstract class AVButtonHandler : MonoBehaviour
{

    abstract public void MouseUp(string buttonName);
	public virtual void MouseDown(string buttonName) { }
}
