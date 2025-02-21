using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    private Text txtMeter;
    private float fMeter;

    [SerializeField] GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        txtMeter = GetComponent<Text>();

        fMeter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fMeter = 100 -  HurdleComponent.DestroyCount * 10;

        if(fMeter == 0)
        {
            // ���g�̔j��
            Destroy(arrow);
            Destroy(transform.parent.gameObject);
        }
        // ���[�^�[�X�V
        else txtMeter.text = $"�c��{fMeter.ToString("f0")}m";
    }
}
