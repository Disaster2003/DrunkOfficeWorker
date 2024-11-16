using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector2 startPoint = new Vector2(0, 0); // 開始地点の座標
    public Vector2 endPoint = new Vector2(10, 0);  // 終了地点の座標
    public float moveSpeed = 1f; // 移動速度

    [Header("揺れの幅、頻度")]
    public float shakeAmplitude = 5.0f; // 揺れの振幅
    public float shakeFrequency = 5.0f; // 揺れの頻度
    private float shakeTime = 0f; // 揺れの時間を管理する

    private float totalDistance; // 開始から終了地点までの距離
    private float currentDistance = 0f; // 現在の移動した距離

    public bool isStopped = true; // 移動停止フラグ

    void Start()
    {
        // 総距離を計算
        totalDistance = Vector3.Distance(startPoint, endPoint);
        transform.position = startPoint; // 初期位置を開始地点に設定
    }

    //何かのアクションのフラグ
    public void SetMoveStart()
    {
        isStopped = true;
    }

    public void SetMoveStop()
    {
        isStopped = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isStopped)
            {
                SetMoveStart();
            }
            else
            {
                SetMoveStop();
            }
        }
        if (isStopped)
        {
            if (startPoint != null && endPoint != null)
            {
                // 移動した距離を更新
                currentDistance += moveSpeed * Time.deltaTime;

                // 距離が全体の距離を超えないようにクランプ
                currentDistance = Mathf.Clamp(currentDistance, 0f, totalDistance);

                // 現在の位置を線形補間で計算
                float t = currentDistance / totalDistance;
                Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, t);

                // 小刻みに揺れる動きをPerlinNoiseで加える
                shakeTime += Time.deltaTime * shakeFrequency;
                float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude で揺れが-振幅~+振幅の範囲で小刻みに動く
                newPosition.y += shakeOffset;

                // 新しい位置に揺れのオフセットを加える
                newPosition.y += shakeOffset;

                // 新しい位置を設定
                transform.position = newPosition;

                // 終了地点に到達した場合は自動で停止
                if (currentDistance >= totalDistance)
                {
                    isStopped = false;
                }
            }
        }
    }
}
