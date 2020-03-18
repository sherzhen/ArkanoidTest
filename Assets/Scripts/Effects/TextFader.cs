using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiniIT.UIEffects
{
    public class TextFader : MonoBehaviour
    {
        [Tooltip("Minimum alpha channel threshold")]
        [SerializeField]
        private float minFadeValue = 0.3f;

        [Tooltip("Maximum alpha channel threshold")]
        [SerializeField]
        private float maxFadeValue = 1;

        [Tooltip("Speed of fading")]
        [SerializeField]
        private float fadeSpeed = 1;

        private Text text = null;

        void Awake()
        {
            text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            fadeEffectCoroutine = StartCoroutine(FadeEffect());
        }

        Coroutine fadeEffectCoroutine = null;
        IEnumerator FadeEffect()
        {
            Color textColor = text.color;
            textColor.a = maxFadeValue;

            while (true)
            {
                while (textColor.a > minFadeValue)
                {
                    textColor.a -= Time.deltaTime * fadeSpeed;
                    text.color = textColor;
                    yield return null;
                }

                while (textColor.a < maxFadeValue)
                {
                    textColor.a += Time.deltaTime * fadeSpeed;
                    text.color = textColor;
                    yield return null;
                }

                yield return null;
            }
        }

        private void OnDisable()
        {
            StopCoroutine(fadeEffectCoroutine);
        }
    }
}