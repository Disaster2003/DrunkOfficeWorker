using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjust : MonoBehaviour
{
    [SerializeField, Header("�������x����͂��Ă�������")]
    private float SPEED_FIRST;
    private static float speedCurrent = 0;
    /// <summary>
    /// ���݂̑��x���擾����
    /// </summary>
    public static float GetCurrentSpeed {  get { return speedCurrent; } }
    [SerializeField, Header("���x�̏㏸�{��")]
    private float fTempoUp;

    private static float fCountMiss = 0;
    [SerializeField, Header("�~�X���(�`���[�g���A���A��-�㋉)")]
    private int[] iArrayFailMax = new int[4];
    private static bool isTempoUpped;
    /// <summary>
    /// �X�s�[�h���グ��
    /// </summary>
    public static bool SetIsTempoUpped { set { isTempoUpped = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if (speedCurrent == 0) speedCurrent = SPEED_FIRST;
        isTempoUpped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fCountMiss >= iArrayFailMax[(int)GameManager.GetInstance.GetLevelState])
        {
            // ������
            fCountMiss = 0;
            speedCurrent = SPEED_FIRST;
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
