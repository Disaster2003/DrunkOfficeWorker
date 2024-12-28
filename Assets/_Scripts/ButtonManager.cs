using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        switch(GameManager.GetInstance().GetLevelState())
        {
            case GameManager.STATE_LEVEL.NONE: 
            case GameManager.STATE_LEVEL.EASY:
                // 1個のボタンをアクティブ化
                buttons[0].SetActive(true);
                break;
            case GameManager.STATE_LEVEL.NORMAL:
                // 3個のボタンをアクティブ化
                for (int i = 0; i < 3; i++)
                {
                    buttons[i].SetActive(true);
                }
                break;
            case GameManager.STATE_LEVEL.HARD:
                // 全てのボタンをアクティブ化
                foreach (GameObject btn in buttons)
                {
                    btn.SetActive(true);
                }
                break;
        }

        foreach (GameObject btn in buttons)
        {
            if (!btn.activeSelf)
            {
                // 非アクティブのボタンを破棄
                Destroy(btn);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            // プレイヤーの状態設定
            FindObjectOfType<PlayerComponent>().SetPlayerState(GetComponentInParent<HurdleComponent>().GetPlayerState());

            // 自身の破壊
            Destroy(gameObject);
        }
        else
        {
            // 入力した数だけ、左へ移動させる
            int cnt = 0;
            foreach (GameObject btn in buttons)
            {
                if (btn == null) cnt++;
                else break;       
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(3 - cnt, transform.localPosition.y), GetComponent<SpeedUP>().firstSpeed * Time.deltaTime);
        }
    }
}