using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    [SerializeField, Header("キー入力の成功時のプレイヤーがするアクション")]
    PlayerComponent.STATE_PLAYER state_player;

    [SerializeField, Header("障害物画像")]
    private Sprite[] spritesHurdle;

    // Start is called before the first frame update
    void Start()
    {
        // 画像の設定
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spritesHurdle[Random.Range(0, spritesHurdle.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 5)
        {
            // カメラ内へ移動
            transform.Translate(-Time.deltaTime, 0, 0);
            return;
        }

        if(state_player == PlayerComponent.STATE_PLAYER.JUMP)
        {
            if (transform.childCount == 0)
            {
                // プレイヤーの下を通過させる
                transform.Translate(10 * -Time.deltaTime, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state_player == PlayerComponent.STATE_PLAYER.TACKLE)
        {
            // 左上にぶっ飛ばす
            Vector2 topLeft = Vector2.up + Vector2.left;
            GetComponent<Rigidbody2D>().AddForce(topLeft, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().AddTorque(360, ForceMode2D.Impulse);
        }
    }
}
