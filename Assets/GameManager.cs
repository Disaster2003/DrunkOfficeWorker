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
        PLAY = 2,     // プレイ画面
        RANKING = 3,  // ランキング
        OVER = 4,     // ゲームオーバー
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
            Debug.LogError("GameManagerのStartが複数回読み込まれています");
            return;
        }

        // 状態の初期化
        state_scene = STATE_SCENE.NONE;
        state_level = STATE_LEVEL.NONE;

        imgFade.fillAmount = 1; // フェードイン
        isChangingScene = false;// シーン遷移状態の初期化
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
            // フェードアウト
            imgFade.fillAmount += Time.deltaTime;
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
        // nullチェック
        if (_state_scene == STATE_SCENE.NONE) return;
        if (_state_scene == STATE_SCENE.PLAY && _state_level == STATE_LEVEL.NONE) return;

        // 遷移先の決定
        state_scene = _state_scene;
        isChangingScene = true;
    }
}
