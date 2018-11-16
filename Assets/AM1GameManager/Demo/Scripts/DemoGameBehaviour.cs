using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoGameBehaviour : MonoBehaviour {
    const float POINT_MAX = 100;
    const float TIME_POINT_RATE = 10;
    float gameTime;
    int ballCount;

	// Use this for initialization
	void Start () {
        gameTime = 0;
        ballCount = GameObject.FindGameObjectsWithTag("Ball").Length;
        GameManager.PlayBGM("game");
        GameManager.Instance.isStageVisible = true;
        GameManager.DrawParams();
    }

    // Update is called once per frame
    void Update () {
        // フェード中は何もしない
        if (GameManager.isFading) return;

        gameTime += Time.deltaTime;

        // マウスがボールを指しているか
        Vector3 mpos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mpos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask("Ball")))
        {
            // ボールゲット
            Destroy(hit.collider.gameObject);
            GameManager.PlaySE("point");
            getPoint();
            ballCount--;
            if (ballCount <= 0)
            {
                GameManager.PlaySE("clear");
                stageClear();
            }
        }
	}

    /// <summary>
    /// 点を加算します。
    /// </summary>
    /// <returns>得点</returns>
    void getPoint()
    {
        int bpoint = (int)(POINT_MAX-gameTime*TIME_POINT_RATE);
        if (bpoint < 0)
        {
            return;
        }
        GameManager.AddScore(bpoint);
    }

    void stageClear()
    {
        // クリア
        GameManager.AddStage();
        if (GameManager.Stage <2)
        {
            // 次のシーンへ。BGMは継続
            GameManager.Instance.LoadScene("DemoStage"+(GameManager.Stage+1), false);
        }
        else
        {
            // タイトルへ。BGMは停止
            GameManager.Instance.LoadScene("DemoTitle");
        }
    }
}
