using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject[] goArrayNotes;

    // Start is called before the first frame update
    void Start()
    {
        if(goArrayNotes == null)
        {
            Debug.Log("ノーツが未設定です");
            return;
        }

        switch(GameManager.GetInstance.GetLevelState)
        {
            case GameManager.STATE_LEVEL.NONE: 
            case GameManager.STATE_LEVEL.EASY:
                // 1個のボタンをアクティブ化
                goArrayNotes[0].SetActive(true);
                break;
            case GameManager.STATE_LEVEL.NORMAL:
                // 3個のボタンをアクティブ化
                for (int i = 0; i < 3; i++)
                {
                    goArrayNotes[i].SetActive(true);
                }
                break;
            case GameManager.STATE_LEVEL.HARD:
                // 全てのボタンをアクティブ化
                foreach (GameObject notes in goArrayNotes)
                {
                    notes.SetActive(true);
                }
                break;
        }

        foreach (GameObject notes in goArrayNotes)
        {
            // 非アクティブのボタンを破棄
            if (!notes.activeSelf) Destroy(notes);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            // プレイヤーの状態設定
            FindObjectOfType<PlayerComponent>().SetPlayerState = GetComponentInParent<HurdleComponent>().GetPlayerState;

            // 自身の破壊
            Destroy(gameObject);
        }
        else
        {
            // 入力した数だけ、左へ移動させる
            int cnt = 0;
            foreach (GameObject btn in goArrayNotes)
            {
                if (btn == null) cnt++;
                else break;       
            }

            transform.localPosition =
                Vector3.Lerp
                (
                    transform.localPosition,
                    new Vector3(3 - cnt, transform.localPosition.y),
                    GetComponent<SpeedAdjust>().speedCurrent * Time.deltaTime
                );
        }
    }
}