using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要

public class PlayerTitleAnimation : MonoBehaviour
{
    public Vector2 positionTarget;
    private bool isMove = false;

    private InputControl IC; // インプットアクションを定義

    [SerializeField]private float speedMove;  //速さ

    // Start is called before the first frame update
    void Start()
    {
        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Decide.started += OnDecide; // アクションにイベントを登録
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            // 現在地を取得
            Vector2 currentPosition = transform.position;

            if (Vector2.Distance(currentPosition, positionTarget) < 0.1f)
            {
                // 自身の破棄
                Destroy(gameObject);
                return;
            }

            // 移動
            transform.position = Vector3.MoveTowards(currentPosition, positionTarget, speedMove * Time.deltaTime);
        }
    }

    /// <summary>
    /// 移動開始
    /// </summary>
    /// <param name="context">ボタン入力</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        isMove = true;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance().StartChangingScene();
    }
}
