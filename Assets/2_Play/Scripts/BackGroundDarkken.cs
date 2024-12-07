using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundDarkken : MonoBehaviour
{
    [SerializeField, Header("�ǂ��܂ňÂ����邩")]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);

    [SerializeField, Header("�Â�����摜")]
    private SpriteRenderer spriteRenderer;

    [SerializeField, Header("�e�X�g���[�h")]
    private bool isTest;

    [SerializeField, Header("(Test��)��ԓx����"), Range(0, 0.5f)]
    private float test_t;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isTest) return;

        Darkken(test_t);
    }

    /// <summary>
    ///  �摜���Â�����
    ///  �^�C�}�[������ŌĂ�
    /// </summary>
    /// <param name="t">��ԓx����</param>
    public void Darkken(float t)
    {
        spriteRenderer.color = Color.Lerp(Color.white, darkColor, t);
    }
}
