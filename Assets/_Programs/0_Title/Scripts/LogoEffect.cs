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
        // コンポーネントの取得
        imgTitleLogo = GetComponent<Image>();
        imgTitleLogo.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (imgTitleLogo.fillAmount >= 1) return;

        if(fIntervalAppearLogo <= 0)
        {
            // ロゴ表示
            imgTitleLogo.fillAmount += Time.deltaTime;
        }
        else
        {
            // ロゴ表示のインターバル中
            fIntervalAppearLogo -= Time.deltaTime;
        }
    }
}
