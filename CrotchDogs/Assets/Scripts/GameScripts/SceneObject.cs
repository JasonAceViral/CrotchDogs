using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneObject : MonoBehaviour {

	public List<tk2dSprite> spriteList;
	public float opacity;
	// Use this for initialization
	void Start () {
	
	}
	
	public void Spawn()
	{
				opacity = 0;
				setOpacity (opacity);
	}

	void Update()
	{
			if (opacity < 1.0f) 
			{
				opacity += Time.deltaTime;

				if (opacity > 1.0f) 
				{
					opacity = 1.0f;
				}

				setOpacity (opacity);
			}
	}


		void setOpacity(float newOpacity)
		{
				foreach (tk2dSprite sprite in spriteList) 
				{
						Color c = sprite.color;
						c.a = newOpacity;
						sprite.color = c;
				}
		}

}
