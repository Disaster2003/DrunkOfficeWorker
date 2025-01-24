using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    private Text txtMeter;
    private float fMeter;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        txtMeter = GetComponent<Text>();

        fMeter = 110;
    }

    // Update is called once per frame
    void Update()
    {
        fMeter = Mathf.Lerp(fMeter, SpawnerPlay.GetSpawnCount * 10, 0.5f);
    }
}
