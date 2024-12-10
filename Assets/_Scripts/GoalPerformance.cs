using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPerformance : MonoBehaviour
{
    [SerializeField] private GameObject spawner;

    // 縮小速度
    public float scaleSpeed = 0.5f;

    [Header("最終サイズスケール")]
    // 最小スケールサイズ
    public Vector3 minimumScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("ただの移動から縮小移動に変わる場所")]
    //途中まで行く座標
    public Vector2 positionGoal;

    //最初の速さ
    [SerializeField] private float speedMove;

    //移動から縮小移動に変更するフラグ
    public bool isArrived = false;

    // Update is called once per frame
    void Update()
    {
        if (spawner) return;

        if (isArrived)
        {
            // 現在のスケールが最小スケールより大きい場合のみ縮小
            if (transform.localScale.x > minimumScale.x)
            {
                // スケールを徐々に縮小
                transform.localScale = Vector3.MoveTowards(transform.localScale, minimumScale, scaleSpeed * Time.deltaTime);
            }
            else
            {
                // 結果画面へ
                GameManager.GetInstance().SetNextScene(GameManager.STATE_SCENE.RANKING);
                GameManager.GetInstance().StartChangingScene();

                // 自身の破棄
                Destroy(gameObject);
            }
        }
        else
        {
            //現在地を取得
            Vector2 currentPosition = transform.position;

            if (Vector2.Distance(currentPosition, positionGoal) < 0.1f)
            {
                isArrived = true;
                return;
            }

            //移動
            transform.position = Vector3.MoveTowards(currentPosition, positionGoal, speedMove * Time.deltaTime);
        }
    }
}
