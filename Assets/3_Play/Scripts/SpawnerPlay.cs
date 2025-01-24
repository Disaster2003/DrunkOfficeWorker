using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlay : MonoBehaviour
{
    [SerializeField] private GameObject[] goArrayHurdle;

    private static int iCountSpawn;
    /// <summary>
    /// スポーンした数を取得する
    /// </summary>
    public static int GetSpawnCount { get { return iCountSpawn; } }

    private float iIntervalSpawn;

    // Start is called before the first frame update
    void Start()
    {
        iCountSpawn = 0;
        iIntervalSpawn = 1.0f;

        HurdleSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // 自身の破棄
        if (iCountSpawn >= 10)
        {
            Destroy(gameObject);
            return;
        }

        // スポーン
        if (iIntervalSpawn <= 0) HurdleSpawn();
        else iIntervalSpawn += -Time.deltaTime;
    }

    /// <summary>
    /// 障害物を生成する
    /// </summary>
    private void HurdleSpawn()
    {
        iIntervalSpawn = 1.0f;

        if (goArrayHurdle.Length <= 0)
        {
            Debug.LogError("スポーンさせる障害物が未設定です");
            return;
        }

        if(iCountSpawn >= 3)
        {
            // 素数か調べる
            bool isPrimeNumbers = false;
            if (iCountSpawn == 3) isPrimeNumbers = true;
            else if (iCountSpawn % 2 > 0 && iCountSpawn % 3 > 0)
            {
                //// 素数か調べる(countが11以上)
                //for (int i = 5; i * i <= count; i++)
                //{
                //    if (count % i == 0 || count % (i + 2) == 0)
                //    {
                //        isPrimeNumbers = false;
                //        break;
                //    }

                isPrimeNumbers = true;
                //}
            }

            // スピードアップON
            if (isPrimeNumbers) SpeedAdjust.SetIsTempoUpped = true;
        }

        // ランダムで生成する
        int rand = Random.Range(0, goArrayHurdle.Length);
        Instantiate(goArrayHurdle[rand]);

        // 生成カウントを進める
        iCountSpawn++;
    }

    private void OnDestroy()
    {
        ButtonAction.ClearNumberGenerate();
    }
}
