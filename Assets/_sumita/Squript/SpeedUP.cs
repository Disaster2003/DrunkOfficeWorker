using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SpeedUP : MonoBehaviour
{
    public float firstSpeed;    //最初のスピード
    public float currentSpeed;   //現在のスピード
    public float upSpeed;       //上げるスピード
    public int conditionsNum = 10;   //条件の数
    private int missCnt;    //ミスのカウント

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = firstSpeed;
    }

    void UpMissCnt()
    {
        missCnt++;
    }

    void SpeedUp()
    {
        currentSpeed += upSpeed;
    }

    void finishSpeedUp()
    {
        currentSpeed = firstSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.left * currentSpeed * Time.deltaTime;
    }
}
