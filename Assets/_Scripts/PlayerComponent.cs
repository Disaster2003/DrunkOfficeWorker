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
    private STATE_PLAYER state_player;

    /// <summary>
    /// プレイヤーの状態を設定する
    /// </summary>
    public STATE_PLAYER SetPlayerState { set { state_player = value; } }

    [SerializeField, Header("プレイヤーの特定位置")]
    private Vector3 positionStart;

    [SerializeField, Header("移動速度")]
    private float fSpeedMove;

    private SpriteRenderer spriteRenderer;
    [Header("プレイヤーのアニメーション画像配列")]
    [SerializeField] private Sprite[] spriteArrayPlayerWait;
    [SerializeField] private Sprite[] spriteArrayPlayerRun;
    [SerializeField] private Sprite[] spriteArrayPlayerTackle;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpUp;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpDown;
    private float fTimerAnimation;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION;

    private Rigidbody2D rb2D;
    private bool isJumped; // true = ジャンプ中, false = not ジャンプ

    [SerializeField, Header("背景コンポーネント")]
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
        // 状態の初期化
        state_player = STATE_PLAYER.WAIT;

        // 開始位置へ
        transform.position = positionStart;

        // コンポーネントの取得
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
                // 待機/走行アニメーション
                float distance = Vector2.Distance(transform.position, positionStart);
                PlayerAnimation(distance < 0.1f ? spriteArrayPlayerWait : spriteArrayPlayerRun);

                switch (state_arrive)
                {
                    case STATE_ARRIVE.MOVE:
                        if (Vector2.Distance(transform.position, positionStart) > 0.1f)
                        {
                            // 開始位置へ向かう
                            transform.position = Vector2.MoveTowards(transform.position, positionStart, fSpeedMove * Time.deltaTime);
                        }
                        // 帰宅
                        else if (background.GetIsFinished) state_arrive = STATE_ARRIVE.CENTER;
                        break;
                    case STATE_ARRIVE.CENTER:
                        if (Vector2.Distance(transform.position, new Vector2(0, -1)) < 0.1f)
                        {
                            state_arrive = STATE_ARRIVE.UP;
                        }
                        else
                        {
                            // 真ん中へ向かう
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), fSpeedMove * Time.deltaTime);
                        }
                        break;
                    case STATE_ARRIVE.UP:
                        // 真ん中上へ向かう
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 3), fSpeedMove * Time.deltaTime);
                        break;
                }
                break;
            case STATE_PLAYER.TACKLE:
                if (transform.position.x < 0)
                {
                    // 走行
                    transform.Translate(fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerRun);
                }
                else
                {
                    // タックル
                    transform.Translate(2 * fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerRun/*Tackle*/);
                }
                break;
            case STATE_PLAYER.JUMP:
                OnJumpAndLand();

                if (rb2D.velocity.y > 0)
                {
                    // 上昇アニメーション
                    PlayerAnimation(spriteArrayPlayerJumpUp);
                }
                else
                {
                    // 下降アニメーション
                    PlayerAnimation(spriteArrayPlayerJumpDown);
                }
                break;
        }
    }

    /// <summary>
    /// プレイヤーのアニメーション処理
    /// </summary>
    /// <param name="sprites">アニメーションする画像配列</param>
    private void PlayerAnimation(Sprite[] sprites)
    {
        if(sprites == null) return;

        if (fTimerAnimation >= INTERVAL_ANIMATION)
        {
            // インターバルの初期化
            fTimerAnimation = 0;

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
            fTimerAnimation += Time.deltaTime;
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
            rb2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rb2D.gravityScale = 1;
        }
        else if (transform.position.y < positionStart.y)
        {
            // 着地
            isJumped = false;
            state_player = STATE_PLAYER.WAIT;
            transform.position = new Vector3(transform.position.x, positionStart.y);
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 0;
        }
    }
}
