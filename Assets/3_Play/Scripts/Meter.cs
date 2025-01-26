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

        fMeter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fMeter = 100 -  HurdleComponent.DestroyCount * 10;
        txtMeter.text = $"残り{fMeter.ToString("f0")}m";
    }
}
