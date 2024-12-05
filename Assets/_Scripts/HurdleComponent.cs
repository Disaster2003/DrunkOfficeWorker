using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    [SerializeField, Header("�L�[���͂̐������̃v���C���[������A�N�V����")]
    private PlayerComponent.STATE_PLAYER state_player;

    [SerializeField, Header("��Q���摜")]
    private Sprite[] spritesHurdle;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(10, 0, 0);

        // �摜�̐ݒ�
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spritesHurdle[Random.Range(0, spritesHurdle.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -10)
        {
            // ���g�̔j��
            Destroy(gameObject);
            return;
        }

        if (transform.childCount == 0)
        {
            if (state_player == PlayerComponent.STATE_PLAYER.JUMP)
            {
                // �v���C���[�̉���ʉ߂�����
                transform.Translate(10 * -Time.deltaTime, 0, 0);
            }
        }
        else if (transform.position.x > 5)
        {
            // �J�������ֈړ�
            transform.Translate(-Time.deltaTime, 0, 0);
        }
    }

    public PlayerComponent.STATE_PLAYER GetPlayerState() {  return state_player; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        if (state_player == PlayerComponent.STATE_PLAYER.TACKLE)
        {
            // �^�b�N���I��
            collision.GetComponent<PlayerComponent>().SetPlayerState(PlayerComponent.STATE_PLAYER.WAIT);

            // ����ɂԂ���΂�
            Vector2 topLeft = Vector2.up + Vector2.left;
            GetComponent<Rigidbody2D>().AddForce(topLeft * 10, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().AddTorque(180, ForceMode2D.Impulse);
        }
    }
}
