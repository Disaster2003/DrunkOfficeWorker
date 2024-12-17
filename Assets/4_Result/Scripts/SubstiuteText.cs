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
        // コンポーネントの取得
        txt = GetComponent<Text>();
        txtChild = transform.GetChild(0).GetComponent<Text>();

        Invoke(nameof(_SubstiuteText), 0.5f);
    }

    /// <summary>
    /// テキストを置き換える
    /// </summary>
    private void _SubstiuteText()
    {
        txtChild.text = txt.text;
        GetComponent<SubstiuteText>().enabled = false;
    }
}
