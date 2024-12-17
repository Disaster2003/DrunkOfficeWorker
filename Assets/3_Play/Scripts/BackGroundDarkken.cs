using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundDarkken : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField, Header("どこまで暗くするか")]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 画像を暗くする
    /// タイマーあたりで呼ぶ
    /// </summary>
    /// <param name="t">補間度合い</param>
    public void Darkken(float t)
    {
        spriteRenderer.color = Color.Lerp(Color.white, darkColor, t);
    }
}
