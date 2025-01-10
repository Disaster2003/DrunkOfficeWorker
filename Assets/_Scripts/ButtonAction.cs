using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField, Header("ボタン画像")]
    private Sprite[] UI_Button;
    [SerializeField, Header("矢印画像")]
    private Sprite[] UI_Arrow;

    [SerializeField, Header("長押しゲージのベース")]
    private GameObject imgInputGaugeBase;
    [SerializeField, Header("長押しゲージ")]
    private Image imgInputGauge;
    [SerializeField, Header("連打ゲージ")]
    private Image imgRepeatGauge;
    [SerializeField] private Sprite[] beer;

    private InputControl IC; // インプットアクションを定義
    [SerializeField, Header("前のボタン")]
    private GameObject imgBeforeButton;

    private enum KIND_BUTTON
    {
        NONE,               // 未選択
        PUSH,               // 押下
        PUSH_LONG,          // 長押し
        PUSH_REPEAT_THREE,  // 3連打
        PUSH_REPEAT_FIVE,   // 5連打
    }
    [SerializeField, Header("チュートリアル用ボタンアクションの選択")]
    private KIND_BUTTON kind_button;
    private int indexMaxKindButton = (int)KIND_BUTTON.PUSH_REPEAT_FIVE;

    private float timerPushLong;
    private bool isPushed; // true = 押下中, false = not 押下

    private int countPush;

    private static Dictionary<KIND_BUTTON, int> numberGenerate = new Dictionary<KIND_BUTTON, int>();

    // Start is called before the first frame update
    void Start()
    {
        DecideButton();

        // 押下状態の初期化
        timerPushLong = 0;
        isPushed = false;
        countPush = 0;

        DecideKindOfButton();

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
                List<Sprite> spriteList = new List<Sprite>(beer);

                // 2番目(インデックス1)と4番目(インデックス3)を削除
                spriteList.RemoveAt(1);
                spriteList.RemoveAt(3);

                // Listを再びSprite配列に変換 (必要であれば)
                beer = spriteList.ToArray();
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                imgInputGaugeBase.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPushed) return;

        if (timerPushLong > 0.5f)
        {
            DeleteComponent();
        }

        // ボタンを押下中は経過時間を加算
        timerPushLong += Time.deltaTime;
        imgInputGauge.fillAmount = timerPushLong / 0.5f;
    }

    /// <summary>
    /// キーの決定
    /// </summary>
    private void DecideButton()
    {
        // ボタンの抽選
        int rand = Random.Range(0, 4);

        // 画像の初期化
        if (Gamepad.all.Count > 0)
        {
            GetComponent<Image>().sprite = UI_Button[rand];
        }
        else
        {
            GetComponent<Image>().sprite = UI_Arrow[rand];
        }
        imgInputGauge.fillAmount = 0;
        imgRepeatGauge.sprite = beer[0];

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
        IC.Player.UpKey.started += (inputAction == IC.Player.UpKey) ? InputButton : MissButton;
        IC.Player.UpKey.canceled += (inputAction == IC.Player.UpKey) ? ReleaseButton : null;
        IC.Player.DownKey.started += (inputAction == IC.Player.DownKey) ? InputButton : MissButton;
        IC.Player.DownKey.canceled += (inputAction == IC.Player.DownKey) ? ReleaseButton : null;
        IC.Player.LeftKey.started += (inputAction == IC.Player.LeftKey) ? InputButton : MissButton;
        IC.Player.LeftKey.canceled += (inputAction == IC.Player.LeftKey) ? ReleaseButton : null;
        IC.Player.RightKey.started += (inputAction == IC.Player.RightKey) ? InputButton : MissButton;
        IC.Player.RightKey.canceled += (inputAction == IC.Player.RightKey) ? ReleaseButton : null;
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
            TextAsset csvFile = Resources.Load("level_adjust") as TextAsset; // ResourcesにあるCSVファイルを格納
            StringReader reader = new StringReader(csvFile.text); // TextAssetをStringReaderに変換
            List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine(); // 1行ずつ読み込む
                csvData.Add(line.Split(',')); // csvDataリストに追加する
            }

            // データ代入
            int levelIndex = (int)GameManager.GetInstance().GetLevelState();
            numberGenerate[KIND_BUTTON.PUSH] = int.Parse(csvData[levelIndex][2]);
            numberGenerate[KIND_BUTTON.PUSH_LONG] = int.Parse(csvData[levelIndex][3]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_THREE] = int.Parse(csvData[levelIndex][4]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_FIVE] = int.Parse(csvData[levelIndex][5]);
        }

        // ボタンアクションの選択
        int rand = 0;
        do
        {
            rand = Random.Range(1, indexMaxKindButton + 1);
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
        if (imgBeforeButton != null) return;

        // nullチェック
        if (kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("ボタンアクションが未選択です");
            return;
        }

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                DeleteComponent();
                return;
            case KIND_BUTTON.PUSH_LONG:
                isPushed = true;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                countPush++;
                break;
        }
    }

    /// <summary>
    /// 解放処理
    /// </summary>
    private void ReleaseButton(InputAction.CallbackContext context)
    {
        switch (kind_button)
        {
            case KIND_BUTTON.PUSH_LONG:
                // ボタンを押していないと経過時間はゼロ
                imgInputGauge.fillAmount = 0;
                timerPushLong = 0;
                isPushed = false;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
                if (countPush >= 3)
                {
                    DeleteComponent();
                    return;
                }
                else imgRepeatGauge.sprite = beer[countPush];
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                if (countPush >= 5)
                {
                    DeleteComponent();
                    return;
                }
                else imgRepeatGauge.sprite = beer[countPush];
                break;
        }
    }

    /// <summary>
    /// キー入力の失敗処理
    /// </summary>
    private void MissButton(InputAction.CallbackContext context)
    {
        SpeedUP.UpMissCnt();
    }

    private void DeleteComponent()
    {
        // インプットアクションの無効化
        IC.Disable();

        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// numberGenerateを空にする
    /// </summary>
    public static void ClearNumberGenerate() { numberGenerate.Clear(); }
}
