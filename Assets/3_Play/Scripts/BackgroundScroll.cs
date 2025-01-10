using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("目標地点")]
    private Vector2 positionGoal = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // 初期配置
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // 背景のスクロール
        transform.Translate(10 * -Time.deltaTime, 0, 0);

        if (transform.position.x <= positionGoal.x)
        {
            // 初期位置へ
            transform.position = Vector3.zero;
        }
    }
}
