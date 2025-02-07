using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v

public class PlayerTitleAnimation : MonoBehaviour
{
    private enum STATE_PLAYER
    {
        WAIT,   // �ҋ@
        AMAZING,// ����
        RUN,    // ���s
    }
    private STATE_PLAYER state_player;

    [SerializeField, Header("�ڕW�n�_��x���W")]
    private float position_xGoal;

    private SpriteRenderer spriteRenderer;
    [Header("�ҋ@�A�����A���s")]
    [SerializeField] private Sprite[] wait;
    [SerializeField] private Sprite[] run;
    [SerializeField] private Sprite[] amazing;
    private float fTimerAnimation;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION;
    [SerializeField, Header("�����̃C���^�[�o��")]
    private float INTERVAL_AMAZING;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    [SerializeField, Header("�ړ����x")]
    private float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        state_player = STATE_PLAYER.WAIT;

        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        IC = new InputControl(); // �C���v�b�g�A�N�V�������擾
        IC.Player.Decide.started += OnDecide; // �A�N�V�����ɃC�x���g��o�^
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B
    }

    // Update is called once per frame
    void Update()
    {
        switch (state_player)
        {
            case STATE_PLAYER.WAIT:
                PlayerAnimation(wait);
                break;
            case STATE_PLAYER.AMAZING:
                PlayerAnimation(amazing);
                break;
            case STATE_PLAYER.RUN:
                if(INTERVAL_AMAZING > 0)
                {
                    INTERVAL_AMAZING -= Time.deltaTime;
                    return;
                }

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

                PlayerAnimation(run);
                break;
        }
    }

    /// <summary>
    /// ����{�^���̉�������
    /// </summary>
    /// <param name="context">����{�^��</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (state_player != STATE_PLAYER.WAIT) return;

        // �ړ��J�n
        state_player = STATE_PLAYER.AMAZING;
    }

    /// <summary>
    /// �v���C���[�̃A�j���[�V��������
    /// </summary>
    /// <param name="sprites">�A�j���[�V��������摜�z��</param>
    private void PlayerAnimation(Sprite[] sprites)
    {
        if (sprites == null) return;

        if (fTimerAnimation >= INTERVAL_ANIMATION)
        {
            // �C���^�[�o���̏�����
            fTimerAnimation = 0;

            for (int i = 0; i < sprites.Length; i++)
            {
                if (spriteRenderer.sprite == sprites[i])
                {
                    if (i == sprites.Length - 1)
                    {
                        if (sprites == amazing)
                        {
                            state_player = STATE_PLAYER.RUN;
                            return;
                        }

                        // �ŏ��̉摜��
                        spriteRenderer.sprite = sprites[0];
                        return;
                    }
                    else
                    {
                        // ���̉摜��
                        spriteRenderer.sprite = sprites[i + 1];
                        return;
                    }
                }
                else if (i == sprites.Length - 1)
                {
                    // �摜��ύX����
                    spriteRenderer.sprite = sprites[0];
                }
            }
        }
        else
        {
            // �A�j���[�V�����̃C���^�[�o����
            fTimerAnimation += Time.deltaTime;
        }
    }
}
