using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    /// <summary>
    /// インスタンスを取得する
    /// </summary>
    public static GameManager GetInstance { get { return instance; } }

    public enum STATE_SCENE
    {
        TITLE = 0,    // タイトル画面
        TUTORIAL = 1, // チュートリアル画面
        NAMING = 2,   // 命名画面
        PLAY = 3,     // プレイ画面
        RESULT = 4,   // 結果画面
        OVER = 5,     // ゲームオーバー
        NONE,         // 何もなし
    }
    private STATE_SCENE state_scene;
    public enum STATE_LEVEL
    {
        NONE,   // プレイ画面以外
        EASY,   // 初級
        NORMAL, // 中級
        HARD,   // 上級
    }
    private STATE_LEVEL state_level;
    /// <summary>
    /// 難易度を取得する
    /// </summary>
    public STATE_LEVEL GetLevelState {  get { return state_level; } }

    [SerializeField, Header("フェードイン/アウト用画像")]
    private Image imgFade;
    private bool isChangingScene; // true = シーン遷移する, false = シーン遷移しない

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // インスタンスの生成
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 単一化
            Destroy(gameObject);
        }

        // 状態の初期化
        state_scene = (STATE_SCENE)SceneManager.GetActiveScene().buildIndex;
        state_level = STATE_LEVEL.NONE;

        // シーン遷移状態の初期化
        imgFade.fillAmount = 1;
        isChangingScene = false;

        // 名前の初期化
        PlayerPrefs.SetString("PlayerName", "");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // nullチェック
        if (imgFade == null)
        {
            Debug.LogError("imgFadeをinspectorで設定していません");
            return;
        }

        if (isChangingScene)
        {
            if (imgFade.fillAmount >= 1)
            {
                // 次のシーンへ
                isChangingScene = false;
                SceneManager.LoadSceneAsync((int)state_scene);
            }
            else
            {
                // フェードアウト
                imgFade.fillAmount += Time.deltaTime;
            }
        }
        else if(imgFade.fillAmount > 0)
        {
            // フェードイン
            imgFade.fillAmount += -Time.deltaTime;
        }
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ゲームプレイ終了
#else
    Application.Quit(); // ゲームプレイ終了
#endif
    }

    /// <summary>
    /// 次のシーンを設定する
    /// </summary>
    /// <param name="_state_scene">次のシーン</param>
    public void SetNextScene(STATE_SCENE _state_scene = STATE_SCENE.NONE, STATE_LEVEL _state_level = STATE_LEVEL.NONE)
    {
        if(isChangingScene) return;

        // 遷移時の特別な処理
        switch (state_scene)
        {
            case STATE_SCENE.NAMING:
                _state_level = state_level;
                isChangingScene = true;
                break;
            case STATE_SCENE.PLAY:
                _state_level = state_level;
                break;
            case STATE_SCENE.RESULT:
                if(_state_scene == STATE_SCENE.TITLE)
                {
                    // 名前リセット
                    PlayerPrefs.SetString("PlayerName", "");
                }

                isChangingScene = true;
                break;
            case STATE_SCENE.OVER:
                isChangingScene = true;
                break;
        }

        // nullチェック
        if (_state_scene == STATE_SCENE.NONE)
        {
            Debug.LogError("遷移先のシーンが未設定です");
            return;
        }
        if (_state_scene == STATE_SCENE.PLAY && _state_level == STATE_LEVEL.NONE)
        {
            Debug.LogError("遷移先の難易度が未設定です");
            return;
        }

        if (_state_scene == STATE_SCENE.PLAY)
        {
            if (state_scene != STATE_SCENE.NAMING)
            {
                if (PlayerPrefs.GetString("PlayerName") == "")
                {
                    // 名前入力画面を挟む
                    _state_scene = STATE_SCENE.NAMING;
                }
            }
        }

        // 遷移先の決定
        state_scene = _state_scene;
        state_level = _state_level;
    }

    /// <summary>
    /// シーンの切り替えを開始する
    /// </summary>
    public void StartChangingScene() { isChangingScene = true; }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="se">鳴らすSE</param>
    public void PlaySE(AudioClip se)
    {
        audioSource.PlayOneShot(se);
    }
}
