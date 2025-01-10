using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjust : MonoBehaviour
{
    [Header("初期速度を入力してください")]
    public float speedCurrent;
    private float speedFirst;
    [SerializeField, Header("速度の上昇値")]
    private float tempoUp;

    private static float missCnt;    //ミスのカウント
    /// <summary>
    /// ミス回数を増やす
    /// </summary>
    public static float AddMissCnt { set { missCnt += value; } }
    public int[] conditionsNum = new int[2];   //条件の数
    private static bool isTempoUpped;
    /// <summary>
    /// スピードを上げる
    /// </summary>
    public static bool SetIsTempoUpped { set { isTempoUpped = value; } }

    // Start is called before the first frame update
    void Start()
    {
        speedFirst = speedCurrent;
        missCnt = 0;
        isTempoUpped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (missCnt >= conditionsNum[(int)GameManager.GetInstance().GetLevelState() - 2])
        {
            // 初期化
            missCnt = 0;
            speedCurrent = speedFirst;
        }
        else if (isTempoUpped)
        {
            // スピードアップ
            isTempoUpped = false;
            missCnt = 0;
            speedCurrent *= tempoUp;
        }
    }
}
