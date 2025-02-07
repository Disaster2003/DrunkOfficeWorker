using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField, Header("�����ʒu�̍��W")]
    private Vector2 positionStart = Vector2.zero;

    [SerializeField, Header("�ړ����x")]
    private float fSpeedMove = 50.0f;
    [Header("�h��̐U���i�����݁j�A�p�x�i�����j")]
    [SerializeField] private float fShakeAmplitude = 5.0f;
    [SerializeField] private float fShakeFrequency = 5.0f;

    private float fDistanceCurrent;
    private float fDistanceTotal;
    private float fTimerShake;

    void Start()
    {
        // �����ʒu�̐ݒ�
        transform.localPosition = positionStart;

        fDistanceCurrent = 0;
        fDistanceTotal = 0;
        fTimerShake = 0;
    }

    void Update()
    {
        // �ړ����������̍X�V
        fDistanceCurrent += fSpeedMove * Time.deltaTime;

        // �ڕW�n�_�̐ݒ�
        Vector2 positionTarget = new Vector2(positionStart.x + SpawnerPlay.GetSpawnCount * 100, positionStart.y);

        // ���������v�Z
        fDistanceTotal = Vector2.Distance(positionStart, positionTarget);

        // �������S�̂̋����𒴂��Ȃ��悤�ɃN�����v
        fDistanceCurrent = Mathf.Clamp(fDistanceCurrent, 0f, fDistanceTotal);

        // ���݂̈ʒu����`��ԂŌv�Z
        float t = fDistanceCurrent / fDistanceTotal;
        Vector2 positionNew = Vector2.Lerp(positionStart, positionTarget, t);

        if (Vector2.Distance(transform.localPosition, positionTarget) > 0.1f)
        {
            // �����݂ɗh��铮����PerlinNoise�ŉ�����
            fTimerShake += Time.deltaTime * fShakeFrequency;
            float shakeOffset = Mathf.PerlinNoise(fTimerShake, 0) * fShakeAmplitude * 2f - fShakeAmplitude; // -shakeAmplitude �ŗh�ꂪ-�U��~+�U���͈̔͂ŏ����݂ɓ���

            // �V�����ʒu�ɗh��̃I�t�Z�b�g��������
            positionNew.y += shakeOffset;
        }

        // �V�����ʒu��ݒ�
        transform.localPosition = positionNew;
    }
}
