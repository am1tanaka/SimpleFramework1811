using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AM1;

public class GameManager : Singleton<GameManager> {

    // ゲーム全体で使うパラメーターを定義します
    #region Game Params

    /// <summary>
    /// ゲーム開始時に呼び出してください。
    /// ゲーム開始時に初期化したいコードを追加してください。
    /// </summary>
    public static void StartInit()
    {
        Score = 0;
        Stage = 0;
        DrawParams();
    }

    /// <summary>
    /// パラメーターを更新します。
    /// </summary>
    public static void DrawParams()
    {
        Instance.scoreText.text = "SCORE " + Score.ToString("D" + Instance.scoreKeta);
        Instance.stageText.text = "Stage " + (Stage + 1).ToString();
    }

    // スコア関連
    #region Score

    // スコアを表示するかフラグ
    [HideInInspector]
    public bool isScoreVisible = true;

    public static int Score
    {
        get;
        private set;
    }
    /// <summary>
    /// 加点します。SCORE_MAXが上限です。
    /// </summary>
    /// <param name="point">追加する点数</param>
    public static void AddScore(int point)
    {
        Score += point;
        if (Score > Instance.SCORE_MAX)
        {
            Score = Instance.SCORE_MAX;
        }
        DrawParams();
    }

    #endregion

    // ステージ数関連
    #region Stage

    // ステージを表示するかフラグ
    [HideInInspector]
    public bool isStageVisible = true;

    public static int Stage
    {
        get;
        private set;
    }
    public static void AddStage()
    {
        Stage++;
    }

    #endregion

    #endregion

    // 基本パラメーター＆コード
    #region am1 code

    [Header("パラメーター関連")]
    [Tooltip("点数上限"), SerializeField]
    int SCORE_MAX = 9999999;
    int scoreKeta;
    [Tooltip("スコアを表示するテキスト"), SerializeField]
    Text scoreText;

    [Tooltip("ステージを表示するテキスト"), SerializeField]
    Text stageText;

    [Header("フェード制御")]
    [Tooltip("デフォルトフェード秒数"), SerializeField]
    float defaultFadeTime = 0.5f;
    [Tooltip("デフォルトフェードカラー"), SerializeField]
    Color defaultFadeColor;

    SoundManager soundManager;
    bool isBGMStop = false;

    /// <summary>
    /// サウンド関連の機能を提供するクラスです。
    /// </summary>
    public static SoundManager SoundManagerInstance
    {
        get
        {
            if (Instance.soundManager == null)
            {
                Instance.soundManager = Instance.GetComponent<SoundManager>();
            }
            return Instance.soundManager;
        }
    }

    Fade fade;
    /// <summary>
    /// フェード中の時、true
    /// </summary>
    public static bool isFading {
        get
        {
            return Instance.fade.isFading;
        }
    }

    void Start () {
        fade = GetComponentInChildren<Fade>();
        scoreKeta = SCORE_MAX.ToString().Length;
        StartInit();
    }

    private void Update()
    {
        // フェードアウト時のBGMの連動を制御
        if (isBGMStop && fade.fadeState == Fade.FADE_STATE.OUT)
        {
            soundManager.SetBGMVolume(fade.fadeAlpha);
        }
        // パラメーターの表示
        scoreText.enabled = isScoreVisible;
        stageText.enabled = isStageVisible;
    }

    /// <summary>
    /// 指定の名前のシーンをフェードアウト後に読み込んで、
    /// フェードインさせます。
    /// フェードの秒数と色はデフォルトの設定のものです。
    /// StartCoroutine()で呼び出してください。
    /// フェード中かどうかをisFadingがtrueかで
    /// 確認できるので、キー操作の無効などは
    /// このフラグでチェックしてください。
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <param name="isbgmstop">BGMをフェードアウトさせたい時、true。デフォルトはfalse</param>
    public void LoadScene(string scene, bool isbgmstop=false)
    {
        StartCoroutine(loadScene(scene, defaultFadeTime, defaultFadeColor, isbgmstop));
    }

    /// <summary>
    /// 指定の名前のシーンをフェードアウト後に読み込んで、
    /// フェードインさせます。
    /// フェードの秒数と色は引数で指定します。
    /// StartCoroutine()で呼び出してください。
    /// フェード中かどうかをisFadingがtrueかで
    /// 確認できるので、キー操作の無効などは
    /// このフラグでチェックしてください。
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <param name="time">秒数</param>
    /// <param name="fadeColor">色</param>
    /// <param name="isbgmstop">BGMをフェードアウトさせたい時、true。デフォルトはfalse</param>
    public void LoadScene(string scene, float time, Color fadeColor, bool isbgmstop = false)
    {
        StartCoroutine(loadScene(scene, time, fadeColor, isbgmstop));
    }

    IEnumerator loadScene(string scene, float time, Color fadeColor, bool isbgmstop)
    {
        isBGMStop = isbgmstop;

        yield return fade.FadeOut(time, fadeColor);
        if (isBGMStop)
        {
            soundManager.StopBGM();
        }
        yield return SceneManager.LoadSceneAsync(scene);
        yield return fade.FadeIn(time, fadeColor);
    }

    /// <summary>
    /// Bgm Listに設定したnameを指定して、BGMを再生します。
    /// すでに同じ曲を再生中の時は何もしません。
    /// </summary>
    /// <param name="name">Bgm Listのname</param>
    public static void PlayBGM(string name )
    {
        SoundManagerInstance.PlayBGM(name);
    }

    /// <summary>
    /// Se Listに設定したnameを指定して、SEを鳴らします。
    /// </summary>
    /// <param name="name">Se Listのname</param>
    public static void PlaySE(string name)
    {
        if (Instance.soundManager == null)
        {
            Instance.soundManager = Instance.GetComponent<SoundManager>();
        }
        SoundManagerInstance.PlaySE(name);
    }

    #endregion
}
