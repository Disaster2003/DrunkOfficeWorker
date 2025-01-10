using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjust : MonoBehaviour
{
    [Header("�������x����͂��Ă�������")]
    public float speedCurrent;
    private float speedFirst;
    [SerializeField, Header("���x�̏㏸�{��")]
    private float fTempoUp;

    private static float fCountMiss;
    public int[] iArrayFailMax = new int[4];
    private static bool isTempoUpped;
    /// <summary>
    /// �X�s�[�h���グ��
    /// </summary>
    public static bool SetIsTempoUpped { set { isTempoUpped = value; } }

    // Start is called before the first frame update
    void Start()
    {
        speedFirst = speedCurrent;
        fCountMiss = 0;
        isTempoUpped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fCountMiss >= iArrayFailMax[(int)GameManager.GetInstance.GetLevelState])
        {
            // ������
            fCountMiss = 0;
            speedCurrent = speedFirst;
        }
        else if (isTempoUpped)
        {
            // �X�s�[�h�A�b�v
            isTempoUpped = false;
            fCountMiss = 0;
            speedCurrent *= fTempoUp;
        }
    }

    /// <summary>
    /// �~�X�񐔂𑝂₷
    /// </summary>
    public static void AddMissCnt() { fCountMiss++; }
}
