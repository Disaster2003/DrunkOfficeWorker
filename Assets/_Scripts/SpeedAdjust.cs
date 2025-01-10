using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjust : MonoBehaviour
{
    [SerializeField, Header("初期速度を入力してください")]
    private float SPEED_FIRST;
    private static float speedCurrent = 0;
    /// <summary>
    /// 現在の速度を取得する
    /// </summary>
    public static float GetCurrentSpeed {  get { return speedCurrent; } }
    [SerializeField, Header("速度の上昇倍率")]
    private float fTempoUp;

    private static float fCountMiss = 0;
    [SerializeField, Header("ミス上限(チュートリアル、初-上級)")]
    private int[] iArrayFailMax = new int[4];
    private static bool isTempoUpped;
    /// <summary>
    /// スピードを上げる
    /// </summary>
    public static bool SetIsTempoUpped { set { isTempoUpped = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if (speedCurrent == 0) speedCurrent = SPEED_FIRST;
        isTempoUpped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fCountMiss >= iArrayFailMax[(int)GameManager.GetInstance.GetLevelState])
        {
            // 初期化
            fCountMiss = 0;
            speedCurrent = SPEED_FIRST;
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
