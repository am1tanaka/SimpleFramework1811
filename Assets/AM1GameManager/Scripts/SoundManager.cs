using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM1
{
    [System.Serializable]
    public struct AudioClipData {
        /// <summary>
        /// オーディオクリップ名。これを指定して再生。
        /// 同じ名前は使えない。省略すると自動的に
        /// エレメント番号の数値が設定されます。
        /// </summary>
        public string name;
        /// <summary>
        /// オーディオクリップ
        /// </summary>
        public AudioClip audioClip;
    }

    public class SoundManager : MonoBehaviour
    {
        [Header("オーディオ")]
        [Tooltip("BGMの音源リスト"), SerializeField]
        AudioClipData[] bgmList;
        [Tooltip("SEの音源リスト"), SerializeField]
        AudioClipData[] seList;
        [Header("---")]
        [Tooltip("BGM用のAudioSource"), SerializeField]
        AudioSource bgmAudio;
        [Tooltip("2DのSE用のAudioSource"), SerializeField]
        AudioSource seAudio;

        private void Awake()
        {
            /// 名無しチェック
            for (int i=0; i<bgmList.Length;i++)
            {
                if (bgmList[i].name.Length == 0)
                {
                    bgmList[i].name = i.ToString();
                }
            }
            for (int i=0;i<seList.Length;i++)
            {
                if (seList[i].name.Length == 0)
                {
                    seList[i].name = i.ToString();
                }
            }
        }

        /// <summary>
        /// Bgm Listに設定したnameを指定して、BGMを再生します。
        /// すでに同じ曲を再生中の時は、ボリュームの最大化のみ行います。
        /// </summary>
        /// <param name="name">Bgm Listのname</param>
        public void PlayBGM(string name)
        {
            AudioClip ac = getBGM(name);
            if (ac != null)
            {
                if (bgmAudio.isPlaying)
                {
                    // 同じ曲を再生中なので何もしません
                    if (bgmAudio.clip == ac)
                    {
                        SetBGMVolume(1f);
                        return;
                    }

                    bgmAudio.Stop();
                }

                bgmAudio.clip = ac;
                SetBGMVolume(1f);
                bgmAudio.Play();
            }
            else
            {
                Debug.LogWarning("PlayBGM: " + name + "という名前のBGMがBgm Listにありませんでした。");
            }
        }

        /// <summary>
        /// Se Listに設定したnameを指定して、SEを鳴らします。
        /// </summary>
        /// <param name="name">Se Listのname</param>
        public void PlaySE(string name)
        {
            AudioClip ac = getSE(name);
            if (ac != null)
            {
                seAudio.PlayOneShot(ac);
            }
            else
            {
                Debug.LogWarning("PlaySE: " + name + "という名前のSEがSe Listにありませんでした。");
            }
        }

        /// <summary>
        /// BGMが再生中だったら停止します。
        /// </summary>
        public void StopBGM()
        {
            if (bgmAudio.isPlaying)
            {
                bgmAudio.Stop();
                bgmAudio.clip = null;
            }
        }

        /// <summary>
        /// 効果音を停止します。
        /// </summary>
        public void StopSE()
        {
            seAudio.Stop();
        }

        AudioClip getBGM(string name)
        {
            for (int i=0; i<bgmList.Length;i++)
            {
                if (bgmList[i].name == name)
                {
                    return bgmList[i].audioClip;
                }
            }
            return null;
        }

        AudioClip getSE(string name)
        {
            for (int i = 0; i < seList.Length; i++)
            {
                if (seList[i].name == name)
                {
                    return seList[i].audioClip;
                }
            }
            return null;
        }

        /// <summary>
        /// BGMのボリュームを設定します。
        /// </summary>
        /// <param name="v">ボリューム</param>
        public void SetBGMVolume(float v)
        {
            bgmAudio.volume = Mathf.Clamp(v, 0, 1);
        }
    }
}