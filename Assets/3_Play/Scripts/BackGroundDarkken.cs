using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundDarkken : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererChild;

    [SerializeField, Header("暗さ")]
    private Color colorDark = new Color(0.5f, 0.5f, 0.5f, 1);

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 画像を暗くする
    /// </summary>
    /// <param name="t">補間度合い</param>
    public void Darkken(float t)
    {
        spriteRenderer.color = Color.Lerp(Color.white, colorDark, t);
        spriteRendererChild.color = Color.Lerp(Color.white, colorDark, t);
    }
}
