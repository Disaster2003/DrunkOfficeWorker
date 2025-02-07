using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceText : MonoBehaviour
{
    private Text txt;
    private Text txtChild;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        txt = GetComponent<Text>();
        txtChild = transform.GetChild(0).GetComponent<Text>();

        Invoke(nameof(InitializeText), 0.5f);
    }

    /// <summary>
    /// テキストを置き換える
    /// </summary>
    private void InitializeText()
    {
        txtChild.text = txt.text;
        GetComponent<ReplaceText>().enabled = false;
    }
}
