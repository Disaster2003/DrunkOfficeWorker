using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSpawner : MonoBehaviour
{
    [SerializeField, Header("��������v���n�u")]
    private GameObject[] pfb_Object;

    [SerializeField, Header("�����ʒu")]
    private Vector3 instancePos;

    // ����������
    [SerializeField, Header("�m�F�p������Ȃ���")]
    private int count;

    // �����������̃Q�b�^�[
    public int get_count{ get { return count; } }

    private void Start()
    {
        // �e�X�g�p
        //Instance();
    }

    /// <summary>
    /// ��������
    /// ���̂Ƃ��납��ǂ�ł��炤
    /// </summary>
    public void Instance()
    {
        // �v���n�u�����݂��Ă����
        if (pfb_Object.Length > 0)
        {
            // �ꉞ�z��ɑΉ����ė����𐶐�����
            // ����Ȃ�������ύX����
            int i = Random.Range(0, pfb_Object.Length-1);

            // ��������
            Instantiate(pfb_Object[i], instancePos, Quaternion.identity);

            // �����J�E���g��i�߂�
            count++;
        }

        // �e�X�g�p
        //Invoke("Instance", 2);
    }

}
