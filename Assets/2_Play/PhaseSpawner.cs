using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSpawner : MonoBehaviour
{
    [SerializeField, Header("生成するプレハブ")]
    private GameObject[] pfb_Object;

    [SerializeField, Header("生成位置")]
    private Vector3 instancePos;

    // 生成した数
    [SerializeField, Header("確認用いじらないで")]
    private int count;

    // 生成した数のゲッター
    public int get_count{ get { return count; } }

    [SerializeField, Header("テスト用")]
    private bool isTest;

    private void Start()
    {
        if (isTest)
        {
            Instance();
        }
    }

    /// <summary>
    /// 生成する
    /// 他のところから読んでもらう
    /// </summary>
    public void Instance()
    {
        // プレハブが存在していれば
        if (pfb_Object.Length > 0)
        {
            // 一応配列に対応して乱数を生成する
            // いらなかったら変更して
            int i = Random.Range(0, pfb_Object.Length-1);

            // 生成して
            Instantiate(pfb_Object[i], instancePos, Quaternion.identity);

            // 生成カウントを進める
            count++;
        }

        if (isTest)
        {
            Invoke("Instance", 2);
        }
    }

}
