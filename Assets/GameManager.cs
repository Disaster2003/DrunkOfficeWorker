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
        TITLE,    // タイトル画面
        TUTORIAL, // チュートリアル画面
        EASY,     // 初級
        NORMAL,   // 中級
        HARD,     // 上級
        RANKING,  // ランキング
        OVER,     // ゲームオーバー
        NONE,     // 何もなし
    }
    private STATE_SCENE state_scene;

    [SerializeField] Image imgFade;
    private bool isChangingScene;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("GameManagerのStartが複数回読み込まれています");
            return;
        }

        // タイトルスタート
        state_scene = STATE_SCENE.NONE;

        imgFade.fillAmount = 1; // フェードイン
        isChangingScene = false;
    }

    // Update is called once per frame
    void Update()
    {
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
    public void SetNextScene(STATE_SCENE _state_scene = STATE_SCENE.NONE)
    {
        // nullチェック
        if(STATE_SCENE.NONE == _state_scene )
        {
            return;
        }

        state_scene = _state_scene;
        isChangingScene = true;
    }
}
