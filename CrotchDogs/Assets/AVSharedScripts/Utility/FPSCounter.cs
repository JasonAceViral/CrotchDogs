/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Annop "Nargus" Prapasapong
 * Created: 7 June 2012
 * *************************************************************************/

using UnityEngine;
using System.Collections;

namespace AceViral {
    public class FPSCounter : MonoBehaviour {
       
        public float frequency = 0.5f;
        public int FramesPerSec { get; protected set; }

        private tk2dTextMesh fpsLabel;


        // Use this for initialization
        void Start () {

            fpsLabel = GetComponent<tk2dTextMesh>();
            fpsLabel.transform.position = new Vector3(-Screen.width * 0.5f / 100f, -Screen.height * 0.5f / 100f, fpsLabel.transform.position.z);

            StartCoroutine(FPS());
        }

        private IEnumerator FPS() {
            for(;;){
                // Capture frame-per-second
                int lastFrameCount = Time.frameCount;
                float lastTime = Time.realtimeSinceStartup;
                yield return new WaitForSeconds(frequency);
                float timeSpan = Time.realtimeSinceStartup - lastTime;
                int frameCount = Time.frameCount - lastFrameCount;

                // Display it
                FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
                fpsLabel.text = "FPS: " + FramesPerSec.ToString();
            }
        }
    }
}