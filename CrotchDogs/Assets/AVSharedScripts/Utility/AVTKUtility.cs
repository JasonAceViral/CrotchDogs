using System;
using UnityEngine;

namespace AceViral {
	public class AVTKUtility {
		public static void SetLabelMaxSize(tk2dTextMesh lbl, float size){
            SetLabelMaxSize(lbl, size, Vector3.one);
		}

        public static void SetLabelMaxSize(tk2dTextMesh lbl, float size, Vector3 maxScale){
            lbl.scale = maxScale;
            float w = lbl.GetEstimatedMeshBoundsForString (lbl.text).size.x;
            if (w > size) {
                float diff = size / w;
                lbl.scale = new Vector3 (diff*maxScale.x,diff*maxScale.y,1);
            }
        }
	}
}

