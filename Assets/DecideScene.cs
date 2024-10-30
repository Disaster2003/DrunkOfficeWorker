using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v

public class DecideScene : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�C��/�A�E�g�p�摜")]
    private Image imgFade;

    [Header("�J�ڐ�̏�ԁE��Փx")]
    [SerializeField] private GameManager.STATE_SCENE state_scene;
    [SerializeField] private GameManager.STATE_LEVEL state_level;

    [Header("�㉺���E���͂������̐؂�ւ���{�^��")]
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    public bool isSelecting; // true = �I�΂�Ă���Afalse = �I�΂�Ă��Ȃ�

    private InputControl IC; // �C���v�b�g�A�N�V�������`
    private Vector2 directionMove;
    public float intervalButtonChange;

    [SerializeField, Header("�{�^���I�����̃n�C���C�g")]
    private GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        // �d��OFF
        GetComponent<Rigidbody2D>().gravityScale = 0;

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
        IC.Player.Direction.performed += OnReserveDirection; // �S�ẴA�N�V�����ɃC�x���g��o�^
        IC.Player.Decide.started += OnDecide;
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B


        directionMove = Vector2.zero; // �ړ������̏�����
        intervalButtonChange = 0;     // �{�^���؂�ւ��Ԋu�̏�����
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

    /// <summary>
    /// ���͕������󂯎��
    /// </summary>
    private void OnReserveDirection(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;

        // null�`�F�b�N
        directionMove = context.ReadValue<Vector2>();
        if (directionMove == Vector2.zero) return;

        if (intervalButtonChange <= 0)
        {
            if (directionMove.x > 0)
            {
                // �E�̃{�^���ɐ؂�ւ�
                SwitchSelectButton(rightButton);
                return;
            }
            if (directionMove.x < 0)
            {
                // ���̃{�^���ɐ؂�ւ�
                SwitchSelectButton(leftButton);
                return;
            }
            if (directionMove.y > 0)
            {
                // ��̃{�^���ɐ؂�ւ�
                SwitchSelectButton(upButton);
                return;
            }
            if (directionMove.y < 0)
            {
                // ���̃{�^���ɐ؂�ւ�
                SwitchSelectButton(downButton);
                return;
            }
        }
    }

    /// <summary>
    /// �{�^���I����؂�ւ���
    /// </summary>
    /// <param name="kindButton">�{�^���̎��</param>
    private void SwitchSelectButton(GameObject kindButton)
    {
        if (kindButton == null) return;

        // �I���̐؂�ւ�
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
