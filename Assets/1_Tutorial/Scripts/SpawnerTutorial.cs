using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] goArrayHurdle;
    private int iIndexSpawn;
    private float iIntervalSpawn;

    // Start is called before the first frame update
    void Start()
    {
        iIndexSpawn = 0;
        iIntervalSpawn = 1.0f;

        HurdleSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // 自身の破棄
        if (iIndexSpawn >= goArrayHurdle.Length)
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

        Instantiate(goArrayHurdle[iIndexSpawn]);

        // 生成カウントを進める
        iIndexSpawn++;
    }
}
