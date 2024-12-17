using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public enum STATE_SCENE
    {
        TITLE = 0,    // タイトル画面
        TUTORIAL = 1, // チュートリアル画面
        NAMING = 2,   // 命名画面
        PLAY = 3,     // プレイ画面
        RANKING = 4,  // ランキング
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

    [SerializeField, Header("フェードイン/アウト用画像")]
    private Image imgFade;
    private bool isChangingScene; // true = シーン遷移する, false = シーン遷移しない

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
            Destroy(gameObject);
        }

        // 状態の初期化
        state_scene = (STATE_SCENE)SceneManager.GetActiveScene().buildIndex;
        state_level = STATE_LEVEL.NONE;

        // シーン遷移状態の初期化
        imgFade.fillAmount = 1;
        isChangingScene = false;
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

                Debug.Log("シーンを切り替えました");
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
    /// インスタンスを取得する
    /// </summary>
    public static GameManager GetInstance() { return instance; }

    /// <summary>
    /// 次のシーンを設定する
    /// </summary>
    /// <param name="_state_scene">次のシーン</param>
    public void SetNextScene(STATE_SCENE _state_scene = STATE_SCENE.NONE, STATE_LEVEL _state_level = STATE_LEVEL.NONE)
    {
        if(isChangingScene) return;

        switch (state_scene)
        {
            case STATE_SCENE.NAMING:
            case STATE_SCENE.RANKING:
                _state_level = state_level;
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

        // 遷移先の決定
        state_scene = _state_scene;
        state_level = _state_level;
    }

    /// <summary>
    /// 難易度を取得する
    /// </summary>
    public STATE_LEVEL GetLevelState() { return state_level; }

    /// <summary>
    /// シーンの切り替えを開始する
    /// </summary>
    public void StartChangingScene() { isChangingScene = true; }
}
