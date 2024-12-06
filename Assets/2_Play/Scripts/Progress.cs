using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] Vector3 position_Goal;   // 目標地点のx座標

    [Header("揺れの幅、頻度")]
    private float shakeAmplitude = 5.0f; // 揺れの振幅
    private float shakeFrequency = 5.0f; // 揺れの頻度
    private float shakeTime = 0f; // 揺れの時間を管理する

    void Update()
    {
        // nullチェック
        if (position_Goal == Vector3.zero)
        {
            Debug.LogWarning("進捗の目標地点が未設定です");
            return;
        }

        // 現在の位置を線形補間で計算
        Vector3 positionTarget = new Vector3(-position_Goal.x + SpawnerPlay.GetCount * 100, transform.localPosition.y);
        Vector3 positionNew = Vector3.Lerp(transform.localPosition, positionTarget, Time.deltaTime);

        //if (Vector3.Distance(transform.localPosition, positionTarget) < 0.1f)
        //{
        //    // 小刻みに揺れる動きをPerlinNoiseで加える
        //    shakeTime += Time.deltaTime * shakeFrequency;
        //    float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude で揺れが-振幅~+振幅の範囲で小刻みに動く
        //    positionNew.y += shakeOffset;

        //    // 新しい位置に揺れのオフセットを加える
        //    positionNew.y += shakeOffset;
        //}

        // 結果を代入
        transform.localPosition = positionNew;
    }
}
