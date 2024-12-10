using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFocus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField, Header("ボタンのフォーカスを開始する障害物の位置")]
    private Vector3 positionFocus;

    [SerializeField, Header("ボタンに注目させるための背景色")]
    private Color colorFocus;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        // nullチェック
        if(spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererコンポーネントが未取得です");
            return;
        }

        GameObject hurdle = GameObject.FindGameObjectWithTag("Hurdle");
        if (!hurdle) return;

        if (hurdle.transform.childCount == 0)
        {
            // 透明化
            spriteRenderer.color = Color.clear;
        }
        else if(Vector3.Distance(hurdle.transform.position, positionFocus) < 0.1f)
        {
            // 不透明化
            spriteRenderer.color = colorFocus;
        }
    }
}
