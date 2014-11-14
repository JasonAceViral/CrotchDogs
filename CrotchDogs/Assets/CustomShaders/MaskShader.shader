// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable

// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "Custom/MaskShader" {
	
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
	}
	
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		Pass {
			/*Stencil {
				Ref 0
				Comp always
				Pass replace
			}*/

			ZWrite On
			ColorMask 0

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata v) {
				float4 ver = v.vertex;
				//ver.z += 0.015f;
				v2f o;
				o.pos = mul( _Object2World, ver);
				float additional = o.pos.z * 0.06251f;
				o.pos.z -= additional;
				o.pos = mul(UNITY_MATRIX_VP, o.pos);
				return o;
			}
			half4 frag(v2f i) : COLOR {
				return half4(0,1,0,0);
			}
			ENDCG
			
			
		}
	} 
}

/*	
//	SubShader {
		
//		GrabPass { "_Grab" }
//		
//		sampler2D _Grab;

//		//Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
//		ZWrite On
//		Blend SrcAlpha OneMinusSrcAlpha 
		
//		Pass {
 	
	 		CGPROGRAM
	 		
			// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
			#pragma exclude_renderers gles
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
			
            struct data {
                float4 vertex : SV_POSITION;
            };
            
            struct fragIn {
            	float4 col : COLOR0;
            };

            data vert (data v)
            {
            
            	v.vertex = v.vertex - float4(0.0f, 0.0f, 1.0f, 0.0f);
            
                data o;

               // float4 verPt = 

                o.vertex = mul (UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            
           
            
            
            //uniform sampler2D _MainTex;
            
//           float4 frag() : COLOR 
//           {
            
//				return float4(1.0f, 0.0f, 0.0f, 1.0f);
//			}

//       		ENDCG

//		}
		
//		Pass {
//		
//		}
		
//		Pass {
//			//ZWrite On
//			//AlphaTest Never
//			SetTexture [_MainTex]
//			{
//				Combine Texture * Primary
//			}
//		}
//	}
//}
*/