using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要

public class DecideScene : MonoBehaviour
{
    [SerializeField, Header("フェードイン/アウト用画像")]
    private Image imgFade;

    [Header("遷移先の状態・難易度")]
    [SerializeField] private GameManager.STATE_SCENE state_scene;
    [SerializeField] private GameManager.STATE_LEVEL state_level;

    [Header("上下左右入力した時の切り替わり先ボタン")]
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    public bool isSelecting; // true = 選ばれている、false = 選ばれていない

    private InputControl IC; // インプットアクションを定義
    private Vector2 directionMove;
    public float intervalButtonChange;

    [SerializeField, Header("ボタン選択時のハイライト")]
    private GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        // 重力OFF
        GetComponent<Rigidbody2D>().gravityScale = 0;

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
        IC.Player.Direction.performed += OnReserveDirection; // 全てのアクションにイベントを登録
        IC.Player.Decide.started += OnDecide;
        IC.Enable(); // インプットアクションを機能させる為に有効化する。


        directionMove = Vector2.zero; // 移動方向の初期化
        intervalButtonChange = 0;     // ボタン切り替え間隔の初期化
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

    /// <summary>
    /// 入力方向を受け取る
    /// </summary>
    private void OnReserveDirection(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;

        // nullチェック
        directionMove = context.ReadValue<Vector2>();
        if (directionMove == Vector2.zero) return;

        if (intervalButtonChange <= 0)
        {
            if (directionMove.x > 0)
            {
                // 右のボタンに切り替え
                SwitchSelectButton(rightButton);
                return;
            }
            if (directionMove.x < 0)
            {
                // 左のボタンに切り替え
                SwitchSelectButton(leftButton);
                return;
            }
            if (directionMove.y > 0)
            {
                // 上のボタンに切り替え
                SwitchSelectButton(upButton);
                return;
            }
            if (directionMove.y < 0)
            {
                // 下のボタンに切り替え
                SwitchSelectButton(downButton);
                return;
            }
        }
    }

    /// <summary>
    /// ボタン選択を切り替える
    /// </summary>
    /// <param name="kindButton">ボタンの種類</param>
    private void SwitchSelectButton(GameObject kindButton)
    {
        if (kindButton == null) return;

        // 選択の切り替え
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
