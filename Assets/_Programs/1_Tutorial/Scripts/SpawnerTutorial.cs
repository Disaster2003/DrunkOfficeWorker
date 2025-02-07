using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] goArrayHurdle;
    private int iIndexSpawn;
    private float iIntervalSpawn;

    [SerializeField] private Text txtTutorial;

    // Start is called before the first frame update
    void Start()
    {
        iIndexSpawn = 0;
        iIntervalSpawn = 1.0f;

        HurdleSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // ���g�̔j��
        if (iIndexSpawn >= goArrayHurdle.Length)
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

        Instantiate(goArrayHurdle[iIndexSpawn]);

        // �����J�E���g��i�߂�
        iIndexSpawn++;

        switch (iIndexSpawn)
        {
            case 1:
                txtTutorial.text = "����!!";
                break;
            case 2:
                txtTutorial.text = "�Ȃ�����!!";
                break;
            case 3:
                txtTutorial.text = "���!!";
                break;
        }
    }
}
