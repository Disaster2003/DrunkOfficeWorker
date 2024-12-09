using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    public enum STATE_PLAYER
    {
        WAIT,   // 待機
        TACKLE, // タックル
        JUMP,   // ジャンプ
    }
    [SerializeField]
    private STATE_PLAYER state_player;

    [Header("プレイヤーの特定位置")]
    [SerializeField] private Vector2 positionStart;

    private SpriteRenderer spriteRenderer;
    [Header("プレイヤーのアニメーション画像配列")]
    [SerializeField] private Sprite[] spritesPlayerWait;
    [SerializeField] private Sprite[] spritesPlayerRun;
    [SerializeField] private Sprite[] spritesPlayerTackle;
    [SerializeField] private Sprite[] spritesPlayerJumpUp;
    [SerializeField] private Sprite[] spritesPlayerJumpDown;
    private float timerAnimation;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION;

    private Rigidbody2D rigidBody2D;
    private bool isJumped; // true = ジャンプ中, false = ジャンプ前後

    private GoalPerformance goalPerformance;

    // Start is called before the first frame update
    void Start()
    {
        // 状態の初期化
        state_player = STATE_PLAYER.WAIT;

        // 開始位置へ
        transform.position = positionStart;

        // 画像の初期化
        spriteRenderer = GetComponent<SpriteRenderer>();
        // spriteRenderer.sprite = spritesPlayerWait[0];
        timerAnimation = 0;

        // 回転不可
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
                // 待機/走行アニメーション
                float distance = Vector2.Distance(transform.position, positionStart);
                PlayerAnimation(distance < 0.1f ? spritesPlayerWait : spritesPlayerRun);

                if (goalPerformance.isArrived)
                {
                    // 真ん中へ向かう
                    transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, Time.deltaTime);
                }
                else
                {
                    // 開始位置へ向かう
                    transform.position = Vector2.MoveTowards(transform.position, positionStart, Time.deltaTime);
                }
                break;
            case STATE_PLAYER.TACKLE:
                if (transform.position.x < 0)
                {
                    // 走行
                    transform.Translate(5 * Time.deltaTime, 0, 0);
                    PlayerAnimation(spritesPlayerRun);
                }
                else
                {
                    // タックル
                    transform.Translate(10 * Time.deltaTime, 0, 0);
                    PlayerAnimation(spritesPlayerTackle);
                }
                break;
            case STATE_PLAYER.JUMP:
                OnJumpAndLand();

                if (rigidBody2D.velocity.y > 0)
                {
                    // 上昇アニメーション
                    PlayerAnimation(spritesPlayerJumpUp);
                }
                else
                {
                    // 下降アニメーション
                    PlayerAnimation(spritesPlayerJumpDown);
                }
                break;
        }
    }

    /// <summary>
    /// プレイヤーの状態を設定する
    /// </summary>
    /// <param name="_state_player">設定する状態</param>
    public void SetPlayerState(STATE_PLAYER _state_player)
    {
        state_player = _state_player;
    } 

    /// <summary>
    /// プレイヤーのアニメーション処理
    /// </summary>
    /// <param name="sprites">アニメーションする画像配列</param>
    private void PlayerAnimation(Sprite[] sprites)
    {
        if(sprites == null) return;

        if (timerAnimation >= INTERVAL_ANIMATION)
        {
            // インターバルの初期化
            timerAnimation = 0;

            for (int i = 0; i < sprites.Length; i++)
            {
                if (spriteRenderer.sprite == sprites[i])
                {
                    if (i == sprites.Length - 1)
                    {
                        // 最初の画像へ
                        spriteRenderer.sprite = sprites[0];
                        return;
                    }
                    else
                    {
                        // 次の画像へ
                        spriteRenderer.sprite = sprites[i + 1];
                        return;
                    }
                }
                else if (i == sprites.Length - 1)
                {
                    // 画像を変更する
                    spriteRenderer.sprite = sprites[0];
                }
            }
        }
        else
        {
            // アニメーションのインターバル中
            timerAnimation += Time.deltaTime;
        }
    }

    /// <summary>
    /// ジャンプ/着地処理
    /// </summary>
    private void OnJumpAndLand()
    {
        if (!isJumped)
        {
            // ジャンプ
            isJumped = true;
            rigidBody2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigidBody2D.gravityScale = 1;
        }
        else if (transform.position.y < positionStart.y)
        {
            // 着地
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
