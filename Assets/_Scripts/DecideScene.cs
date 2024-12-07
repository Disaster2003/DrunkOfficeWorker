using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v
using UnityEngine.EventSystems;

public class DecideScene : MonoBehaviour
{
    [Header("�J�ڐ�̏�ԁE��Փx")]
    [SerializeField] private GameManager.STATE_SCENE state_scene;
    [SerializeField] private GameManager.STATE_LEVEL state_level;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    // Start is called before the first frame update
    void Start()
    {
        IC = new InputControl(); // �C���v�b�g�A�N�V�������擾
        IC.Player.Decide.started += OnDecide; // �A�N�V�����ɃC�x���g��o�^
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B
    }

    /// <summary>
    /// ����{�^���̉�������
    /// </summary>
    /// <param name="context">����{�^��</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;

        // �J�ڐ�̃V�[�������肷��
        GameManager.GetInstance().SetNextScene(state_scene, state_level);
    }

    private void OnDestroy()
    {
        // �C���v�b�g�A�N�V�����̒�~
        IC.Disable();
    }
}
