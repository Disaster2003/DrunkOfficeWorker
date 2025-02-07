using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomShakeCamera : MonoBehaviour
{
    [Header("�J�����ɃA�^�b�`���Ă�������")]
    public Camera mainCamera;
    public Transform targetObject;  // �Y�[���������^�[�Q�b�g�I�u�W�F�N�g
    public float zoomSpeed = 3.0f;   // �Y�[�����x
    public float moveSpeed = 2.0f;   // �J�����ړ����x
    [Header("FOV�̒l")]
    public float targetFOV = 60.0f;  // �Y�[�����FOV
    [Header("�J�����̈ʒu�ƃY�[�������̋��e�͈�")]
    public float threshold = 100.0f;   // �J�����̈ʒu�ƃY�[�������̋��e�͈�
    public float resetSpeed = 2.0f; // ���ɖ߂����x

    private float initialFOV;
    private Vector3 initialPosition;
    private bool isZoomingIn = true; // �Y�[���C�����Ă��邩�ǂ����̃t���O
    private bool isResetting = false; // ���ɖ߂��Ă��邩�ǂ����̃t���O

    public bool isAction = false;  //���̓��������邽�߂̃t���O

    bool isStayflg = false;

    [Header("�h��̋����A����")]
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
            isResetting = true; // �Y�[���C�������������烊�Z�b�g���J�n
            isStayflg = false;
        }
    }

    void TriggerShake()
    {
        if (isAction)
        {
            StartCoroutine(Shake(swayTime, swayPower)); // 0.5�b�ԁA�̋����ŗh�炷
        }
    }

    void ZoomInToTarget()
    {
        Vector3 targetPosition = targetObject.position;  // �^�[�Q�b�g�̍��W���擾

        if (targetPosition != null)
        {
            // �J�������Y�[����ɋ߂Â���
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                 targetPosition,
                Time.deltaTime * moveSpeed
            );

            // FOV�𒲐����ăY�[���C������
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize,
                targetFOV,
                Time.deltaTime * zoomSpeed
            );

            // �Y�[���������������ǂ����`�F�b�N
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < threshold &&
                Mathf.Abs(mainCamera.orthographicSize - targetFOV) < 0.1f)
            {
                // �Y�[��������̏�����ǉ��i�K�v�ɉ����āj
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
          isResetting = false; // ���Z�b�g������
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
