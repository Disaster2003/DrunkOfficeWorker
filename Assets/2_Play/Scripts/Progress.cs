using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] Vector3 position_Goal;   // 目標地点のx座標

    [Header("揺れの幅、頻度")]
    public float shakeAmplitude = 5.0f; // 揺れの振幅
    public float shakeFrequency = 5.0f; // 揺れの頻度
    private float shakeTime = 0f; // 揺れの時間を管理する

    void Update()
    {
        if (position_Goal == Vector3.zero)
        {
            return;
        }

        Vector3 positionNew =
            Vector3.Lerp
            (
                transform.localPosition,
                new Vector3(-position_Goal.x + SpawnerPlay.GetCount * 100, transform.localPosition.y),
                Time.deltaTime
            );

        //// 小刻みに揺れる動きをPerlinNoiseで加える
        //shakeTime += Time.deltaTime * shakeFrequency;
        //float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude で揺れが-振幅~+振幅の範囲で小刻みに動く
        //positionNew.y += shakeOffset;

        //// 新しい位置に揺れのオフセットを加える
        //positionNew.y += shakeOffset;

        // 現在の位置を線形補間で計算
        transform.localPosition = positionNew;         
    }
}
