using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUP : MonoBehaviour
{
    [Header("初期速度を入力してください")]
    public float speedCurrent;
    private float speedFirst;
    [SerializeField, Header("速度の上昇値")]
    private float upSpeed;
    public int conditionsNum = 10;   //条件の数
    private static float missCnt;    //ミスのカウント

    // Start is called before the first frame update
    void Start()
    {
        speedFirst = speedCurrent;
        missCnt = 0;
    }

    public static void UpMissCnt()
    {
        missCnt++;
    }

    public void SpeedUp()
    {
        speedCurrent += upSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (missCnt >= conditionsNum)
        {
            // 初期化
            speedCurrent = speedFirst;
        }
    }
}
