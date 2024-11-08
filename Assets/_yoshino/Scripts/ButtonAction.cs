using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private Sprite[] UI_Button;
    [SerializeField] private Image imgInputGauge;

    private InputControl IC; // インプットアクションを定義

    [SerializeField] private GameObject imgBeforeButton;

    private enum KIND_BUTTON
    {
        NONE,       // 未選択
        PUSH,       // 押下
        PUSH_LONG,  // 長押し
        PUSH_REPEAT,// 連打
    }
    [SerializeField, Header("チュートリアル用ボタンアクションの選択")]
    private KIND_BUTTON kind_button;
    private float timerPushLong;
    private bool isPushed; // true = 押下中, false = not 押下
    private int countPush;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 4);

        // 画像の初期化
        GetComponent<Image>().sprite = UI_Button[rand];
        imgInputGauge.fillAmount = 0;

        // インプットアクションを取得
        IC = new InputControl();
        // アクションにイベントを登録
        switch (rand)
        {
            case 0:
                IC.Player.UpKey.started += InputButton;
                IC.Player.UpKey.canceled += ReleaseButton;
                break;
            case 1:
                IC.Player.DownKey.started += InputButton;
                IC.Player.DownKey.canceled += ReleaseButton;
                break;
            case 2:
                IC.Player.LeftKey.started += InputButton;
                IC.Player.LeftKey.canceled += ReleaseButton;
                break;
            case 3:
                IC.Player.RightKey.started += InputButton;
                IC.Player.RightKey.canceled += ReleaseButton;
                break;
        }
        // インプットアクションの有効化
        IC.Enable();

        if (kind_button == KIND_BUTTON.NONE)
        {
            // ボタンアクションの選択
            rand = Random.Range(1, 4);
            kind_button = (KIND_BUTTON)rand;
        }

        // 押下状態の初期化
        timerPushLong = 0;
        countPush = 0;
        isPushed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPushed)
        {
            if (timerPushLong > 0.5f)
            {
                // 自身の破壊
                Destroy(gameObject);
            }

            // ボタンを押下中は経過時間を加算
            timerPushLong += Time.deltaTime;
            imgInputGauge.fillAmount = timerPushLong / 0.5f;
        }

        if (imgBeforeButton != null) return;

        if (transform.position.x > 5)
        {
            // ボタンをずらす
            transform.Translate(-Time.deltaTime, 0, 0);
            return;
        }
    }

    private void OnDestroy()
    {
        // インプットアクションの無効化
        IC.Disable();
        Destroy(imgInputGauge.gameObject);
    }

    /// <summary>
    /// 押下処理
    /// </summary>
    private void InputButton(InputAction.CallbackContext context)
    {
        // nullチェック
        if(kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("ボタンアクションが未選択です");
            return;
        }

        if (imgBeforeButton != null) return;

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                // 自身の破壊
                Destroy(gameObject);
                break;
            case KIND_BUTTON.PUSH_LONG:
                isPushed = true;
                break;
            case KIND_BUTTON.PUSH_REPEAT:
                countPush++;
                break;
        }
    }

    /// <summary>
    /// 解放処理
    /// </summary>
    private void ReleaseButton(InputAction.CallbackContext context)
    {
        // ボタンを押していないと経過時間はゼロ
        imgInputGauge.fillAmount = 0;
        timerPushLong = 0;
        isPushed = false;

        if (countPush > 5)
        {
            // 自身の破壊
            Destroy(gameObject);
        }
    }
}
