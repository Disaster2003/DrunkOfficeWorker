using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAction : MonoBehaviour
{
    // 縮小速度
    public float scaleSpeed = 0.5f;

    [Header("最終サイズスケール")]
    // 最小スケールサイズ
    public Vector3 minimumScale = new Vector3(0.1f, 0.1f, 0.1f);

    // 初期スケール
    private Vector3 initialScale;

    [Header("ただの移動から縮小移動に変わる場所")]
    //途中まで行く座標
    public Vector2 changePosition;

    [Header("最終的に行く座標")]
    public Vector2 targetPosition;

    //最初の速さ
    public float firstSpeed = 50.0f;

    //次の速さ
    public float secondSpeed = 50.0f;

    //ゴールについたのか
    public bool isGoalflg;

    //移動から縮小移動に変更するフラグ
    bool isChangeMoveflg = false;

    void Start()
    {
        // オブジェクトの初期スケールを保存
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (isGoalflg)
        {
            //現在地を取得
            Vector2 currentPosition = transform.position;

            //移動
            transform.position = Vector3.MoveTowards(currentPosition, changePosition, firstSpeed * Time.deltaTime);

            if (Vector2.Distance(currentPosition, changePosition) < 0.1f)
            {
                isChangeMoveflg = true;
                isGoalflg = false;
            }
        }
        if (isChangeMoveflg)
        {
            // 現在のスケールが最小スケールより大きい場合のみ縮小
            if (transform.localScale.x > minimumScale.x)
            {
                // スケールを徐々に縮小
                transform.localScale = Vector3.MoveTowards(transform.localScale, minimumScale, scaleSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
