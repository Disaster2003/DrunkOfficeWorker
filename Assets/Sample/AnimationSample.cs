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
    /// �A�j���[�V��������
    /// </summary>
    /// <param name="_sprite">�A�j���[�V�����摜</param>
    private void Animation(Sprite[] _sprite)
    {
        for(int i = 0; i < _sprite.Length; i++)
        {
            if(spriteRenderer == _sprite[i])
            {
                if(i==_sprite.Length-1)
                {
                    // �ŏ��̉摜�ɖ߂�
                    spriteRenderer.sprite = _sprite[0];
                }
                else
                {
                    // ���̉摜��
                    spriteRenderer.sprite = _sprite[i];
                }
            }
            else if (i == _sprite.Length - 1)
            {
                // �摜��ύX����
                spriteRenderer.sprite = _sprite[0];
            }

        }
    }
}
