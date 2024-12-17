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
        // コンポーネントの取得
        txtName = GetComponent<Text>();
    }

    private void OnDestroy()
    {
        // 入力した名前を保存
        PlayerPrefs.SetString("PlayerName", txtName.text);
    }
}
