﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイマークラス
/// </summary>
public class Timer : MonoBehaviour
{
    private Text txtTimer;

    [SerializeField, Header("背景を暗くするコンポーネント")]
    private BackGroundDarkken bgd;

    [SerializeField, Header("制限時間")]
    private float time = 30;

    // タイマー
    private static float timer = 0;

    // タイマーのゲッターランキングで呼び出す
    public static float get_timer
    {
        get
        {
            return timer;
        }
    }

    // 最大時間
    public static readonly double timeMax = 86400;

    // タイマーを止める
    private bool isStop = false;

    /// <summary>
    /// 時間に変換
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <returns></returns>
    public static int GetHours(int totalSeconds)
    {
        if (totalSeconds > 0)
        {
            return totalSeconds / 3600;
        }
        return 0;
    }

    /// <summary>
    /// 分に変換
    /// </summary>
    /// <returns></returns>
    public static int GetMinutes(int totalSeconds)
    {
        if (totalSeconds > 0)
        {
            return (totalSeconds % 3600) / 60;
        }
        return 0;
    }

    /// <summary>
    /// 秒に変換
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <returns></returns>
    public static int GetSeconds(int totalSeconds)
    {
        if (totalSeconds > 0)
        {
            return totalSeconds % 60;
        }
        return 0;
    }

    /// <summary>
    /// タイマーを動かすのを止める
    /// </summary>
    public void Stop()
    {
        isStop = true;
    }

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
        if (!isStop)
        {
            // 制限時間が無くなるまで時間を減らす
            if (timer <= 0)
            {
                timer = 0;
            }
            else
            {
                // 時間を減らす
                timer -= Time.deltaTime;
            }
        }

        if (bgd)
        {
            // 背景を時間に連動させて暗くしていく
            bgd.Darkken(Mathf.Clamp01(1 - (time - (time - timer)) / time));
        }

        Render();
    }

    /// <summary>
    /// UIの表示
    /// </summary>
    private void Render()
    {
        // 表示のためにtimeMaxからタイマーを減算
        int totalTime = (int)(timeMax - timer);
        txtTimer.text =
            GetHours(totalTime).ToString("00時") +
            GetMinutes(totalTime).ToString("00分") +
            GetSeconds(totalTime).ToString("00秒");
    }
}
