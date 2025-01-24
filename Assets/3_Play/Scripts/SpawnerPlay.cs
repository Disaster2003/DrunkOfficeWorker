using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlay : MonoBehaviour
{
    [SerializeField] private GameObject[] goArrayHurdle;

    private static int iCountSpawn;
    /// <summary>
    /// �X�|�[�����������擾����
    /// </summary>
    public static int GetSpawnCount { get { return iCountSpawn; } }

    private float iIntervalSpawn;

    // Start is called before the first frame update
    void Start()
    {
        iCountSpawn = 0;
        iIntervalSpawn = 1.0f;

        HurdleSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // ���g�̔j��
        if (iCountSpawn >= 10)
        {
            Destroy(gameObject);
            return;
        }

        // �X�|�[��
        if (iIntervalSpawn <= 0) HurdleSpawn();
        else iIntervalSpawn += -Time.deltaTime;
    }

    /// <summary>
    /// ��Q���𐶐�����
    /// </summary>
    private void HurdleSpawn()
    {
        iIntervalSpawn = 1.0f;

        if (goArrayHurdle.Length <= 0)
        {
            Debug.LogError("�X�|�[���������Q�������ݒ�ł�");
            return;
        }

        if(iCountSpawn >= 3)
        {
            // �f�������ׂ�
            bool isPrimeNumbers = false;
            if (iCountSpawn == 3) isPrimeNumbers = true;
            else if (iCountSpawn % 2 > 0 && iCountSpawn % 3 > 0)
            {
                //// �f�������ׂ�(count��11�ȏ�)
                //for (int i = 5; i * i <= count; i++)
                //{
                //    if (count % i == 0 || count % (i + 2) == 0)
                //    {
                //        isPrimeNumbers = false;
                //        break;
                //    }

                isPrimeNumbers = true;
                //}
            }

            // �X�s�[�h�A�b�vON
            if (isPrimeNumbers) SpeedAdjust.SetIsTempoUpped = true;
        }

        // �����_���Ő�������
        int rand = Random.Range(0, goArrayHurdle.Length);
        Instantiate(goArrayHurdle[rand]);

        // �����J�E���g��i�߂�
        iCountSpawn++;
    }

    private void OnDestroy()
    {
        ButtonAction.ClearNumberGenerate();
    }
}
