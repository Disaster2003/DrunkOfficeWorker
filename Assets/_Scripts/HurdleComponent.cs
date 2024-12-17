using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [SerializeField, Header("キー入力の成功時のプレイヤーがするアクション")]
    private PlayerComponent.STATE_PLAYER state_player;

    [SerializeField, Header("障害物画像")]
    private Sprite[] spritesHurdle;

    [SerializeField, Header("移動速度")]
    private float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // 初期配置
        transform.position = new Vector3(10, 0, 0);

        // 画像の設定
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spritesHurdle[Random.Range(0, spritesHurdle.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -10)
        {
            // 自身の破棄
            Destroy(gameObject);
        }
        else if (transform.childCount == 0)
        {
            if (state_player == PlayerComponent.STATE_PLAYER.JUMP)
            {
                // プレイヤーの下を通過させる
                transform.Translate(5 * speedMove * -Time.deltaTime, 0, 0);
            }
        }
        else if (transform.position.x > 5)
        {
            // カメラ内へ移動
            transform.Translate(speedMove * -Time.deltaTime, 0, 0);
        }
    }

    public PlayerComponent.STATE_PLAYER GetPlayerState() { return state_player; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        // nullチェック
        if(rb2D == null)
        {
            Debug.LogError("障害物のRigidbody2Dが取得できませんでした");
            return;
        }

        if (state_player == PlayerComponent.STATE_PLAYER.TACKLE)
        {
            // タックル終了
            collision.GetComponent<PlayerComponent>().SetPlayerState(PlayerComponent.STATE_PLAYER.WAIT);

            // 左上にぶっ飛ばす
            Vector2 topLeft = Vector2.up + Vector2.left;
            rb2D.AddForce(topLeft * 10, ForceMode2D.Impulse);
            rb2D.AddTorque(180, ForceMode2D.Impulse);
        }
    }
}
