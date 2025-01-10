using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v

public class PlayerTitleAnimation : MonoBehaviour
{
    [SerializeField, Header("�ڕW�n�_��x���W")]
    private float position_xGoal;
    private bool isMove; // true = �����ėǂ�, false = �������爫��

    [Header("�ҋ@�A�����A���s")]
    [SerializeField] private Sprite[] wait;
    [SerializeField] private Sprite[] amazing;
    [SerializeField] private Sprite[] run;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    [SerializeField, Header("�ړ����x")]
    private float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;

        IC = new InputControl(); // �C���v�b�g�A�N�V�������擾
        IC.Player.Decide.started += OnDecide; // �A�N�V�����ɃC�x���g��o�^
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove) return;

        if (transform.position.x > position_xGoal)
        {
            // �V�[���J�ڂ̊J�n
            GameManager.GetInstance.StartChangingScene();

            // ���g�̔j��
            Destroy(gameObject);
            return;
        }

        // �E�ړ�
        transform.Translate(speedMove * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// ����{�^���̉�������
    /// </summary>
    /// <param name="context">����{�^��</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (isMove) return;

        // �ړ��J�n
        isMove = true;
    }
}
