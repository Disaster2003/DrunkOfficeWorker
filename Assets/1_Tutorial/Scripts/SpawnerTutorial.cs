using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] hurdleTutorial;
    private int indexSpawn;

    // Start is called before the first frame update
    void Start()
    {
        indexSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Hurdle"))
        {
            // ���g�̔j��
            if (indexSpawn >= hurdleTutorial.Length) Destroy(gameObject);
            // �X�|�[��
            else HurdleSpawn();
        }
    }

    /// <summary>
    /// ��Q���𐶐�����
    /// </summary>
    private void HurdleSpawn()
    {
        Instantiate(hurdleTutorial[indexSpawn]);
        indexSpawn++;
    }
}
