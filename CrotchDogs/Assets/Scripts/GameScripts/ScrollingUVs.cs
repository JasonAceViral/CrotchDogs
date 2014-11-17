using UnityEngine;
using System.Collections;

public class ScrollingUVs: MonoBehaviour {

	public int materialIndex = 0;
	Vector2 uvOffset = Vector2.zero;
	public Vector2 uvAnimationRate = new Vector2( 5.0f, 0.0f );

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( gameObject.renderer.enabled )
		{

						gameObject.renderer.material.SetTextureOffset( "_MainTex", uvOffset );
		}
	}
}
