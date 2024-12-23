using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlay : MonoBehaviour
{
    [SerializeField, Header("生成するプレハブ")]
    private GameObject[] pfb_Object;

    [SerializeField, Header("生成位置")]
    private Vector3 instancePos;

    // 生成した数
    [SerializeField, Header("確認用いじらないで")]
    private static int count;

    // 生成した数のゲッター
    public static int GetCount { get { return count; } }

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        HurdleSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // 自身の破棄
        if (count >= 10) Destroy(gameObject);
        // スポーン
        else HurdleSpawn();
    }

    /// <summary>
    /// 生成する
    /// 他のところから読んでもらう
    /// </summary>
    private void HurdleSpawn()
    {
        // プレハブが存在していれば
        if (pfb_Object.Length <= 0) return;

        // 一応配列に対応して乱数を生成する
        // いらなかったら変更して
        int i = Random.Range(0, pfb_Object.Length);

        // 生成して
        Instantiate(pfb_Object[i], instancePos, Quaternion.identity);

        // 生成カウントを進める
        count++;
    }

    private void OnDestroy()
    {
        ButtonAction.ClearNumberGenerate();
    }
}
