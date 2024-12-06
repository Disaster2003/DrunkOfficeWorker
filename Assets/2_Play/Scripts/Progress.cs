using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] Vector3 position_Goal;   // �ڕW�n�_��x���W

    [Header("�h��̕��A�p�x")]
    private float shakeAmplitude = 5.0f; // �h��̐U��
    private float shakeFrequency = 5.0f; // �h��̕p�x
    private float shakeTime = 0f; // �h��̎��Ԃ��Ǘ�����

    void Update()
    {
        if (position_Goal == Vector3.zero)
        {
            return;
        }

        //// �����݂ɗh��铮����PerlinNoise�ŉ�����
        //shakeTime += Time.deltaTime * shakeFrequency;
        //float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude �ŗh�ꂪ-�U��~+�U���͈̔͂ŏ����݂ɓ���
        //transform.position += new Vector3(0, shakeOffset);

        // �V�����ʒu�ɗh��̃I�t�Z�b�g��������
        Vector3 positionTarget = new Vector3(-position_Goal.x + SpawnerPlay.GetCount * 100, transform.localPosition.y/* + shakeOffset*/);

        // ���݂̈ʒu����`��ԂŌv�Z
        transform.localPosition = Vector3.Lerp(transform.localPosition, positionTarget, Time.deltaTime);
    }
}
