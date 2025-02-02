using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField, Header("ボタン画像")]
    private Sprite[] spriteArrayXboxButton;
    [SerializeField, Header("矢印画像")]
    private Sprite[] spriteArrayArrow;

    private Image imgButton;
    [SerializeField, Header("長押しゲージのベース")]
    private GameObject imgInputGaugeBase;
    [SerializeField, Header("長押しゲージ")]
    private Image imgInputGauge;
    [SerializeField, Header("連打ゲージ")]
    private Image imgRepeatGauge;
    [SerializeField] private Sprite[] spriteArrayBeer;

    private InputControl IC; // インプットアクションを定義
    [SerializeField, Header("前のボタン")]
    private GameObject goNotesBefore;

    private enum KIND_BUTTON
    {
        NONE,               // 未選択
        PUSH,               // 押下
        PUSH_LONG,          // 長押し
        PUSH_REPEAT_THREE,  // 3連打
        PUSH_REPEAT_FIVE,   // 5連打
    }
    [SerializeField, Header("チュートリアルのみ選択")]
    private KIND_BUTTON kind_button;
    private int iIndexMaxKind_Button = (int)KIND_BUTTON.PUSH_REPEAT_FIVE;

    private float fTimerPushLong;
    private bool isPushed; // true = 押下中, false = not 押下
    private int iCountPush;

    [SerializeField] private TextAsset level_adjust;
    private static Dictionary<KIND_BUTTON, int> numberGenerate = new Dictionary<KIND_BUTTON, int>();

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        imgButton = GetComponent<Image>();

        DecideButton();

        // 押下状態の初期化
        fTimerPushLong = 0;
        isPushed = false;
        iCountPush = 0;

        DecideKindOfButton();

        // 不必要な画像を非表示
        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                imgInputGaugeBase.SetActive(false);
                imgRepeatGauge.gameObject.SetActive(false);
                break;
            case KIND_BUTTON.PUSH_LONG:
                imgRepeatGauge.gameObject.SetActive(false);
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
                imgInputGaugeBase.SetActive(false);

                // Sprite配列をListに変換
                List<Sprite> spriteList = new List<Sprite>(spriteArrayBeer);

                // 2番目(インデックス1)と4番目(インデックス3)を削除
                spriteList.RemoveAt(1);
                spriteList.RemoveAt(3);

                // Listを再びSprite配列に変換
                spriteArrayBeer = spriteList.ToArray();
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                imgInputGaugeBase.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < 4)
        {
            // 先頭3つを表示
            imgButton.color = Color.white;
        }
        //else if(transform.position.x < 6)
        //{
        //    // 4つめを半透明
        //    imgButton.color = new Color(1, 1, 1, 0.5f);
        //}
        else
        {
            // 以降非表示
            imgButton.color = Color.black;
        }

        if (!isPushed) return;

        if (fTimerPushLong >= 0.5f)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            // ボタンを押下中は経過時間を加算
            fTimerPushLong += Time.deltaTime;
            imgInputGauge.fillAmount = fTimerPushLong / 0.5f;
        }
    }

    private void OnDestroy()
    {
        // インプットアクションの無効化
        IC.Disable();
    }

    /// <summary>
    /// キーの決定
    /// </summary>
    private void DecideButton()
    {
        // ボタンの抽選
        int rand = Random.Range(0, spriteArrayXboxButton.Length);

        // 画像の初期化
        if (Gamepad.all.Count > 0)
        {
            imgButton.sprite = spriteArrayXboxButton[rand];
        }
        else
        {
            imgButton.sprite = spriteArrayArrow[rand];
        }
        imgInputGauge.fillAmount = 0;
        imgRepeatGauge.sprite = spriteArrayBeer[0];

        // インプットアクションを取得
        IC = new InputControl();
        // アクションにイベントを登録
        switch (rand)
        {
            case 0:
                InitializedButton(IC.Player.UpKey);
                break;
            case 1:
                InitializedButton(IC.Player.DownKey);
                break;
            case 2:
                InitializedButton(IC.Player.LeftKey);
                break;
            case 3:
                InitializedButton(IC.Player.RightKey);
                break;
        }
        // インプットアクションの有効化
        IC.Enable();
    }

    /// <summary>
    /// キーの初期化
    /// </summary>
    /// <param name="inputAction">機能させるキー</param>
    private void InitializedButton(InputAction inputAction)
    {
        SetInputAndRelease(inputAction, IC.Player.UpKey);
        SetInputAndRelease(inputAction, IC.Player.DownKey);
        SetInputAndRelease(inputAction, IC.Player.LeftKey);
        SetInputAndRelease(inputAction, IC.Player.RightKey);
    }

    /// <summary>
    /// 押下と離した時のイベントを設定する
    /// </summary>
    /// <param name="inputAction">入力されたキー</param>
    /// <param name="keyKind">キーの種類</param>
    private void SetInputAndRelease(InputAction inputAction, InputAction keyKind)
    {
        keyKind.started += (inputAction == keyKind) ? InputButton : MissButton;
        if (inputAction == keyKind) keyKind.canceled += ReleaseButton;
    }


    /// <summary>
    /// ボタンの種類を決定
    /// </summary>
    private void DecideKindOfButton()
    {
        if (kind_button != KIND_BUTTON.NONE) return;

        if (numberGenerate.Count == 0)
        {
            // csvの読み込み
            StringReader reader = new StringReader(level_adjust.text); // TextAssetをStringReaderに変換
            List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine(); // 1行ずつ読み込む
                csvData.Add(line.Split(',')); // csvDataリストに追加する
            }

            // データ代入
            int iIndexLevel = (int)GameManager.GetInstance.GetLevelState;
            numberGenerate[KIND_BUTTON.PUSH] = int.Parse(csvData[iIndexLevel][2]);
            numberGenerate[KIND_BUTTON.PUSH_LONG] = int.Parse(csvData[iIndexLevel][3]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_THREE] = int.Parse(csvData[iIndexLevel][4]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_FIVE] = int.Parse(csvData[iIndexLevel][5]);
        }

        // ボタンアクションの選択
        int rand = 0;
        do
        {
            rand = Random.Range(1, iIndexMaxKind_Button + 1);
            if (numberGenerate[(KIND_BUTTON)rand] > 0)
            {
                numberGenerate[(KIND_BUTTON)rand]--;
                break;
            }
        }
        while (true);
        kind_button = (KIND_BUTTON)rand;
    }

    /// <summary>
    /// 押下処理
    /// </summary>
    private void InputButton(InputAction.CallbackContext context)
    {
        if (goNotesBefore != null) return;

        // nullチェック
        if (kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("ボタンアクションが未選択です");
            return;
        }

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                Destroy(transform.parent.gameObject);
                break;
            case KIND_BUTTON.PUSH_LONG:
                isPushed = true;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                iCountPush++;
                break;
        }
    }

    /// <summary>
    /// 解放処理
    /// </summary>
    private void ReleaseButton(InputAction.CallbackContext context)
    {
        if (goNotesBefore != null) return;

        // nullチェック
        if (kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("ボタンアクションが未選択です");
            return;
        }

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH_LONG:
                // ボタンを押していないと経過時間はゼロ
                imgInputGauge.fillAmount = 0;
                fTimerPushLong = 0;
                isPushed = false;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
                CountPushNumber(3);
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                CountPushNumber(5);
                break;
        }
    }

    /// <summary>
    /// 押下回数を数える
    /// </summary>
    /// <param name="iCountMax">押下上限</param>
    private void CountPushNumber(int iCountMax)
    {
        if (iCountPush >= iCountMax) Destroy(transform.parent.gameObject);
        else imgRepeatGauge.sprite = spriteArrayBeer[iCountPush];
    }

    /// <summary>
    /// キー入力の失敗処理
    /// </summary>
    private void MissButton(InputAction.CallbackContext context)
    {
        SpeedAdjust.AddMissCnt();
    }

    /// <summary>
    /// ボタンの生成数の合計を空にする
    /// </summary>
    public static void ClearNumberGenerate() { numberGenerate.Clear(); }
}
