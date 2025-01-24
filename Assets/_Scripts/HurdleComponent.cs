using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    [SerializeField, Header("�����ʒu")]
    private Vector2 positionInitialize;

    private Rigidbody2D rb2D;

    [SerializeField, Header("�L�[���͐������A�v���C���[������A�N�V����")]
    private PlayerComponent.STATE_PLAYER state_player;

    /// <summary>
    /// �A�N�V�����̏�Ԃ��擾����
    /// </summary>
    public PlayerComponent.STATE_PLAYER GetPlayerState { get { return state_player; } }

    private SpriteRenderer spriteRenderer;
    private Sprite[] spritesHurdle;
    private float intervalAnimation;

    [Header("��Q���摜")]
    [SerializeField] private Sprite[] beercase;
    [SerializeField] private Sprite[] rubbish;

    [SerializeField] private Sprite[] ojisan;

    // Start is called before the first frame update
    void Start()
    {
        // �����z�u
        transform.position = positionInitialize;

        // �R���|�[�l���g�̎擾
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �摜�̐ݒ�
        List<Sprite[]> spritesList = new List<Sprite[]>();
        switch (state_player)
        {
            case PlayerComponent.STATE_PLAYER.TACKLE:
                spritesList.Add(beercase);
                spritesList.Add(rubbish);
                break;
            case PlayerComponent.STATE_PLAYER.JUMP:
                spritesList.Add(ojisan);
                break;
        }
        spritesHurdle = spritesList[Random.Range(0, spritesList.Count)];
        spriteRenderer.sprite = spritesHurdle[0];

        // �A�j���[�V�����Ԋu�̏�����
        intervalAnimation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 10)
        {
            // ���g�̔j��
            Destroy(gameObject);
        }
        else if (transform.position.x > 5)
        {
            // �J�������ֈړ�
            transform.Translate(SpeedAdjust.GetCurrentSpeed * -Time.deltaTime, 0, 0);
        }

        if (state_player == PlayerComponent.STATE_PLAYER.JUMP)
        {
            if (transform.childCount == 0)
            {
                // �v���C���[�̉���ʉ߂�����
                transform.Translate(5 * SpeedAdjust.GetCurrentSpeed * -Time.deltaTime, 0, 0);
            }
            else Animation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        // null�`�F�b�N
        if(rb2D == null)
        {
            Debug.LogError("��Q����Rigidbody2D�����擾�ł�");
            return;
        }

        if (state_player == PlayerComponent.STATE_PLAYER.TACKLE)
        {
            spriteRenderer.sprite = spritesHurdle[1];

            // �^�b�N���I��
            collision.GetComponent<PlayerComponent>().SetPlayerState = PlayerComponent.STATE_PLAYER.WAIT;

            // ����ɂԂ���΂�
            Vector2 topLeft = Vector2.up + Vector2.left;
            rb2D.AddForce(topLeft * 10, ForceMode2D.Impulse);
            rb2D.AddTorque(180, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// �A�j���[�V��������
    /// </summary>
    private void Animation()
    {
        if (intervalAnimation < 0.2f)
        {
            // �A�j���[�V�����̃C���^�[�o����
            intervalAnimation += Time.deltaTime;
            return;
        }

        // �A�j���[�V����
        intervalAnimation = 0;
        for (int i = 0; i < spritesHurdle.Length; i++)
        {
            if (spriteRenderer.sprite == spritesHurdle[i])
            {
                if (i == spritesHurdle.Length - 1)
                {
                    // �ŏ��̉摜�ɖ߂�
                    spriteRenderer.sprite = spritesHurdle[0];
                    return;
                }
                else
                {
                    // ���̉摜��
                    spriteRenderer.sprite = spritesHurdle[i + 1];
                    return;
                }
            }
        }
    }

}
