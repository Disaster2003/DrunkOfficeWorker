using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField, Header("初期位置の座標")]
    private Vector2 positionStart = Vector2.zero;

    [SerializeField, Header("移動速度")]
    private float fSpeedMove = 50.0f;
    [Header("揺れの振幅（小刻み）、頻度（速さ）")]
    [SerializeField] private float fShakeAmplitude = 5.0f;
    [SerializeField] private float fShakeFrequency = 5.0f;

    private float fDistanceCurrent;
    private float fDistanceTotal;
    private float fTimerShake;

    void Start()
    {
        // 初期位置の設定
        transform.localPosition = positionStart;

        fDistanceCurrent = 0;
        fDistanceTotal = 0;
        fTimerShake = 0;
    }

    void Update()
    {
        // 移動した距離の更新
        fDistanceCurrent += fSpeedMove * Time.deltaTime;

        // 目標地点の設定
        Vector2 positionTarget = new Vector2(positionStart.x + SpawnerPlay.GetSpawnCount * 100, positionStart.y);

        // 総距離を計算
        fDistanceTotal = Vector2.Distance(positionStart, positionTarget);

        // 距離が全体の距離を超えないようにクランプ
        fDistanceCurrent = Mathf.Clamp(fDistanceCurrent, 0f, fDistanceTotal);

        // 現在の位置を線形補間で計算
        float t = fDistanceCurrent / fDistanceTotal;
        Vector2 positionNew = Vector2.Lerp(positionStart, positionTarget, t);

        if (Vector2.Distance(transform.localPosition, positionTarget) > 0.1f)
        {
            // 小刻みに揺れる動きをPerlinNoiseで加える
            fTimerShake += Time.deltaTime * fShakeFrequency;
            float shakeOffset = Mathf.PerlinNoise(fTimerShake, 0) * fShakeAmplitude * 2f - fShakeAmplitude; // -shakeAmplitude で揺れが-振幅~+振幅の範囲で小刻みに動く

            // 新しい位置に揺れのオフセットを加える
            positionNew.y += shakeOffset;
        }

        // 新しい位置を設定
        transform.localPosition = positionNew;
    }
}
