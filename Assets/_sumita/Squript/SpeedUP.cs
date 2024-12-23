using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SpeedUP : MonoBehaviour
{
    public float firstSpeed;    //�ŏ��̃X�s�[�h
    public float currentSpeed;   //���݂̃X�s�[�h
    public float upSpeed;       //�グ��X�s�[�h
    public int conditionsNum = 10;   //�����̐�
    private int missCnt;    //�~�X�̃J�E���g

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
