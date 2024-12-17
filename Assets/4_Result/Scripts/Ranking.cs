using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ランキングコンポーネント
/// </summary>
public class Ranking : MonoBehaviour
{
    [Header("名前、スコアのテキスト")]
    [SerializeField] private Text[] txtName;
    [SerializeField] private Text[] txtScore;

    // ランキングの格納
    private string[] nameRank = new string[6];
    private int[] scoreRank = new int[6];

    string nameLevel;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
        nameLevel = GameManager.GetInstance().GetLevelState().ToString();

        GetRanking();
        RankingCheck();
        RenderRanking();
    }

    /// <summary>
    /// 更新イベント
    /// </summary>
    private void Update()
    {
        // ランキングの初期化
        if ((Input.GetKey(KeyCode.Escape) && Input.GetKeyDown(KeyCode.Return)))
        {
            for (int i = 1; i < scoreRank.Length; i++)
            {
                PlayerPrefs.SetString(nameLevel + "_RN" + i, "-");
                PlayerPrefs.SetInt(nameLevel + "_RS" + i, 0);
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
        if (PlayerPrefs.HasKey(nameLevel + "_RS1"))
        {
            for (int i = 1; i < scoreRank.Length; i++)
            {
                // データ領域の読み込み
                nameRank[i] = PlayerPrefs.GetString(nameLevel + "_RN" + i);
                scoreRank[i] = PlayerPrefs.GetInt(nameLevel + "_RS" + i);
            }
            Debug.Log("データ領域を読み込みました");
        }
        else
        {
            for (int i = 1; i < scoreRank.Length; i++)
            {
                nameRank[i] = "-";
                scoreRank[i] = 0;
                PlayerPrefs.SetString(nameLevel + "_RN" + i, nameRank[i]);
                PlayerPrefs.SetInt(nameLevel + "_RS" + i, scoreRank[i]);
            }
            Debug.Log("データ領域を初期化しました");
        }
    }

    /// <summary>
    /// ランキングの更新があるかチェック
    /// </summary>
    private void RankingCheck()
    {
        // 新しいタイムを取得
        string newName = PlayerPrefs.GetString("PlayerName");
        int newTime = (int)Timer.get_timer;

        int newRank = 0; // まず今回のスコアを最下位と仮定する

        for (int idx = 1; idx < scoreRank.Length; idx++)
        { // 昇順 1...5
            if (scoreRank[idx] < newTime)
            {
                newRank = idx; // 新しいランクとして判定する
                break; // より良いランクが見つかったら終了
            }
        }

        // 同じスコアがなく、または新しいランクが見つかったら
        if (newRank != 0)
        {
            //0位のままでなかったらランクイン確定
            for (int idx = scoreRank.Length - 1; idx > newRank; idx--)
            {
                //繰り下げ処理
                nameRank[idx] = nameRank[idx - 1];
                scoreRank[idx] = scoreRank[idx - 1];
            }

            //新ランクに登録
            nameRank[newRank] = newName;
            scoreRank[newRank] = newTime;

            for (int idx = 1; idx < scoreRank.Length; idx++)
            {
                //データ領域に保存
                PlayerPrefs.SetString(nameLevel + "_RN" + idx, nameRank[idx]);
                PlayerPrefs.SetInt(nameLevel + "_RS" + idx, scoreRank[idx]);
            }
        }
    }

    /// <summary>
    /// ランキングを表示する
    /// </summary>
    private void RenderRanking()
    {
        for (int idx = 0; idx < txtScore.Length; idx++)
        {
            int totalTime = scoreRank[idx + 1];
            if (totalTime > 0)
            {
                totalTime = (int)(Timer.timeMax - totalTime);
                txtScore[idx].text =
                Timer.GetHours(totalTime).ToString("00時") +
                Timer.GetMinutes(totalTime).ToString("00分") +
                Timer.GetSeconds(totalTime).ToString("00秒");
            }
            else
            {
                // スコアがないよ
                txtName[idx].text = nameRank[idx + 1];
                txtScore[idx].text = "--時--分--秒";
            }
        }
    }
}
