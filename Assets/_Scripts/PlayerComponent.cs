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
    [SerializeField] private Sprite[] spriteArrayPlayerJumpUp;
    [SerializeField] private Sprite[] spriteArrayPlayerJumpDown;
    [SerializeField] private Sprite[] spriteArrayPlayerTackle;
    private float fTimerAnimation;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION;

    private Rigidbody2D rb2D;
    private bool isJumped; // true = ジャンプ中, false = not ジャンプ

    [SerializeField, Header("背景コンポーネント")]
    private BackgroundScroll background;
    private enum STATE_ARRIVE
    {
        NONE,   // 何もなし
        MOVE,   // 動作中
        CENTER, // 駅の入り口
        ENTER,  // 入っていく
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
        spriteRenderer.sprite = spriteArrayPlayerWait[0];
        fTimerAnimation = 0;

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.freezeRotation = true;
        isJumped = false;

        state_arrive = STATE_ARRIVE.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state_player)
        {
            case STATE_PLAYER.WAIT:
                switch (state_arrive)
                {
                    case STATE_ARRIVE.NONE:
                        if (background.GetIsFinished)
                        {
                            // 帰宅開始
                            state_arrive = STATE_ARRIVE.CENTER;
                            return;
                        }
                        if (!background.GetIsStopped)
                        {
                            // 動作開始
                            state_arrive = STATE_ARRIVE.MOVE;
                            return;
                        }

                        PlayerAnimation(spriteArrayPlayerWait);
                        break;
                    case STATE_ARRIVE.MOVE:
                        if (background.GetIsStopped)
                        {
                            if (Vector2.Distance(transform.position, positionStart) < 0.1f)
                            {
                                // 動作終了
                                state_arrive = STATE_ARRIVE.NONE;
                                return;
                            }
                        }

                        // 開始位置へ向かう
                        transform.position = Vector2.MoveTowards(transform.position, positionStart, fSpeedMove * Time.deltaTime);

                        PlayerAnimation(spriteArrayPlayerRun);
                        break;
                    case STATE_ARRIVE.CENTER:
                        if (Vector2.Distance(transform.position, new Vector2(0, -1)) < 0.1f)
                        {
                            state_arrive = STATE_ARRIVE.ENTER;
                        }
                        else
                        {
                            // 真ん中へ向かう
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), fSpeedMove * Time.deltaTime);

                            PlayerAnimation(spriteArrayPlayerRun);
                        }
                        break;
                    case STATE_ARRIVE.ENTER:
                        // 真ん中上へ向かう
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 3), fSpeedMove * Time.deltaTime);

                        PlayerAnimation(spriteArrayPlayerWait);
                        break;
                }
                break;
            case STATE_PLAYER.TACKLE:
                if (transform.position.x < -4)
                {
                    // 走行
                    transform.Translate(fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerRun);
                }
                else
                {
                    // タックル
                    transform.Translate(2 * fSpeedMove * Time.deltaTime, 0, 0);
                    PlayerAnimation(spriteArrayPlayerTackle);
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
                        if(sprites == spriteArrayPlayerJumpUp || sprites == spriteArrayPlayerJumpDown)
                        {
                            return;
                        }

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
            state_arrive = STATE_ARRIVE.MOVE;
            transform.position = new Vector3(transform.position.x, positionStart.y);
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 0;
        }
    }
}
