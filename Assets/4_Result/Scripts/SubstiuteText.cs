using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubstiuteText : MonoBehaviour
{
    private Text text;
    private Text txtChild;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        text = GetComponent<Text>();
        txtChild = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.text != txtChild.text)
        {
            // �q�R���|�[�l���g�̃e�L�X�g�𑵂���
            txtChild.text = text.text;
            text.enabled = false;
        }
    }
}
