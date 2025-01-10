using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPerformance : MonoBehaviour
{
    [SerializeField] private GameObject spawner;

    [SerializeField, Header("縮小速度")]
    private float fSpeedShrink = 0.5f;
    [SerializeField, Header("最小サイズ")]
    private Vector3 scaleMin = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField, Header("縮小の目標地点")]
    private Vector3 positionGoal = Vector3.zero;
    [SerializeField, Header("移動速度")]
    private float fSpeedMove;

    public bool isArrived = false; // true = 縮小地点に到着, false = 未到達

    // Update is called once per frame
    void Update()
    {
        if (spawner) return;

        if (isArrived)
        {
            // 現在のスケールが最小スケールより大きい場合のみ縮小
            if (transform.localScale.x > scaleMin.x)
            {
                // スケールを徐々に縮小
                transform.localScale = Vector3.MoveTowards(transform.localScale, scaleMin, fSpeedShrink * Time.deltaTime);
            }
            else
            {
                // 結果画面へ
                GameManager.GetInstance.SetNextScene(GameManager.STATE_SCENE.RESULT);
                GameManager.GetInstance.StartChangingScene();

                // 自身の破棄
                Destroy(gameObject);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, positionGoal) < 0.1f)
            {
                // 目標地点に到着
                isArrived = true;
                return;
            }

            // 移動
            transform.position = Vector3.MoveTowards(transform.position, positionGoal, fSpeedMove * Time.deltaTime);
        }
    }
}
