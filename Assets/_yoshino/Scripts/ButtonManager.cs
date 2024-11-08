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
            // 自身の破壊
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // プレイヤーの状態設定
        GameObject.Find("Player").GetComponent<PlayerComponent>().SetPlayerState(GetComponentInParent<HurdleComponent>().GetPlayerState());
    }
}
