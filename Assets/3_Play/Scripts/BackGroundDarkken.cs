using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundDarkken : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField, Header("�ǂ��܂ňÂ����邩")]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// �摜���Â�����
    /// �^�C�}�[������ŌĂ�
    /// </summary>
    /// <param name="t">��ԓx����</param>
    public void Darkken(float t)
    {
        spriteRenderer.color = Color.Lerp(Color.white, darkColor, t);
    }
}
