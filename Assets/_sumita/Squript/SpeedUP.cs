using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUP : MonoBehaviour
{
    public float firstSpeed;    //最初のスピード
    public float currentSpeed;   //現在のスピード
    public float upSpeed;       //上げるスピード
    public int conditionsNum = 10;   //条件の数
    private static float missCnt;    //ミスのカウント

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = firstSpeed;
        missCnt = 0;
    }

    public static void UpMissCnt()
    {
        missCnt++;
    }

    public void SpeedUp()
    {
        currentSpeed += upSpeed;
    }

    public void finishSpeedUp()
    {
        currentSpeed = firstSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (missCnt >= conditionsNum)
        {
            finishSpeedUp();
        }

        transform.position = Vector3.left * currentSpeed * Time.deltaTime;
    }
}
