using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField, Header("スコアを表示するテキスト")]
    private Text[] txt_ranks;

    // ランキングの格納
    private int[] rankTimes = new int[6];

    // 今回の時間
    [SerializeField]
    private int newTime;

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    private void Start()
    {
        GetRanking();
        RankingCheck();
        RenderRanking();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 1; i < rankTimes.Length - 1; i++)
            {
                PlayerPrefs.SetInt("R" + i, 0);
            }
            Debug.Log("データ領域を初期化しました");
        }
    }

    /// <summary>
    /// ランキングがあれば値を取得してくる
    /// </summary>
    private void GetRanking()
    {
        //アプリのデータ領域が存在するか
        if (PlayerPrefs.HasKey("R1"))
        {
            for (int i = 1; i < rankTimes.Length - 1; i++)
            {
                // データ領域の読み込み
                rankTimes[i] = PlayerPrefs.GetInt("R" + i);
            }
            Debug.Log("データ領域を読み込みました");
        }
        else
        {
            for (int i = 1; i < rankTimes.Length - 1; i++)
            {
                rankTimes[i] = rankTimes[i] = 0;
                PlayerPrefs.SetInt("R" + i, 0);
            }
            Debug.Log("データ領域を初期化しました");
        }
    }

    /// <summary>
    /// ランキングの更新があるかチェック
    /// </summary>
    private void RankingCheck()
    {
        int newRank = 0; // まず今回のスコアを最下位と仮定する
        bool isDuplicate = false; // 同じスコアがあるかチェック用

        for (int idx = 0; idx < rankTimes.Length - 1; idx++)
        { // 昇順 1...3
            if (rankTimes[idx] == newTime)
            {
                isDuplicate = true; // 同じスコアがあった
                break;
            }
            if (rankTimes[idx] < newTime)
            {
                newRank = idx; // 新しいランクとして判定する
                break; // より良いランクが見つかったら終了
            }
        }

        if (newRank != 0)
        { //0位のままでなかったらランクイン確定
            for (int idx = rankTimes.Length - 1; idx > newRank; idx--)
            {
                rankTimes[idx] = rankTimes[idx - 1]; //繰り下げ処理
            }
            rankTimes[newRank] = newTime; //新ランクに登録
            for (int idx = 1; idx <= rankTimes.Length - 1; idx++)
            {
                PlayerPrefs.SetInt("R" + idx, rankTimes[idx]); //データ領域に保存
            }
        }
    }

    /// <summary>
    /// ランキングを表示する
    /// </summary>
    private void RenderRanking()
    {
        for (int idx = 0; idx < txt_ranks.Length; idx++)
        {
            int totalTime = rankTimes[idx];
            if (totalTime > 0)
            {
                totalTime = (int)(Timer.timeMax - totalTime);
                txt_ranks[idx].text = 
                Timer.GetHours(totalTime).ToString("00時") +
                Timer.GetMinutes(totalTime).ToString("00分") +
                Timer.GetSeconds(totalTime).ToString("00秒");
            }
            else
            {
                // スコアがないよ
                txt_ranks[idx].text = "--時--分--秒";
            }
        }
    }
}
