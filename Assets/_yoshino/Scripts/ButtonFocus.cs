using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFocus : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

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

        if (GameObject.FindGameObjectWithTag("Hurdle"))
        {
            spriteRenderer.color = new Color(0, 0, 0, 0.5f);
        }
        else
        {
            spriteRenderer.color = Color.clear;
        }
    }
}
