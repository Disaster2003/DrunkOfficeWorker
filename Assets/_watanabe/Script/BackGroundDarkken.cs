using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundDarkken : MonoBehaviour
{
    [SerializeField, Header("どこまで暗くするか")]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);

    [SerializeField, Header("暗くする画像")]
    private Image imgBackGround;

    [SerializeField, Header("補間度合い"), Range(0, 0.5f)]
    float test_t;

    private void Update()
    {
        // テスト用
        //Darkken(test_t);
    }

    /// <summary>
    ///  画像を暗くする
    ///  タイマーあたりで呼ぶ
    /// </summary>
    /// <param name="t">補間度合い</param>
    public void Darkken(float t)
    {
        if(!imgBackGround)
        {
            Debug.Log(gameObject.name + "に暗くする画像が割り振られていないよ");
        }
        imgBackGround.color = Color.Lerp(Color.white, darkColor, t);
    }
}
