using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField, Header("初期位置の座標")]
    private Vector3 positionStart = Vector3.zero;
    [SerializeField, Header("移動速度")]
    private float speedMove = 50.0f;
    [Header("揺れの振幅（小刻み）、頻度（速さ）")]
    [SerializeField] private float shakeAmplitude = 5.0f;
    [SerializeField] private float shakeFrequency = 5.0f;

    private float distanceCurrent = 0f; // 現在の移動した距離
    private float distanceTotal;        // 開始から終了地点までの距離
    private float timeShake;            // 揺れの時間を管理する

    void Start()
    {
        // 初期位置の設定
        transform.localPosition = positionStart;
    }

    void Update()
    {
        // 移動した距離を更新
        distanceCurrent += speedMove * Time.deltaTime;

        // 目標地点の設定
        Vector3 positionTarget = new Vector3(positionStart.x + SpawnerPlay.GetCount * 100, positionStart.y);

        // 総距離を計算
        distanceTotal = Vector3.Distance(positionStart, positionTarget);

        // 距離が全体の距離を超えないようにクランプ
        distanceCurrent = Mathf.Clamp(distanceCurrent, 0f, distanceTotal);

        // 現在の位置を線形補間で計算
        float t = distanceCurrent / distanceTotal;
        Vector3 newPosition = Vector3.Lerp(positionStart, positionTarget, t);

        if (Vector3.Distance(transform.localPosition, positionTarget) > 0.1f)
        {
            // 小刻みに揺れる動きをPerlinNoiseで加える
            timeShake += Time.deltaTime * shakeFrequency;
            float shakeOffset = Mathf.PerlinNoise(timeShake, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude で揺れが-振幅~+振幅の範囲で小刻みに動く

            // 新しい位置に揺れのオフセットを加える
            newPosition.y += shakeOffset;
        }

        // 新しい位置を設定
        transform.localPosition = newPosition;
    }
}
