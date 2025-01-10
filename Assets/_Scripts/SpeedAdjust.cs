using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjust : MonoBehaviour
{
    [Header("初期速度を入力してください")]
    public float speedCurrent;
    private float speedFirst;
    [SerializeField, Header("速度の上昇倍率")]
    private float fTempoUp;

    private static float fCountMiss;
    public int[] iArrayFailMax = new int[4];
    private static bool isTempoUpped;
    /// <summary>
    /// スピードを上げる
    /// </summary>
    public static bool SetIsTempoUpped { set { isTempoUpped = value; } }

    // Start is called before the first frame update
    void Start()
    {
        speedFirst = speedCurrent;
        fCountMiss = 0;
        isTempoUpped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fCountMiss >= iArrayFailMax[(int)GameManager.GetInstance.GetLevelState])
        {
            // 初期化
            fCountMiss = 0;
            speedCurrent = speedFirst;
        }
        else if (isTempoUpped)
        {
            // スピードアップ
            isTempoUpped = false;
            fCountMiss = 0;
            speedCurrent *= fTempoUp;
        }
    }

    /// <summary>
    /// ミス回数を増やす
    /// </summary>
    public static void AddMissCnt() { fCountMiss++; }
}
