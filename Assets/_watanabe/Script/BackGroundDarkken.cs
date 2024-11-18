using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundDarkken : MonoBehaviour
{
    [SerializeField, Header("�ǂ��܂ňÂ����邩")]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);

    [SerializeField, Header("�Â�����摜")]
    private Image imgBackGround;

    [SerializeField, Header("�e�X�g���[�h")]
    private bool isTest;

    [SerializeField, Header("(Test��)��ԓx����"), Range(0, 0.5f)]
    private float test_t;

    private void Update()
    {
        if (isTest)
        {
            Darkken(test_t);
        }
    }

    /// <summary>
    ///  �摜���Â�����
    ///  �^�C�}�[������ŌĂ�
    /// </summary>
    /// <param name="t">��ԓx����</param>
    public void Darkken(float t)
    {
        if(!imgBackGround)
        {
            Debug.Log(gameObject.name + "�ɈÂ�����摜������U���Ă��Ȃ���");
        }
        imgBackGround.color = Color.Lerp(Color.white, darkColor, t);
    }
}
