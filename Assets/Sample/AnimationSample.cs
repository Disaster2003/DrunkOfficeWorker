using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSample : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sample;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sample[0];
    }

    // Update is called once per frame
    void Update()
    {
        Animation(sample);
    }

    /// <summary>
    /// アニメーション処理
    /// </summary>
    /// <param name="_sprite">アニメーション画像</param>
    private void Animation(Sprite[] _sprite)
    {
        for(int i = 0; i < _sprite.Length; i++)
        {
            if(spriteRenderer == _sprite[i])
            {
                if(i==_sprite.Length-1)
                {
                    // 最初の画像に戻す
                    spriteRenderer.sprite = _sprite[0];
                }
                else
                {
                    // 次の画像へ
                    spriteRenderer.sprite = _sprite[i];
                }
            }
            else if (i == _sprite.Length - 1)
            {
                // 画像を変更する
                spriteRenderer.sprite = _sprite[0];
            }

        }
    }
}
