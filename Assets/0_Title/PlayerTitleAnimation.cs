using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v

public class PlayerTitleAnimation : MonoBehaviour
{
    public float position_xGoal;
    private bool isMove = false;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    [SerializeField]private float speedMove;  //����

    // Start is called before the first frame update
    void Start()
    {
        IC = new InputControl(); // �C���v�b�g�A�N�V�������擾
        IC.Player.Decide.started += OnDecide; // �A�N�V�����ɃC�x���g��o�^
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (transform.position.x > position_xGoal)
            {
                // ���g�̔j��
                Destroy(gameObject);
                return;
            }

            // �ړ�
            transform.Translate(speedMove * Time.deltaTime, 0, 0);
        }
    }

    /// <summary>
    /// �ړ��J�n
    /// </summary>
    /// <param name="context">�{�^������</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        isMove = true;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance().StartChangingScene();
    }
}
