using UnityEngine;
using System.Collections;

public class StretchBackground : MonoBehaviour
{
    float screenWidth, screenHeight;
    public tk2dCamera cam;
    tk2dBaseSprite backgroundSprite;
    float spriteWidth, spriteHeight;
    public bool enableXAxis = true;
    public bool enableYAxis = true;
    public bool maintainAspect;

	public float Width_Modifier = 1.01f, Height_Modifier = 1.01f;

    // Use this for initialization
    void Awake()
    {
		//if Camera has not been set
		if (cam == null) 
		{
			cam = (tk2dCamera.Instance);
		}

        screenWidth = cam.ScreenExtents.width;
        screenHeight = cam.ScreenExtents.height;
		Debug.Log ("w,h" + screenWidth +"," + screenHeight );

		if (screenWidth == 0) 
		{
				return;
		}

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);

        MeshRenderer meshRend = GetComponent<MeshRenderer>();
       
        spriteWidth = meshRend.bounds.size.x / gameObject.transform.localScale.x;
        spriteHeight = meshRend.bounds.size.y / gameObject.transform.localScale.y;

        // Multiply by a bit more to overlap the screen edges a bit
		float requiredXScale = Width_Modifier * screenWidth / spriteWidth;
		float requiredYScale = Height_Modifier * screenHeight / spriteHeight;

        Vector3 scale = transform.localScale;

        if (enableXAxis && !enableYAxis)
        {
            scale = new Vector3(requiredXScale, requiredXScale, 1);
        }
        else if (!enableXAxis && enableYAxis)
        {
            scale = new Vector3(requiredYScale, requiredYScale, 1);
        }
        else if (maintainAspect)
        {
            if (requiredXScale > requiredYScale)
            {
                scale = new Vector3(requiredXScale, requiredXScale, 1);
            }
            else
            {
                scale = new Vector3(requiredYScale, requiredYScale, 1);
            }
        }
        else
        {
            scale = new Vector3(requiredXScale, requiredYScale, 1);
        }

		

		transform.localScale = scale;
    }
}