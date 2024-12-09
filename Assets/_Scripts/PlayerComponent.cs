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
    [SerializeField]
    private STATE_PLAYER state_player;

    [Header("�v���C���[�̓���ʒu")]
    [SerializeField] private Vector2 positionStart;

    private SpriteRenderer spriteRenderer;
    [Header("�v���C���[�̃A�j���[�V�����摜�z��")]
    [SerializeField] private Sprite[] spritesPlayerWait;
    [SerializeField] private Sprite[] spritesPlayerRun;
    [SerializeField] private Sprite[] spritesPlayerTackle;
    [SerializeField] private Sprite[] spritesPlayerJumpUp;
    [SerializeField] private Sprite[] spritesPlayerJumpDown;
    private float timerAnimation;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION;

    private Rigidbody2D rigidBody2D;
    private bool isJumped; // true = �W�����v��, false = �W�����v�O��

    private GoalPerformance goalPerformance;

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ̏�����
        state_player = STATE_PLAYER.WAIT;

        // �J�n�ʒu��
        transform.position = positionStart;

        // �摜�̏�����
        spriteRenderer = GetComponent<SpriteRenderer>();
        // spriteRenderer.sprite = spritesPlayerWait[0];
        timerAnimation = 0;

        // ��]�s��
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.freezeRotation = true;
        isJumped = false;

        goalPerformance = GetComponent<GoalPerformance>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state_player)
        {
            case STATE_PLAYER.WAIT:
                // �ҋ@/���s�A�j���[�V����
                float distance = Vector2.Distance(transform.position, positionStart);
                PlayerAnimation(distance < 0.1f ? spritesPlayerWait : spritesPlayerRun);

                if (goalPerformance.isArrived)
                {
                    // �^�񒆂֌�����
                    transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, Time.deltaTime);
                }
                else
                {
                    // �J�n�ʒu�֌�����
                    transform.position = Vector2.MoveTowards(transform.position, positionStart, Time.deltaTime);
                }
                break;
            case STATE_PLAYER.TACKLE:
                if (transform.position.x < 0)
                {
                    // ���s
                    transform.Translate(5 * Time.deltaTime, 0, 0);
                    PlayerAnimation(spritesPlayerRun);
                }
                else
                {
                    // �^�b�N��
                    transform.Translate(10 * Time.deltaTime, 0, 0);
                    PlayerAnimation(spritesPlayerTackle);
                }
                break;
            case STATE_PLAYER.JUMP:
                OnJumpAndLand();

                if (rigidBody2D.velocity.y > 0)
                {
                    // �㏸�A�j���[�V����
                    PlayerAnimation(spritesPlayerJumpUp);
                }
                else
                {
                    // ���~�A�j���[�V����
                    PlayerAnimation(spritesPlayerJumpDown);
                }
                break;
        }
    }

    /// <summary>
    /// �v���C���[�̏�Ԃ�ݒ肷��
    /// </summary>
    /// <param name="_state_player">�ݒ肷����</param>
    public void SetPlayerState(STATE_PLAYER _state_player)
    {
        state_player = _state_player;
    } 

    /// <summary>
    /// �v���C���[�̃A�j���[�V��������
    /// </summary>
    /// <param name="sprites">�A�j���[�V��������摜�z��</param>
    private void PlayerAnimation(Sprite[] sprites)
    {
        if(sprites == null) return;

        if (timerAnimation >= INTERVAL_ANIMATION)
        {
            // �C���^�[�o���̏�����
            timerAnimation = 0;

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
            timerAnimation += Time.deltaTime;
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
            rigidBody2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigidBody2D.gravityScale = 1;
        }
        else if (transform.position.y < positionStart.y)
        {
            // ���n
            isJumped = false;
            state_player = STATE_PLAYER.WAIT;
            transform.position = new Vector3(transform.position.x, positionStart.y);
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.gravityScale = 0;
        }
    }

    private void OnDestroy()
    {
        GameManager.GetInstance().SetNextScene(GameManager.STATE_SCENE.RANKING);
        GameManager.GetInstance().StartChangingScene();
    }
}
