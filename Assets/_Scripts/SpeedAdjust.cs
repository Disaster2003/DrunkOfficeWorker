using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjust : MonoBehaviour
{
    [Header("�������x����͂��Ă�������")]
    public float speedCurrent;
    private float speedFirst;
    [SerializeField, Header("���x�̏㏸�l")]
    private float tempoUp;

    private static float missCnt;    //�~�X�̃J�E���g
    /// <summary>
    /// �~�X�񐔂𑝂₷
    /// </summary>
    public static float AddMissCnt { set { missCnt += value; } }
    public int[] conditionsNum = new int[2];   //�����̐�
    private static bool isTempoUpped;
    /// <summary>
    /// �X�s�[�h���グ��
    /// </summary>
    public static bool SetIsTempoUpped { set { isTempoUpped = value; } }

    // Start is called before the first frame update
    void Start()
    {
        speedFirst = speedCurrent;
        missCnt = 0;
        isTempoUpped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (missCnt >= conditionsNum[(int)GameManager.GetInstance().GetLevelState() - 2])
        {
            // ������
            missCnt = 0;
            speedCurrent = speedFirst;
        }
        else if (isTempoUpped)
        {
            // �X�s�[�h�A�b�v
            isTempoUpped = false;
            missCnt = 0;
            speedCurrent *= tempoUp;
        }
    }
}
