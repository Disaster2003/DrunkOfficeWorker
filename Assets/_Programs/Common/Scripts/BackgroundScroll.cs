using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("目標地点")]
    private Vector2 positionGoal = Vector2.zero;

    private bool isStopped;
    /// <summary>
    /// 背景のスクロール停止を取得する
    /// </summary>
    public bool GetIsStopped { get { return isStopped; } }

    private bool isFinished;
    /// <summary>
    /// 背景のスクロール終了を取得する
    /// </summary>
    public bool GetIsFinished { get { return isFinished; } }

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererChild;
    private AsyncOperationHandle<SpriteAtlas> handle;
    private Sprite[] spritesBackground;
    [SerializeField] private string[] namesNomiyagai;
    [SerializeField] private string[] nameStation;
    [SerializeField] private GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        // 初期配置
        transform.position = Vector3.zero;

        isStopped = false;
        isFinished = false;

        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = transform.GetChild(0).GetComponent<SpriteRenderer>();

        // スプライトシートをロード
        handle = Addressables.LoadAssetAsync<SpriteAtlas>("Background");

        LoadBackgroundSprite(namesNomiyagai);
    }

    // Update is called once per frame
    void Update()
    {
        // nullチェック
        if(spriteRenderer is null || spriteRendererChild is null)
        {
            Debug.LogError("背景のSpriteRendererが未取得です");
            return;
        }

        if (isFinished)
        {
            isStopped = true;
            return;
        }

        // 障害物が定位置でないならスクロールし続ける
        GameObject hurdle = GameObject.FindGameObjectWithTag("Hurdle");
        if (hurdle != null)
        {
            if (Mathf.Abs(hurdle.transform.position.x) < 0.2f)
            {
                isStopped = true;
                return;
            }
        }

        // 背景のスクロール
        isStopped = false;
        transform.Translate(5 * -Time.deltaTime, 0, 0);

        if (transform.position.x <= positionGoal.x)
        {
            if (spritesBackground.Length == nameStation.Length)
            {
                // 役割終了
                isFinished = true;
                return;
            }
            else if (spawner == null)
            {
                // 駅到着
                spriteRenderer.sprite = spriteRendererChild.sprite;

                LoadBackgroundSprite(nameStation);
                spriteRendererChild.sprite = spritesBackground[0];
            }
            else
            {
                // 街中
                spriteRenderer.sprite = spriteRendererChild.sprite;
                spriteRendererChild.sprite = spritesBackground[Random.Range(0, namesNomiyagai.Length)];
            }

            // 初期位置へ
            transform.position = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        // ロードした画像の解放
        Addressables.Release(handle);
    }

    /// <summary>
    /// アニメーション画像をロードする
    /// </summary>
    /// <param name="_namesSprite">ロードするアニメーション画像名</param>
    private async void LoadBackgroundSprite(string[] _namesSprite)
    {
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SpriteAtlas spriteAtlas = handle.Result;

            // スプライトを配列に格納
            spritesBackground = new Sprite[_namesSprite.Length];
            for (int i = 0; i < _namesSprite.Length; i++)
            {
                spritesBackground[i] = spriteAtlas.GetSprite(_namesSprite[i]);
                if (spritesBackground[i] == null)
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
