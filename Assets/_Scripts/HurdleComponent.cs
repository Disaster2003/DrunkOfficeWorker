using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [SerializeField, Header("�L�[���͐������A�v���C���[������A�N�V����")]
    private PlayerComponent.STATE_PLAYER state_player;

    /// <summary>
    /// �A�N�V�����̏�Ԃ��擾����
    /// </summary>
    public PlayerComponent.STATE_PLAYER GetPlayerState { get { return state_player; } }

    [SerializeField, Header("��Q���摜")]
    private Sprite[] spriteArrayHurdle;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        rb2D = GetComponent<Rigidbody2D>();

        // �����z�u
        transform.position = new Vector3(10, 0, 0);

        // �摜�̐ݒ�
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteArrayHurdle[Random.Range(0, spriteArrayHurdle.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -10)
        {
            // ���g�̔j��
            Destroy(gameObject);
        }
        else if (transform.childCount == 0)
        {
            if (state_player == PlayerComponent.STATE_PLAYER.JUMP)
            {
                // �v���C���[�̉���ʉ߂�����
                transform.Translate(5 * SpeedAdjust.GetCurrentSpeed * -Time.deltaTime, 0, 0);
            }
        }
        else if (transform.position.x > 5)
        {
            // �J�������ֈړ�
            transform.Translate(SpeedAdjust.GetCurrentSpeed * -Time.deltaTime, 0, 0);
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
            // �^�b�N���I��
            collision.GetComponent<PlayerComponent>().SetPlayerState = PlayerComponent.STATE_PLAYER.WAIT;

            // ����ɂԂ���΂�
            Vector2 topLeft = Vector2.up + Vector2.left;
            rb2D.AddForce(topLeft * 10, ForceMode2D.Impulse);
            rb2D.AddTorque(180, ForceMode2D.Impulse);
        }
    }
}
