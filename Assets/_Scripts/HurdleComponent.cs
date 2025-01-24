using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleComponent : MonoBehaviour
{
    [SerializeField, Header("初期位置")]
    private Vector2 positionInitialize;

    private Rigidbody2D rb2D;

    [SerializeField, Header("キー入力成功時、プレイヤーがするアクション")]
    private PlayerComponent.STATE_PLAYER state_player;

    /// <summary>
    /// アクションの状態を取得する
    /// </summary>
    public PlayerComponent.STATE_PLAYER GetPlayerState { get { return state_player; } }

    private SpriteRenderer spriteRenderer;
    private Sprite[] spritesHurdle;
    private float intervalAnimation;

    [Header("障害物画像")]
    [SerializeField] private Sprite[] beercase;
    [SerializeField] private Sprite[] rubbish;

    [SerializeField] private Sprite[] ojisan;

    // Start is called before the first frame update
    void Start()
    {
        // 初期配置
        transform.position = positionInitialize;

        // コンポーネントの取得
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 画像の設定
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

        // アニメーション間隔の初期化
        intervalAnimation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 10)
        {
            // 自身の破棄
            Destroy(gameObject);
        }
        else if (transform.position.x > 5)
        {
            // カメラ内へ移動
            transform.Translate(SpeedAdjust.GetCurrentSpeed * -Time.deltaTime, 0, 0);
        }

        if (state_player == PlayerComponent.STATE_PLAYER.JUMP)
        {
            if (transform.childCount == 0)
            {
                // プレイヤーの下を通過させる
                transform.Translate(5 * SpeedAdjust.GetCurrentSpeed * -Time.deltaTime, 0, 0);
            }
            else Animation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        // nullチェック
        if(rb2D == null)
        {
            Debug.LogError("障害物のRigidbody2Dが未取得です");
            return;
        }

        if (state_player == PlayerComponent.STATE_PLAYER.TACKLE)
        {
            spriteRenderer.sprite = spritesHurdle[1];

            // タックル終了
            collision.GetComponent<PlayerComponent>().SetPlayerState = PlayerComponent.STATE_PLAYER.WAIT;

            // 左上にぶっ飛ばす
            Vector2 topLeft = Vector2.up + Vector2.left;
            rb2D.AddForce(topLeft * 10, ForceMode2D.Impulse);
            rb2D.AddTorque(180, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// アニメーション処理
    /// </summary>
    private void Animation()
    {
        if (intervalAnimation < 0.2f)
        {
            // アニメーションのインターバル中
            intervalAnimation += Time.deltaTime;
            return;
        }

        // アニメーション
        intervalAnimation = 0;
        for (int i = 0; i < spritesHurdle.Length; i++)
        {
            if (spriteRenderer.sprite == spritesHurdle[i])
            {
                if (i == spritesHurdle.Length - 1)
                {
                    // 最初の画像に戻す
                    spriteRenderer.sprite = spritesHurdle[0];
                    return;
                }
                else
                {
                    // 次の画像へ
                    spriteRenderer.sprite = spritesHurdle[i + 1];
                    return;
                }
            }
        }
    }

}
