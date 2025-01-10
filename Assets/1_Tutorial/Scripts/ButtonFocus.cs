using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFocus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField, Header("�{�^�����t�H�[�J�X�����Q���̊J�n�ʒu")]
    private Vector2 positionFocus;

    [SerializeField, Header("�{�^���ɒ��ڂ�����w�i�F")]
    private Color colorFocus;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        // null�`�F�b�N
        if(spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer�R���|�[�l���g�����擾�ł�");
            return;
        }

        GameObject hurdle = GameObject.FindGameObjectWithTag("Hurdle");
        if (!hurdle) return;

        if (hurdle.transform.childCount == 0)
        {
            // ������
            spriteRenderer.color = Color.clear;
        }
        else if(Vector3.Distance(hurdle.transform.position, positionFocus) < 0.1f)
        {
            // �s������
            spriteRenderer.color = colorFocus;
        }
    }
}
