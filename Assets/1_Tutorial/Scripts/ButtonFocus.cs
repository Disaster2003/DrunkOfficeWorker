using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFocus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField, Header("ボタンをフォーカスする障害物の開始位置")]
    private Vector2 positionFocus;

    [SerializeField, Header("ボタンに注目させる背景色")]
    private Color colorFocus;

    [SerializeField] private Text txtTutorial;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 非表示
        spriteRenderer.color = Color.clear;
        txtTutorial.color = Color.clear;
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
            txtTutorial.color = Color.clear;
        }
        else if(Vector3.Distance(hurdle.transform.position, positionFocus) < 0.1f)
        {
            // 不透明化
            spriteRenderer.color = colorFocus;
            txtTutorial.color = Color.white;
        }
    }
}
