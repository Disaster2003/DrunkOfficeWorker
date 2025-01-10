using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイマークラス
/// </summary>
public class Timer : MonoBehaviour
{
    private Text txtTimer;

    [SerializeField, Header("制限時間")]
    private float time = 30.0f;

    private static float timer = 0;

    /// <summary>
    /// タイマーを取得する
    /// </summary>
    public static float GetTimer { get { return timer; } }

    public static readonly int timeMax = 86400; // 最大時間

    [SerializeField] private GameObject spawner;

    [SerializeField] private BackGroundDarkken backGroundDarkken;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
        txtTimer = GetComponent<Text>();

        // 値を初期化
        timer = time;
    }

    /// <summary>
    /// 更新イベント
    /// </summary>
    private void Update()
    {
        if (spawner == null) return;

        if (timer <= 0)
        {
            // ゲームオーバー
            timer = 0;
            GameManager.GetInstance.SetNextScene(GameManager.STATE_SCENE.OVER);
        }
        else
        {
            // 時間経過
            timer -= Time.deltaTime;
        }

        ReplaceTime();

        // nullチェック
        if (backGroundDarkken == null)
        {
            Debug.LogError("BackgroundDarkkenが未設定です");
            return;
        }

        backGroundDarkken.Darkken(Mathf.Clamp01(1 - (time - (time - timer)) / time));
    }

    /// <summary>
    /// 時間に置き換える
    /// </summary>
    private void ReplaceTime()
    {
        int totalTime = (int)(timeMax - timer);
        txtTimer.text =
            GetHours(totalTime).ToString("00時") +
            GetMinutes(totalTime).ToString("00分") +
            GetSeconds(totalTime).ToString("00秒");
    }

    /// <summary>
    /// 時間に変換する
    /// </summary>
    /// <param name="iTotalSeconds">秒数の合計</param>
    /// <returns>時間数</returns>
    public static int GetHours(int iTotalSeconds)
    {
        if (iTotalSeconds > 0) return iTotalSeconds / 3600;

        return 0;
    }

    /// <summary>
    /// 分に変換する
    /// </summary>
    /// <param name="iTotalSeconds">秒数の合計</param>
    /// <returns>分数</returns>
    public static int GetMinutes(int iTotalSeconds)
    {
        if (iTotalSeconds > 0) return (iTotalSeconds % 3600) / 60;
        
        return 0;
    }

    /// <summary>
    /// 秒に変換する
    /// </summary>
    /// <param name="iTotalSeconds">秒数の合計</param>
    /// <returns>秒数</returns>
    public static int GetSeconds(int iTotalSeconds)
    {
        if (iTotalSeconds > 0) return iTotalSeconds % 60;

        return 0;
    }
}
