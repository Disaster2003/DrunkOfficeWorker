using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomShakeCamera : MonoBehaviour
{
    [Header("カメラにアタッチしてください")]
    public Camera mainCamera;
    public Transform targetObject;  // ズームしたいターゲットオブジェクト
    public float zoomSpeed = 3.0f;   // ズーム速度
    public float moveSpeed = 2.0f;   // カメラ移動速度
    [Header("FOVの値")]
    public float targetFOV = 60.0f;  // ズーム後のFOV
    [Header("カメラの位置とズーム完了の許容範囲")]
    public float threshold = 100.0f;   // カメラの位置とズーム完了の許容範囲
    public float resetSpeed = 2.0f; // 元に戻す速度

    private float initialFOV;
    private Vector3 initialPosition;
    private bool isZoomingIn = true; // ズームインしているかどうかのフラグ
    private bool isResetting = false; // 元に戻しているかどうかのフラグ

    public bool isAction = false;  //この動きをするためのフラグ

    bool isStayflg = false;

    [Header("揺れの強さ、時間")]
    public float swayPower = 5.0f;
    public float swayTime = 3.0f;

    bool isAllActionflg = false;

    void Start()
    {
        initialFOV = mainCamera.orthographicSize;
        initialPosition = mainCamera.transform.position;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Transform transform = GetComponent<Transform>();
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
        isStayflg = true;
        if (isStayflg)
        {
            isResetting = true; // ズームインが完了したらリセットを開始
            isStayflg = false;
        }
    }

    void TriggerShake()
    {
        if (isAction)
        {
            StartCoroutine(Shake(swayTime, swayPower)); // 0.5秒間、の強さで揺らす
        }
    }

    void ZoomInToTarget()
    {
        Vector3 targetPosition = targetObject.position;  // ターゲットの座標を取得

        if (targetPosition != null)
        {
            // カメラをズーム先に近づける
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                 targetPosition,
                Time.deltaTime * moveSpeed
            );

            // FOVを調整してズームインする
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize,
                targetFOV,
                Time.deltaTime * zoomSpeed
            );

            // ズームが完了したかどうかチェック
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < threshold &&
                Mathf.Abs(mainCamera.orthographicSize - targetFOV) < 0.1f)
            {
                // ズーム完了後の処理を追加（必要に応じて）
                TriggerShake();
                isZoomingIn = false;
            }
        }
    }

    void ResetCamera()
    {
      mainCamera.transform.position = Vector3.Lerp(
         mainCamera.transform.position,
        initialPosition,
        Time.deltaTime * resetSpeed);

      mainCamera.orthographicSize = Mathf.Lerp(
          mainCamera.orthographicSize,
          initialFOV,
          Time.deltaTime * resetSpeed);

      if (Vector3.Distance(mainCamera.transform.position, initialPosition) < threshold &&
          Mathf.Abs(mainCamera.orthographicSize - initialFOV) < 0.1f)
      {
          isResetting = false; // リセットが完了
          isAction = false;
      }
    }

    void Update()
    {
        if (!isAction && Input.GetKeyDown(KeyCode.Space))
        {
            isAction = true;
            isZoomingIn = true;
        }
        if (isAction)
        {
            if (isZoomingIn)
            {
                ZoomInToTarget();
                isStayflg = false;
            }
            else if (isResetting)
            {
                ResetCamera();
            }
        }
    }
}
