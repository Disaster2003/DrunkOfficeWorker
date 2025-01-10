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
        HurdleSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // ���g�̔j��
        if (count >= 10)
        {
            Destroy(gameObject);
            return;
        }

        // �X�|�[��
        HurdleSpawn();
    }

    /// <summary>
    /// ��������
    /// ���̂Ƃ��납��ǂ�ł��炤
    /// </summary>
    private void HurdleSpawn()
    {
        // �v���n�u�����݂��Ă����
        if (pfb_Object.Length <= 0) return;

        if(count >= 3)
        {
            bool isPrimeNumbers = false;
            if (count == 3)
            {
                isPrimeNumbers = true;
            }
            else if(count % 2 > 0 && count % 3 > 0)
            {
                // �f�������ׂ�
                for (int i = 5; i * i <= count; i++)
                {
                    if (count % i == 0 || count % (i + 2) == 0)
                    {
                        isPrimeNumbers = false;
                        break;
                    }

                    isPrimeNumbers = true;
                }
            }

            // �X�s�[�h�A�b�vON
            if (isPrimeNumbers) SpeedAdjust.SetIsTempoUpped = true;
        }

        // �ꉞ�z��ɑΉ����ė����𐶐�����
        // ����Ȃ�������ύX����
        int rand = Random.Range(0, pfb_Object.Length);

        // ��������
        Instantiate(pfb_Object[rand], instancePos, Quaternion.identity);

        // �����J�E���g��i�߂�
        count++;
    }

    private void OnDestroy()
    {
        ButtonAction.ClearNumberGenerate();
    }
}
