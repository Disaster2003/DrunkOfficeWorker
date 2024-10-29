using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    [SerializeField, Header("�L�[���͂̐������̃v���C���[������A�N�V����")]
    PlayerComponent.STATE_PLAYER state_player;
    private bool isSuccessInputKey; // true = �L�[���͐���, false = �L�[���͖�����

    // Start is called before the first frame update
    void Start()
    {
        isSuccessInputKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(state_player == PlayerComponent.STATE_PLAYER.JUMP)
        {
            if (isSuccessInputKey)
            {
                // �v���C���[�̉���ʉ߂�����
                transform.Translate(10 * Time.deltaTime, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state_player == PlayerComponent.STATE_PLAYER.TACKLE)
        {
            // ����ɂԂ���΂�
            Vector2 topLeft = Vector2.up + Vector2.left;
            GetComponent<Rigidbody2D>().AddForce(topLeft, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().AddTorque(360, ForceMode2D.Impulse);
        }
    }
}
