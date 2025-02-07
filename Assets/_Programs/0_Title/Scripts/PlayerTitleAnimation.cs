using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem; // 新Inputシステムの利用に必要
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

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
    private AsyncOperationHandle<SpriteAtlas> handle;
    private Sprite[] spritesAnimation;
    [Header("待機、驚愕、走行")]
    [SerializeField] private string[] namesSpriteWait;
    [SerializeField] private string[] namesSpriteRun;
    [SerializeField] private string[] namesSpriteAmazing;
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

        // スプライトシートをロード
        handle = Addressables.LoadAssetAsync<SpriteAtlas>("PlayerAnimation");

        LoadAnimationSprite(namesSpriteWait);

        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Decide.started += OnDecide; // アクションにイベントを登録
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimation();

        if (state_player != STATE_PLAYER.RUN) return;

        if (INTERVAL_AMAZING > 0)
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
    }

    private void OnDestroy()
    {
        // ロードした画像の解放
        Addressables.Release(handle);
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
        LoadAnimationSprite(namesSpriteAmazing);
    }

    /// <summary>
    /// プレイヤーのアニメーション処理
    /// </summary>
    private void PlayerAnimation()
    {
        if (spritesAnimation == null) return;

        if (fTimerAnimation < INTERVAL_ANIMATION)
        {
            // アニメーションのインターバル中
            fTimerAnimation += Time.deltaTime;
            return;
        }

        // インターバルの初期化
        fTimerAnimation = 0;

        for (int i = 0; i < spritesAnimation.Length; i++)
        {
            if (spriteRenderer.sprite == spritesAnimation[i])
            {
                if (i == spritesAnimation.Length - 1)
                {
                    if (state_player == STATE_PLAYER.AMAZING)
                    {
                        state_player = STATE_PLAYER.RUN;
                        LoadAnimationSprite(namesSpriteRun);
                        return;
                    }

                    // 最初の画像へ
                    spriteRenderer.sprite = spritesAnimation[0];
                    return;
                }
                else
                {
                    // 次の画像へ
                    spriteRenderer.sprite = spritesAnimation[i + 1];
                    return;
                }
            }
            else if (i == spritesAnimation.Length - 1)
            {
                // 画像を変更する
                spriteRenderer.sprite = spritesAnimation[0];
            }
        }
    }

    /// <summary>
    /// アニメーション画像をロードする
    /// </summary>
    /// <param name="_namesSprite">ロードするアニメーション画像名</param>
    private async void LoadAnimationSprite(string[] _namesSprite)
    {
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SpriteAtlas spriteAtlas = handle.Result;

            // スプライトを配列に格納
            spritesAnimation = new Sprite[_namesSprite.Length];
            for (int i = 0; i < _namesSprite.Length; i++)
            {
                spritesAnimation[i] = spriteAtlas.GetSprite(_namesSprite[i]);
                if (spritesAnimation[i] == null)
                {
                    Debug.LogError("Sprite not found: " + _namesSprite[i]);
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load Sprite Atlas: " + handle.OperationException.Message);
        }
    }
}
