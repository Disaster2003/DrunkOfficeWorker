using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoEffect : MonoBehaviour
{
    private Image imgTitleLogo;
    [SerializeField] private float interval;

    // Start is called before the first frame update
    void Start()
    {
        imgTitleLogo = GetComponent<Image>();
        imgTitleLogo.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (imgTitleLogo.fillAmount >= 1) return;

        // ロゴエフェクト
        if(interval < 0)
        {
            imgTitleLogo.fillAmount += Time.deltaTime;
        }
        else
        {
            interval -= Time.deltaTime;
        }
    }
}
