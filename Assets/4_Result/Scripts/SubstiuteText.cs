using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubstiuteText : MonoBehaviour
{
    private Text txt;
    private Text txtChild;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        txt = GetComponent<Text>();
        txtChild = transform.GetChild(0).GetComponent<Text>();

        Invoke(nameof(_SubstiuteText), 0.5f);
    }

    /// <summary>
    /// �e�L�X�g��u��������
    /// </summary>
    private void _SubstiuteText()
    {
        txtChild.text = txt.text;
        GetComponent<SubstiuteText>().enabled = false;
    }
}
