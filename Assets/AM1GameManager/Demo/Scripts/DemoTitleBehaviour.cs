using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTitleBehaviour : MonoBehaviour {

    private void Start()
    {
        GameManager.PlayBGM("title");
        GameManager.Instance.isStageVisible = false;
    }

    void Update () {
        // フェードしていない状態で、マウスクリックされたら、ステージ１へ
        if (!GameManager.isFading && Input.GetMouseButtonDown(0))
        {
            GameManager.PlaySE("click");
            GameManager.StartInit();
            GameManager.Instance.LoadScene("DemoStage1", true);
        }
	}
}
