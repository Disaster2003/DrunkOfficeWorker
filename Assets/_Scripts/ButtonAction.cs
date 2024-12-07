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

    [SerializeField, Header("長押し・連打ゲージ")]
    private Image imgInputGauge;

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

    private float timerPushLong;
    private bool isPushed; // true = 押下中,false = not 押下
    private int countPush;

    private static Dictionary<KIND_BUTTON, int> numberGenerate = new Dictionary<KIND_BUTTON, int>();

    // Start is called before the first frame update
    void Start()
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

        // 押下状態の初期化
        timerPushLong = 0;
        countPush = 0;
        isPushed = false;

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
        do
        {
            rand = Random.Range(1, 4);
            if (numberGenerate[(KIND_BUTTON)rand] > 0)
            {
                numberGenerate[(KIND_BUTTON)rand]--;
                break;
            }
        }
        while (true);
        kind_button = (KIND_BUTTON)rand;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPushed) return;

        if (timerPushLong > 0.5f)
        {
            // 自身の破壊
            Destroy(gameObject);
        }

        // ボタンを押下中は経過時間を加算
        timerPushLong += Time.deltaTime;
        imgInputGauge.fillAmount = timerPushLong / 0.5f;
    }

    private void OnDestroy()
    {
        // インプットアクションの無効化
        IC.Disable();
        Destroy(transform.parent.gameObject);
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
                // 自身の破壊
                Destroy(gameObject);
                break;
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
                if(countPush >= 3)
                {
                    // 自身の破壊
                    Destroy(gameObject);
                }
                else
                {
                    imgInputGauge.fillAmount = countPush / 3.0f;
                }
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                if (countPush >= 5)
                {
                    // 自身の破壊
                    Destroy(gameObject);
                }
                else
                {
                    imgInputGauge.fillAmount = countPush / 5.0f;
                }
                break;
        }
    }
}
