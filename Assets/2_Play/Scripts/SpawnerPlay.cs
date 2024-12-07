using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlay : MonoBehaviour
{
    [SerializeField, Header("��������v���n�u")]
    private GameObject[] pfb_Object;

    [SerializeField, Header("�����ʒu")]
    private Vector3 instancePos;

    // ����������
    [SerializeField, Header("�m�F�p������Ȃ���")]
    private static int count;

    // �����������̃Q�b�^�[
    public static int GetCount { get { return count; } }

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // ���g�̔j��
        if (count >= 10) Destroy(gameObject);
        // �X�|�[��
        else HurdleSpawn();
    }

    /// <summary>
    /// ��������
    /// ���̂Ƃ��납��ǂ�ł��炤
    /// </summary>
    private void HurdleSpawn()
    {
        // �v���n�u�����݂��Ă����
        if (pfb_Object.Length <= 0) return;

        // �ꉞ�z��ɑΉ����ė����𐶐�����
        // ����Ȃ�������ύX����
        int i = Random.Range(0, pfb_Object.Length);

        // ��������
        Instantiate(pfb_Object[i], instancePos, Quaternion.identity);

        // �����J�E���g��i�߂�
        count++;
    }
}
