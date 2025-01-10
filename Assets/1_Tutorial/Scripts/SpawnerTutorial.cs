using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] goArrayHurdle;
    private int iIndexSpawn;

    // Start is called before the first frame update
    void Start()
    {
        iIndexSpawn = 0;
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
        HurdleSpawn();
    }

    /// <summary>
    /// 障害物を生成する
    /// </summary>
    private void HurdleSpawn()
    {
        Instantiate(goArrayHurdle[iIndexSpawn]);

        // 生成カウントを進める
        iIndexSpawn++;
    }
}
