using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPerformance : MonoBehaviour
{
    [SerializeField] private GameObject spawner;

    [SerializeField, Header("�k�����x")]
    private float fSpeedShrink = 0.5f;
    [SerializeField, Header("�ŏ��T�C�Y")]
    private Vector3 scaleMin = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField, Header("�k���̖ڕW�n�_")]
    private Vector3 positionGoal = Vector3.zero;
    [SerializeField, Header("�ړ����x")]
    private float fSpeedMove;

    public bool isArrived = false; // true = �k���n�_�ɓ���, false = �����B

    // Update is called once per frame
    void Update()
    {
        if (spawner) return;

        if (isArrived)
        {
            // ���݂̃X�P�[�����ŏ��X�P�[�����傫���ꍇ�̂ݏk��
            if (transform.localScale.x > scaleMin.x)
            {
                // �X�P�[�������X�ɏk��
                transform.localScale = Vector3.MoveTowards(transform.localScale, scaleMin, fSpeedShrink * Time.deltaTime);
            }
            else
            {
                // ���ʉ�ʂ�
                GameManager.GetInstance.SetNextScene(GameManager.STATE_SCENE.RESULT);
                GameManager.GetInstance.StartChangingScene();

                // ���g�̔j��
                Destroy(gameObject);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, positionGoal) < 0.1f)
            {
                // �ڕW�n�_�ɓ���
                isArrived = true;
                return;
            }

            // �ړ�
            transform.position = Vector3.MoveTowards(transform.position, positionGoal, fSpeedMove * Time.deltaTime);
        }
    }
}
