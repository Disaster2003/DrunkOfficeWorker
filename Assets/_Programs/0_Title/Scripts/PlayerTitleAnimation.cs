using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要

public class PlayerTitleAnimation : MonoBehaviour
{
    private enum STATE_PLAYER
    {
        WAIT,   // 待機
        AMAZING,// 驚き
        RUN,    // 走行
    }
    private STATE_PLAYER state_player;

    [SerializeField, Header("目標地点のx座標")]
    private float position_xGoal;

    private SpriteRenderer spriteRenderer;
    [Header("待機、驚愕、走行")]
    [SerializeField] private Sprite[] wait;
    [SerializeField] private Sprite[] run;
    [SerializeField] private Sprite[] amazing;
    private float fTimerAnimation;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION;
    [SerializeField, Header("驚きのインターバル")]
    private float INTERVAL_AMAZING;

    private InputControl IC; // インプットアクションを定義

    [SerializeField, Header("移動速度")]
    private float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        state_player = STATE_PLAYER.WAIT;

        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Decide.started += OnDecide; // アクションにイベントを登録
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
    }

    // Update is called once per frame
    void Update()
    {
        switch (state_player)
        {
            case STATE_PLAYER.WAIT:
                PlayerAnimation(wait);
                break;
            case STATE_PLAYER.AMAZING:
                PlayerAnimation(amazing);
                break;
            case STATE_PLAYER.RUN:
                if(INTERVAL_AMAZING > 0)
                {
                    INTERVAL_AMAZING -= Time.deltaTime;
                    return;
                }

                if (transform.position.x > position_xGoal)
                {
                    // シーン遷移の開始
                    GameManager.GetInstance.StartChangingScene();

                    // 自身の破棄
                    Destroy(gameObject);
                    return;
                }

                // 右移動
                transform.Translate(speedMove * Time.deltaTime, 0, 0);

                PlayerAnimation(run);
                break;
        }
    }

    /// <summary>
    /// 決定ボタンの押下処理
    /// </summary>
    /// <param name="context">決定ボタン</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (state_player != STATE_PLAYER.WAIT) return;

        // 移動開始
        state_player = STATE_PLAYER.AMAZING;
    }

    /// <summary>
    /// プレイヤーのアニメーション処理
    /// </summary>
    /// <param name="sprites">アニメーションする画像配列</param>
    private void PlayerAnimation(Sprite[] sprites)
    {
        if (sprites == null) return;

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
                        if (sprites == amazing)
                        {
                            state_player = STATE_PLAYER.RUN;
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
}
