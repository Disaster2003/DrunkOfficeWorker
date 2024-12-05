using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
                buttons[0].SetActive(true);
                break;
            case GameManager.STATE_LEVEL.NORMAL:
                for (int i = 0; i < 3; i++)
                {
                    buttons[i].SetActive(true);
                }
                break;
            case GameManager.STATE_LEVEL.HARD:
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
            int cnt = 0;
            foreach (GameObject btn in buttons)
            {
                if (btn == null) cnt++;
                else break;       
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(3 - cnt, transform.localPosition.y), Time.deltaTime);
        }
    }
}