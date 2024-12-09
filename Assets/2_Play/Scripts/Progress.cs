using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    public Vector3 startPoint = new Vector3(0, 0, 0); // �J�n�n�_�̍��W
    public Vector3 endPoint = new Vector3(10, 0, 0);  // �I���n�_�̍��W
    public float moveSpeed = 1f; // �ړ����x
    [Header("�h��̕��A�p�x")]
    public float shakeAmplitude = 0.1f; // �h��̐U���i�����݁j
    public float shakeFrequency = 10f; // �h��̕p�x�i�����j

    private float totalDistance; // �J�n����I���n�_�܂ł̋���
    private float currentDistance = 0f; // ���݂̈ړ���������
    private float shakeTime = 0f; // �h��̎��Ԃ��Ǘ�����

    void Start()
    {
        // ���������v�Z
        totalDistance = Vector3.Distance(startPoint, endPoint);
        transform.position = startPoint; // �����ʒu���J�n�n�_�ɐݒ�
    }

    void Update()
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

        // �V�����ʒu�ɗh��̃I�t�Z�b�g��������
        newPosition.y += shakeOffset;

        // �V�����ʒu��ݒ�
        transform.position = newPosition;
    }
}
