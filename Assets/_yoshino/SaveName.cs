using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveName : MonoBehaviour
{
    private Text txtName;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        txtName = GetComponent<Text>();
    }

    private void OnDestroy()
    {
        // ���͂������O��ۑ�
        PlayerPrefs.SetString("PlayerName", txtName.text);
    }
}
