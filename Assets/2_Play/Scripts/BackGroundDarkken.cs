using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundDarkken : MonoBehaviour
{
    [SerializeField, Header("どこまで暗くするか")]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);

    [SerializeField, Header("暗くする画像")]
    private SpriteRenderer spriteRenderer;

    [SerializeField, Header("テストモード")]
    private bool isTest;

    [SerializeField, Header("(Test時)補間度合い"), Range(0, 0.5f)]
    private float test_t;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isTest) return;

        Darkken(test_t);
    }

    /// <summary>
    ///  画像を暗くする
    ///  タイマーあたりで呼ぶ
    /// </summary>
    /// <param name="t">補間度合い</param>
    public void Darkken(float t)
    {
        spriteRenderer.color = Color.Lerp(Color.white, darkColor, t);
    }
}
