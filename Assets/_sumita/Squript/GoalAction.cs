using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAction : MonoBehaviour
{
    // �k�����x
    public float scaleSpeed = 0.5f;

    [Header("�ŏI�T�C�Y�X�P�[��")]
    // �ŏ��X�P�[���T�C�Y
    public Vector3 minimumScale = new Vector3(0.1f, 0.1f, 0.1f);

    // �����X�P�[��
    private Vector3 initialScale;

    [Header("�����̈ړ�����k���ړ��ɕς��ꏊ")]
    //�r���܂ōs�����W
    public Vector2 changePosition;

    [Header("�ŏI�I�ɍs�����W")]
    public Vector2 targetPosition;

    //�ŏ��̑���
    public float firstSpeed = 50.0f;

    //���̑���
    public float secondSpeed = 50.0f;

    //�S�[���ɂ����̂�
    public bool isGoalflg;

    //�ړ�����k���ړ��ɕύX����t���O
    bool isChangeMoveflg = false;

    void Start()
    {
        // �I�u�W�F�N�g�̏����X�P�[����ۑ�
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (isGoalflg)
        {
            //���ݒn���擾
            Vector2 currentPosition = transform.position;

            //�ړ�
            transform.position = Vector3.MoveTowards(currentPosition, changePosition, firstSpeed * Time.deltaTime);

            if (Vector2.Distance(currentPosition, changePosition) < 0.1f)
            {
                isChangeMoveflg = true;
                isGoalflg = false;
            }
        }
        if (isChangeMoveflg)
        {
            // ���݂̃X�P�[�����ŏ��X�P�[�����傫���ꍇ�̂ݏk��
            if (transform.localScale.x > minimumScale.x)
            {
                // �X�P�[�������X�ɏk��
                transform.localScale = Vector3.MoveTowards(transform.localScale, minimumScale, scaleSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
