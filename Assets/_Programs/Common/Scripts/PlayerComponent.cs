using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    public enum STATE_PLAYER
    {
        WAIT,   // �ҋ@
        TACKLE, // �^�b�N��
        JUMP,   // �W�����v
    }
    private STATE_PLAYER state_player;

    /// <summary>
    /// �v���C���[�̏�Ԃ�ݒ肷��
    /// </summary>
    public STATE_PLAYER SetPlayerState { set { state_player = value; } }

    [SerializeField, Header("�v���C���[�̓���ʒu")]
    private Vector3 positionStart;

    [SerializeField, Header("�ړ����x")]
    private float fSpeedMove;

    private SpriteRenderer spriteRenderer;
    [Header("�v���C���[�̃A�j���[�V�����摜�z��")]
    [SerializeField] private Sprite[] spriteArrayPlayerWait;
    [SerializeField] private Sprite[] spriteArrayPlayerRun;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpUp;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpDown;
    [SerializeField] private Sprite[] spriteArrayPlayerTackle;
    private float fTimerAnimation;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION;

    private Rigidbody2D rb2D;
    private bool isJumped; // true = �W�����v��, false = not �W�����v

    [SerializeField, Header("�w�i�R���|�[�l���g")]
    private BackgroundScroll background;
    private enum STATE_ARRIVE
    {
        NONE,   // �����Ȃ�
        MOVE,   // ���쒆
        CENTER, // �w�̓����
        ENTER,  // �����Ă���
    }
    private STATE_ARRIVE state_arrive;

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ̏�����
        state_player = STATE_PLAYER.WAIT;

        // �J�n�ʒu��
        transform.position = positionStart;

        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteArrayPlayerWait[0];
        fTimerAnimation = 0;

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.freezeRotation = true;
        isJumped = false;

        state_arrive = STATE_ARRIVE.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state_player)
        {
            case STATE_PLAYER.WAIT:
                switch (state_arrive)
                {
                    case STATE_ARRIVE.NONE:
                        if (background.GetIsFinished)
                        {
                            // �A��J�n
                            state_arrive = STATE_ARRIVE.CENTER;
                            return;
                        }
                        if (!background.GetIsStopped)
                        {
                            // ����J�n
                            state_arrive = STATE_ARRIVE.MOVE;
                            return;
                        }

                        PlayerAnimation(spriteArrayPlayerWait);
                        break;
                    case STATE_ARRIVE.MOVE:
                        if (background.GetIsStopped)
                        {
                            if (Vector2.Distance(transform.position, positionStart) < 0.1f)
                            {
                                // ����I��
                                state_arrive = STATE_ARRIVE.NONE;
                                return;
                            }
                        }

                        // �J�n�ʒu�֌�����
                        transform.position = Vector2.MoveTowards(transform.position, positionStart, fSpeedMove * Time.deltaTime);

                        PlayerAnimation(spriteArrayPlayerRun);
                        break;
                    case STATE_ARRIVE.CENTER:
                        if (Vector2.Distance(transform.position, new Vector2(0, -1)) < 0.1f)
                        {
                            state_arrive = STATE_ARRIVE.ENTER;
                        }
                        else
                        {
                            // �^�񒆂֌�����
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), fSpeedMove * Time.deltaTime);

                            PlayerAnimation(spriteArrayPlayerRun);
                        }
                        break;
                    case STATE_ARRIVE.ENTER:
                        // �^�񒆏�֌�����
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 3), fSpeedMove * Time.deltaTime);

                        PlayerAnimation(spriteArrayPlayerWait);
                        break;
                }
                break;
            case STATE_PLAYER.TACKLE:
                if (transform.position.x < -4)
                {
                    // ���s
                    transform.Translate(fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerRun);
                }
                else
                {
                    // �^�b�N��
                    transform.Translate(2 * fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerTackle);
                }
                break;
            case STATE_PLAYER.JUMP:
                OnJumpAndLand();

                if (rb2D.velocity.y > 0)
                {
                    // �㏸�A�j���[�V����
                    PlayerAnimation(spriteArrayPlayerJumpUp);
                }
                else
                {
                    // ���~�A�j���[�V����
                    PlayerAnimation(spriteArrayPlayerJumpDown);
                }
                break;
        }
    }

    /// <summary>
    /// �v���C���[�̃A�j���[�V��������
    /// </summary>
    /// <param name="sprites">�A�j���[�V��������摜�z��</param>
    private void PlayerAnimation(Sprite[] sprites)
    {
        if(sprites == null) return;

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
                        if(sprites == spriteArrayPlayerJumpUp || sprites == spriteArrayPlayerJumpDown)
                        {
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

    /// <summary>
    /// �W�����v/���n����
    /// </summary>
    private void OnJumpAndLand()
    {
        if (!isJumped)
        {
            // �W�����v
            isJumped = true;
            rb2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rb2D.gravityScale = 1;
        }
        else if (transform.position.y < positionStart.y)
        {
            // ���n
            isJumped = false;
            state_player = STATE_PLAYER.WAIT;
            state_arrive = STATE_ARRIVE.MOVE;
            transform.position = new Vector3(transform.position.x, positionStart.y);
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 0;
        }
    }
}
