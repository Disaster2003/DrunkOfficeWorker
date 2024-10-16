using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSample : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sample;
    private float intervalAnimation;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sample[0];
        intervalAnimation = 0;
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
        if (intervalAnimation > 0.2f)
        {
            intervalAnimation = 0;

            for (int i = 0; i < _sprite.Length; i++)
            {
                if (spriteRenderer.sprite == _sprite[i])
                {
                    if (i == _sprite.Length - 1)
                    {
                        // �ŏ��̉摜�ɖ߂�
                        spriteRenderer.sprite = _sprite[0];
                        return;
                    }
                    else
                    {
                        // ���̉摜��
                        spriteRenderer.sprite = _sprite[i + 1];
                        return;
                    }
                }
                else if (i == _sprite.Length - 1)
                {
                    // �摜��ύX����
                    spriteRenderer.sprite = _sprite[0];
                }

            }
        }
        else
        {
            // �A�j���[�V�����̃C���^�[�o����
            intervalAnimation += Time.deltaTime;
        }
    }
}
