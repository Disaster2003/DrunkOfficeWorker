using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要

public class DecideScene : MonoBehaviour
{
    [SerializeField] Image imgFade; // フェード用画像

    [SerializeField] GameManager.STATE_SCENE state_scene;
    [SerializeField] GameManager.STATE_LEVEL state_level;

    [SerializeField] GameObject upButton;   // 上入力した時の切り替わり先ボタン
    [SerializeField] GameObject downButton; // 下入力した時の切り替わり先ボタン
    [SerializeField] GameObject leftButton; // 左入力した時の切り替わり先ボタン
    [SerializeField] GameObject rightButton;// 右入力した時の切り替わり先ボタン
    public bool isSelecting; // true = 選ばれている、false = 選ばれていない

    private InputControl IC;           // インプットアクションを定義
    private Vector2 direction;         // 移動方向
    public float intervalButtonChange; // ボタン切り替えのインターバル

    [SerializeField] GameObject highlight; // 選択時のハイライト

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0; // 重力OFF

        if (state_scene == GameManager.STATE_SCENE.TITLE || state_scene == GameManager.STATE_SCENE.TUTORIAL)
        {
            // 選択状態
            isSelecting = true;
        }
        else
        {
            // 非選択状態
            isSelecting = false;
        }

        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Direction.performed += OnDirection; // 全てのアクションにイベントを登録
        IC.Player.Decide.started += OnDecide;
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
        direction = Vector2.zero; // 移動方向の初期化
        intervalButtonChange = 0; // インターバルの初期化
    }

    // Update is called once per frame
    void Update()
    {
        if(imgFade.fillAmount == 0)
        {
            // 重力ON
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        if (isSelecting)
        {
            if (Time.time % 1 <= 0.5f)
            {
                // ハイライトON
                highlight.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                // ハイライトOFF
                highlight.GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
        else
        {
            // 非選択時は、ハイライトOFF
            highlight.GetComponent<SpriteRenderer>().color = Color.clear;
        }

        if(intervalButtonChange > 0)
        {
            // ボタン切り替えのインターバル中
            intervalButtonChange += -Time.deltaTime;
        }
    }

    private void OnDirection(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;

        // nullチェック
        direction = context.ReadValue<Vector2>();
        if (direction == Vector2.zero) return;

        if (intervalButtonChange <= 0)
        {
            if (direction.x > 0)
            {
                // 右のボタンに切り替え
                ChangeSelectButton(rightButton);
                return;
            }
            if (direction.x < 0)
            {
                // 左のボタンに切り替え
                ChangeSelectButton(leftButton);
                return;
            }
            if (direction.y > 0)
            {
                // 上のボタンに切り替え
                ChangeSelectButton(upButton);
                return;
            }
            if (direction.y < 0)
            {
                // 下のボタンに切り替え
                ChangeSelectButton(downButton);
                return;
            }
        }
    }

    /// <summary>
    /// ボタン選択を切り替える
    /// </summary>
    /// <param name="kindButton">ボタンの種類</param>
    private void ChangeSelectButton(GameObject kindButton)
    {
        if (kindButton == null) return;
        isSelecting = false;
        kindButton.GetComponent<DecideScene>().isSelecting = true;
        kindButton.GetComponent<DecideScene>().intervalButtonChange = 0.1f;
    }

    /// <summary>
    /// 遷移先のシーンを決定する
    /// </summary>
    /// <param name="context">ボタン入力</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;
        GameManager.GetInstance().SetNextScene(state_scene, state_level);
    }
}
