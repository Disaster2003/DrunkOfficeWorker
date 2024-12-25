using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUP : MonoBehaviour
{
    public float firstSpeed;    //�ŏ��̃X�s�[�h
    public float currentSpeed;   //���݂̃X�s�[�h
    public float upSpeed;       //�グ��X�s�[�h
    public int conditionsNum = 10;   //�����̐�
    private static float missCnt;    //�~�X�̃J�E���g

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
