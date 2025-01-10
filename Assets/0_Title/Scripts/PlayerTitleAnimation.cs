using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要

public class PlayerTitleAnimation : MonoBehaviour
{
    [SerializeField, Header("目標地点のx座標")]
    private float position_xGoal;
    private bool isMove; // true = 動いて良き, false = 動いたら悪し

    [Header("待機、驚愕、走行")]
    [SerializeField] private Sprite[] wait;
    [SerializeField] private Sprite[] amazing;
    [SerializeField] private Sprite[] run;

    private InputControl IC; // インプットアクションを定義

    [SerializeField, Header("移動速度")]
    private float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;

        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Decide.started += OnDecide; // アクションにイベントを登録
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove) return;

        if (transform.position.x > position_xGoal)
        {
            // シーン遷移の開始
            GameManager.GetInstance.StartChangingScene();

            // 自身の破棄
            Destroy(gameObject);
            return;
        }

        // 右移動
        transform.Translate(speedMove * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// 決定ボタンの押下処理
    /// </summary>
    /// <param name="context">決定ボタン</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (isMove) return;

        // 移動開始
        isMove = true;
    }
}
