using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AM1
{
    public class Fade : MonoBehaviour
    {
        /// <summary>
        /// フェードの状態
        /// </summary>
        public enum FADE_STATE
        {
            NONE,
            IN,
            OUT
        }

        /// <summary>
        /// 現在のフェードの状態
        /// </summary>
        public FADE_STATE fadeState
        {
            get;
            private set;
        }

        /// <summary>
        /// フェード実行中の時、true
        /// </summary>
        public bool isFading
        {
            get
            {
                return fadeImage.enabled;
            }
        }

        /// <summary>
        /// フェードのアルファ値を返します。
        /// </summary>
        public float fadeAlpha
        {
            get
            {
                return fadeImage.color.a;
            }
        }

        Image fadeImage;

        private void Awake()
        {
            fadeImage = GetComponent<Image>();
            fadeImage.enabled = false;
            fadeState = FADE_STATE.NONE;
        }

        /// <summary>
        /// フェードインを開始します。
        /// </summary>
        /// <param name="time">フェードインさせる時間</param>
        /// <param name="fadeColor">フェードの色</param>
        /// <returns></returns>
        public IEnumerator FadeIn(float time, Color fadeColor)
        {
            fadeState = FADE_STATE.IN;
            yield return fade(1, 0, time, fadeColor);
            fadeState = FADE_STATE.NONE;
            fadeImage.enabled = false;
        }

        /// <summary>
        /// フェードアウトを開始します。
        /// </summary>
        /// <param name="time">フェードアウトさせる時間</param>
        /// <param name="fadeColor">フェードの色</param>
        /// <returns></returns>
        public IEnumerator FadeOut(float time, Color fadeColor)
        {
            fadeState = FADE_STATE.OUT;
            yield return fade(0, 1, time, fadeColor);
            fadeState = FADE_STATE.NONE;
        }

        IEnumerator fade(float from, float to, float time, Color fadeColor)
        {
            float count = 0;
            Color col = fadeColor;
            col.a = from;
            fadeImage.enabled = true;

            while (count < time)
            {
                fadeImage.color = col;
                yield return null;

                count += Time.unscaledDeltaTime;
                col.a = Mathf.Lerp(from, to, count / time);
            }

            col.a = to;
            fadeImage.color = col;
        }
    }
}