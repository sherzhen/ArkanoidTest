using System.Collections;
using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera = null;

        #region Camera BG color changing

        /// <summary>Returns true when the camera has finished changing the background color</summary>
        public bool IsCameraColorChanged { get; private set; } = false;

        public void SetBackgroundColor(Color newColor)
        {
            IsCameraColorChanged = false;

            StartCoroutine(BackgroundColorChanging(newColor));
        }

        private Color oldColor;
        private float transitionTime = 0;

        private IEnumerator BackgroundColorChanging(Color newColor)
        {
            oldColor = mainCamera.backgroundColor;
            transitionTime = 0;

            while (mainCamera.backgroundColor != newColor)
            {
                transitionTime += Time.deltaTime;
                mainCamera.backgroundColor = Color.Lerp(oldColor, newColor, transitionTime);
                yield return null;
            }

            IsCameraColorChanged = true;
        }
        #endregion
    }
}