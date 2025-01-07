using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUP : MonoBehaviour
{
    [Header("�������x����͂��Ă�������")]
    public float speedCurrent;
    private float speedFirst;
    [SerializeField, Header("���x�̏㏸�l")]
    private float upSpeed;
    public int conditionsNum = 10;   //�����̐�
    private static float missCnt;    //�~�X�̃J�E���g

    // Start is called before the first frame update
    void Start()
    {
        speedFirst = speedCurrent;
        missCnt = 0;
    }

    public static void UpMissCnt()
    {
        missCnt++;
    }

    public void SpeedUp()
    {
        speedCurrent += upSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (missCnt >= conditionsNum)
        {
            // ������
            speedCurrent = speedFirst;
        }
    }
}
