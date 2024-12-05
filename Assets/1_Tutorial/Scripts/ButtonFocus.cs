using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFocus : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private Color colorFocus;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererコンポーネントが未取得です");
            return;
        }

        GameObject hurdle = GameObject.FindGameObjectWithTag("Hurdle");
        if (hurdle)
        {
            if (hurdle.transform.childCount == 0)
            {
                // 透明化
                spriteRenderer.color = Color.clear;
            }
            else
            {
                // 不透明
                spriteRenderer.color = colorFocus;
            }
        }
    }
}
