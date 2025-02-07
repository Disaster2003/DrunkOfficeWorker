using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundDarkken : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererChild;

    [SerializeField, Header("�Â�")]
    private Color colorDark = new Color(0.5f, 0.5f, 0.5f, 1);

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// �摜���Â�����
    /// </summary>
    /// <param name="t">��ԓx����</param>
    public void Darkken(float t)
    {
        spriteRenderer.color = Color.Lerp(Color.white, colorDark, t);
        spriteRendererChild.color = Color.Lerp(Color.white, colorDark, t);
    }
}
