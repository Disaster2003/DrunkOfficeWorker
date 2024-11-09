using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
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
    }
}