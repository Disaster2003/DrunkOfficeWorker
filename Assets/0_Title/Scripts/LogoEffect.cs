using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoEffect : MonoBehaviour
{
    private Image imgTitleLogo;
    [SerializeField] private float fIntervalAppearLogo;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        imgTitleLogo = GetComponent<Image>();
        imgTitleLogo.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (imgTitleLogo.fillAmount >= 1) return;

        if(fIntervalAppearLogo <= 0)
        {
            // ���S�\��
            imgTitleLogo.fillAmount += Time.deltaTime;
        }
        else
        {
            // ���S�\���̃C���^�[�o����
            fIntervalAppearLogo -= Time.deltaTime;
        }
    }
}
