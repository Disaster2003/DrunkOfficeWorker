using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPerformance : MonoBehaviour
{
    [SerializeField] private GameObject spawner;

    // �k�����x
    public float scaleSpeed = 0.5f;

    [Header("�ŏI�T�C�Y�X�P�[��")]
    // �ŏ��X�P�[���T�C�Y
    public Vector3 minimumScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("�����̈ړ�����k���ړ��ɕς��ꏊ")]
    //�r���܂ōs�����W
    public Vector2 positionGoal;

    //�ŏ��̑���
    [SerializeField] private float speedMove;

    //�ړ�����k���ړ��ɕύX����t���O
    public bool isArrived = false;

    // Update is called once per frame
    void Update()
    {
        if (spawner) return;

        if (isArrived)
        {
            // ���݂̃X�P�[�����ŏ��X�P�[�����傫���ꍇ�̂ݏk��
            if (transform.localScale.x > minimumScale.x)
            {
                // �X�P�[�������X�ɏk��
                transform.localScale = Vector3.MoveTowards(transform.localScale, minimumScale, scaleSpeed * Time.deltaTime);
            }
            else
            {
                // ���ʉ�ʂ�
                GameManager.GetInstance().SetNextScene(GameManager.STATE_SCENE.RANKING);
                GameManager.GetInstance().StartChangingScene();

                // ���g�̔j��
                Destroy(gameObject);
            }
        }
        else
        {
            //���ݒn���擾
            Vector2 currentPosition = transform.position;

            if (Vector2.Distance(currentPosition, positionGoal) < 0.1f)
            {
                isArrived = true;
                return;
            }

            //�ړ�
            transform.position = Vector3.MoveTowards(currentPosition, positionGoal, speedMove * Time.deltaTime);
        }
    }
}
