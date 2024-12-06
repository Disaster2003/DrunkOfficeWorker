using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要
using UnityEngine.EventSystems;

public class DecideScene : MonoBehaviour
{
    [Header("遷移先の状態・難易度")]
    [SerializeField] private GameManager.STATE_SCENE state_scene;
    [SerializeField] private GameManager.STATE_LEVEL state_level;

    private InputControl IC; // インプットアクションを定義

    // Start is called before the first frame update
    void Start()
    {
        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Decide.started += OnDecide; // アクションにイベントを登録
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
    }

    /// <summary>
    /// 決定ボタンの押下処理
    /// </summary>
    /// <param name="context">決定ボタン</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;

        // 遷移先のシーンを決定する
        GameManager.GetInstance().SetNextScene(state_scene, state_level);
    }

    private void OnDestroy()
    {
        // インプットアクションの停止
        IC.Disable();
    }
}
