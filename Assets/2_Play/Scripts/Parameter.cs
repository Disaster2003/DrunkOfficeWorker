using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : MonoBehaviour
{
    public Vector2 startPoint = new Vector2(0, 0); // �J�n�n�_�̍��W
    public Vector2 endPoint = new Vector2(10, 0);  // �I���n�_�̍��W
    public float moveSpeed = 1f; // �ړ����x

    [Header("�h��̕��A�p�x")]
    public float shakeAmplitude = 5.0f; // �h��̐U��
    public float shakeFrequency = 5.0f; // �h��̕p�x
    private float shakeTime = 0f; // �h��̎��Ԃ��Ǘ�����

    private float totalDistance; // �J�n����I���n�_�܂ł̋���
    private float currentDistance = 0f; // ���݂̈ړ���������

    void Start()
    {
        // ���������v�Z
        totalDistance = Vector3.Distance(startPoint, endPoint);
        transform.position = startPoint; // �����ʒu���J�n�n�_�ɐݒ�
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            // �ړ������������X�V
            currentDistance += moveSpeed * Time.deltaTime;

            // �������S�̂̋����𒴂��Ȃ��悤�ɃN�����v
            currentDistance = Mathf.Clamp(currentDistance, 0f, totalDistance);

            // ���݂̈ʒu����`��ԂŌv�Z
            float t = currentDistance / totalDistance;
            Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, t);

            // �����݂ɗh��铮����PerlinNoise�ŉ�����
            shakeTime += Time.deltaTime * shakeFrequency;
            float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude �ŗh�ꂪ-�U��~+�U���͈̔͂ŏ����݂ɓ���
            newPosition.y += shakeOffset;

            // �V�����ʒu�ɗh��̃I�t�Z�b�g��������
            newPosition.y += shakeOffset;

            // �V�����ʒu��ݒ�
            transform.position = newPosition;
        }
    }
}
