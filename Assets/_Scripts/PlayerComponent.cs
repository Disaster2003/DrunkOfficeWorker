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
    [SerializeField] private Sprite[] spriteArrayPlayerTackle;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpUp;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpDown;
    private float fTimerAnimation;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION;

    private Rigidbody2D rb2D;
    private bool isJumped; // true = �W�����v��, false = not �W�����v

    [SerializeField, Header("�w�i�R���|�[�l���g")]
    private BackgroundScroll background;
    private enum STATE_ARRIVE
    {
        MOVE,
        CENTER,
        UP,
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
        // spriteRenderer.sprite = spritesPlayerWait[0];
        fTimerAnimation = 0;

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.freezeRotation = true;
        isJumped = false;

        state_arrive = STATE_ARRIVE.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state_player)
        {
            case STATE_PLAYER.WAIT:
                // �ҋ@/���s�A�j���[�V����
                float distance = Vector2.Distance(transform.position, positionStart);
                PlayerAnimation(distance < 0.1f ? spriteArrayPlayerWait : spriteArrayPlayerRun);

                switch (state_arrive)
                {
                    case STATE_ARRIVE.MOVE:
                        if (Vector2.Distance(transform.position, positionStart) > 0.1f)
                        {
                            // �J�n�ʒu�֌�����
                            transform.position = Vector2.MoveTowards(transform.position, positionStart, fSpeedMove * Time.deltaTime);
                        }
                        // �A��
                        else if (background.GetIsFinished) state_arrive = STATE_ARRIVE.CENTER;
                        break;
                    case STATE_ARRIVE.CENTER:
                        if (Vector2.Distance(transform.position, new Vector2(0, -1)) < 0.1f)
                        {
                            state_arrive = STATE_ARRIVE.UP;
                        }
                        else
                        {
                            // �^�񒆂֌�����
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), fSpeedMove * Time.deltaTime);
                        }
                        break;
                    case STATE_ARRIVE.UP:
                        // �^�񒆏�֌�����
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 3), fSpeedMove * Time.deltaTime);
                        break;
                }
                break;
            case STATE_PLAYER.TACKLE:
                if (transform.position.x < 0)
                {
                    // ���s
                    transform.Translate(fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerRun);
                }
                else
                {
                    // �^�b�N��
                    transform.Translate(2 * fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerRun/*Tackle*/);
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
            transform.position = new Vector3(transform.position.x, positionStart.y);
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 0;
        }
    }
}
