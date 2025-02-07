using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ランキングコンポーネント
/// </summary>
public class Ranking : MonoBehaviour
{
    [Header("名前、スコアのテキスト")]
    [SerializeField] private Text[] txtArrayName;
    [SerializeField] private Text[] txtArrayScore;

    private string[] strArrayNameRank = new string[6];
    private int[] iArrayScoreRank = new int[6];

    private string strNameLevel;

    // Start is called before the first frame update
    private void Start()
    {
        // 難易度の文字列を取得
        strNameLevel = GameManager.GetInstance.GetLevelState.ToString();

        GetRanking();
        RankingCheck();
        RenderRanking();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            // データ領域の初期化
            for (int i = 1; i < iArrayScoreRank.Length; i++)
            {
                PlayerPrefs.SetString(strNameLevel + "_RN" + i, "-");
                PlayerPrefs.SetInt(strNameLevel + "_RS" + i, 0);
            }
        }
    }

    /// <summary>
    /// データ領域を初期化、読み込みする
    /// </summary>
    private void GetRanking()
    {
        if (PlayerPrefs.HasKey(strNameLevel + "_RS1"))
        {
            // データ領域の読み込み
            for (int i = 1; i < iArrayScoreRank.Length; i++)
            {
                strArrayNameRank[i] = PlayerPrefs.GetString(strNameLevel + "_RN" + i);
                iArrayScoreRank[i] = PlayerPrefs.GetInt(strNameLevel + "_RS" + i);
            }
        }
        else
        {
            // データ領域の初期化
            for (int i = 1; i < iArrayScoreRank.Length; i++)
            {
                strArrayNameRank[i] = "-";
                iArrayScoreRank[i] = 0;
                PlayerPrefs.SetString(strNameLevel + "_RN" + i, strArrayNameRank[i]);
                PlayerPrefs.SetInt(strNameLevel + "_RS" + i, iArrayScoreRank[i]);
            }
        }
    }

    /// <summary>
    /// ランキングを更新する
    /// </summary>
    private void RankingCheck()
    {
        // 新しいハンドル名・タイムの取得
        string strNameNew = PlayerPrefs.GetString("PlayerName");
        int iTimeNew = (int)Timer.GetTimer;

        int iRankNew = 0; // まず今回のスコアを最下位と仮定する

        for (int idx = 1; idx < iArrayScoreRank.Length; idx++)
        {
            // 昇順 1...5
            if (iArrayScoreRank[idx] < iTimeNew)
            {
                // ランク番号の記録
                iRankNew = idx;
                break;
            }
        }

        // 同じスコアがなく、または新しいランクが見つかったら
        if (iRankNew != 0)
        {
            // 0位のままでなかったらランクイン確定
            for (int idx = iArrayScoreRank.Length - 1; idx > iRankNew; idx--)
            {
                //繰り下げ処理
                strArrayNameRank[idx] = strArrayNameRank[idx - 1];
                iArrayScoreRank[idx] = iArrayScoreRank[idx - 1];
            }

            // 新ランクに登録
            strArrayNameRank[iRankNew] = strNameNew;
            iArrayScoreRank[iRankNew] = iTimeNew;

            for (int idx = 1; idx < iArrayScoreRank.Length; idx++)
            {
                // データ領域に保存
                PlayerPrefs.SetString(strNameLevel + "_RN" + idx, strArrayNameRank[idx]);
                PlayerPrefs.SetInt(strNameLevel + "_RS" + idx, iArrayScoreRank[idx]);
            }
        }
    }

    /// <summary>
    /// ランキングを表示する
    /// </summary>
    private void RenderRanking()
    {
        for (int idx = 0; idx < txtArrayScore.Length; idx++)
        {
            int iTimeTotal = iArrayScoreRank[idx + 1];
            if (iTimeTotal > 0)
            {
                // 時間に置き換え
                iTimeTotal = Timer.timeMax - iTimeTotal;
                txtArrayScore[idx].text =
                Timer.GetHours(iTimeTotal).ToString("00時") +
                Timer.GetMinutes(iTimeTotal).ToString("00分") +
                Timer.GetSeconds(iTimeTotal).ToString("00秒");
            }
            else
            {
                // スコアなし
                txtArrayScore[idx].text = "--時--分--秒";
            }

            // 名前の代入
            txtArrayName[idx].text = strArrayNameRank[idx + 1];
        }
    }
}
