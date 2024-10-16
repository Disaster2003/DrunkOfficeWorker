using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v

public class DecideScene : MonoBehaviour
{
    [SerializeField] Image imgFade; // �t�F�[�h�p�摜

    [SerializeField] GameManager.STATE_SCENE state_scene;
    [SerializeField] GameManager.STATE_LEVEL state_level;

    [SerializeField] GameObject upButton;   // ����͂������̐؂�ւ���{�^��
    [SerializeField] GameObject downButton; // �����͂������̐؂�ւ���{�^��
    [SerializeField] GameObject leftButton; // �����͂������̐؂�ւ���{�^��
    [SerializeField] GameObject rightButton;// �E���͂������̐؂�ւ���{�^��
    public bool isSelecting; // true = �I�΂�Ă���Afalse = �I�΂�Ă��Ȃ�

    private InputControl IC;           // �C���v�b�g�A�N�V�������`
    private Vector2 direction;         // �ړ�����
    public float intervalButtonChange; // �{�^���؂�ւ��̃C���^�[�o��

    [SerializeField] GameObject highlight; // �I�����̃n�C���C�g

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0; // �d��OFF

        if (state_scene == GameManager.STATE_SCENE.TITLE || state_scene == GameManager.STATE_SCENE.TUTORIAL)
        {
            // �I�����
            isSelecting = true;
        }
        else
        {
            // ��I�����
            isSelecting = false;
        }

        IC = new InputControl(); // �C���v�b�g�A�N�V�������擾
        IC.Player.Direction.performed += OnDirection; // �S�ẴA�N�V�����ɃC�x���g��o�^
        IC.Player.Decide.started += OnDecide;
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B
        direction = Vector2.zero; // �ړ������̏�����
        intervalButtonChange = 0; // �C���^�[�o���̏�����
    }

    // Update is called once per frame
    void Update()
    {
        if(imgFade.fillAmount == 0)
        {
            // �d��ON
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        if (isSelecting)
        {
            if (Time.time % 1 <= 0.5f)
            {
                // �n�C���C�gON
                highlight.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                // �n�C���C�gOFF
                highlight.GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
        else
        {
            // ��I�����́A�n�C���C�gOFF
            highlight.GetComponent<SpriteRenderer>().color = Color.clear;
        }

        if(intervalButtonChange > 0)
        {
            // �{�^���؂�ւ��̃C���^�[�o����
            intervalButtonChange += -Time.deltaTime;
        }
    }

    private void OnDirection(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;

        // null�`�F�b�N
        direction = context.ReadValue<Vector2>();
        if (direction == Vector2.zero) return;

        if (intervalButtonChange <= 0)
        {
            if (direction.x > 0)
            {
                // �E�̃{�^���ɐ؂�ւ�
                ChangeSelectButton(rightButton);
                return;
            }
            if (direction.x < 0)
            {
                // ���̃{�^���ɐ؂�ւ�
                ChangeSelectButton(leftButton);
                return;
            }
            if (direction.y > 0)
            {
                // ��̃{�^���ɐ؂�ւ�
                ChangeSelectButton(upButton);
                return;
            }
            if (direction.y < 0)
            {
                // ���̃{�^���ɐ؂�ւ�
                ChangeSelectButton(downButton);
                return;
            }
        }
    }

    /// <summary>
    /// �{�^���I����؂�ւ���
    /// </summary>
    /// <param name="kindButton">�{�^���̎��</param>
    private void ChangeSelectButton(GameObject kindButton)
    {
        if (kindButton == null) return;
        isSelecting = false;
        kindButton.GetComponent<DecideScene>().isSelecting = true;
        kindButton.GetComponent<DecideScene>().intervalButtonChange = 0.1f;
    }

    /// <summary>
    /// �J�ڐ�̃V�[�������肷��
    /// </summary>
    /// <param name="context">�{�^������</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;
        GameManager.GetInstance().SetNextScene(state_scene, state_level);
    }
}
