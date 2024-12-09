using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField, Header("�����ʒu�̍��W")]
    private Vector3 positionStart = Vector3.zero;
    [SerializeField, Header("�ړ����x")]
    private float speedMove = 50.0f;
    [Header("�h��̐U���i�����݁j�A�p�x�i�����j")]
    [SerializeField] private float shakeAmplitude = 5.0f;
    [SerializeField] private float shakeFrequency = 5.0f;

    private float distanceCurrent = 0f; // ���݂̈ړ���������
    private float distanceTotal;        // �J�n����I���n�_�܂ł̋���
    private float timeShake;            // �h��̎��Ԃ��Ǘ�����

    void Start()
    {
        // �����ʒu�̐ݒ�
        transform.localPosition = positionStart;
    }

    void Update()
    {
        // �ړ������������X�V
        distanceCurrent += speedMove * Time.deltaTime;

        // �ڕW�n�_�̐ݒ�
        Vector3 positionTarget = new Vector3(positionStart.x + SpawnerPlay.GetCount * 100, positionStart.y);

        // ���������v�Z
        distanceTotal = Vector3.Distance(positionStart, positionTarget);

        // �������S�̂̋����𒴂��Ȃ��悤�ɃN�����v
        distanceCurrent = Mathf.Clamp(distanceCurrent, 0f, distanceTotal);

        // ���݂̈ʒu����`��ԂŌv�Z
        float t = distanceCurrent / distanceTotal;
        Vector3 newPosition = Vector3.Lerp(positionStart, positionTarget, t);

        if (Vector3.Distance(transform.localPosition, positionTarget) > 0.1f)
        {
            // �����݂ɗh��铮����PerlinNoise�ŉ�����
            timeShake += Time.deltaTime * shakeFrequency;
            float shakeOffset = Mathf.PerlinNoise(timeShake, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude �ŗh�ꂪ-�U��~+�U���͈̔͂ŏ����݂ɓ���

            // �V�����ʒu�ɗh��̃I�t�Z�b�g��������
            newPosition.y += shakeOffset;
        }

        // �V�����ʒu��ݒ�
        transform.localPosition = newPosition;
    }
}
