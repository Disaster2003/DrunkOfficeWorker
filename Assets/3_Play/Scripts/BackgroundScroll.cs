using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("目標地点")]
    private Vector2 positionGoal = Vector2.zero;

    private bool isFinished;
    /// <summary>
    /// 背景のスクロール終了を取得する
    /// </summary>
    public bool GetIsFinished { get { return isFinished; } }

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererChild;
    [SerializeField] private Sprite[] nomiyagai;
    [SerializeField] private Sprite station;
    [SerializeField] private GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        // 初期配置
        transform.position = Vector3.zero;

        isFinished = false;

        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = transform.GetChild(0).GetComponent<SpriteRenderer>();
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

        if (GameObject.FindGameObjectWithTag("Hurdle") || isFinished) return;
       
        if (transform.position.x <= positionGoal.x)
        {
            if (spriteRendererChild.sprite == station)
            {
                // 役割終了
                isFinished = true;
                return;
            }
            else if (spawner == null)
            {
                // 駅到着
                spriteRenderer.sprite = spriteRendererChild.sprite;
                spriteRendererChild.sprite = station;
            }
            else
            {
                // 街中
                spriteRenderer.sprite = spriteRendererChild.sprite;
                spriteRendererChild.sprite = nomiyagai[Random.Range(0, nomiyagai.Length)];
            }

            // 初期位置へ
            transform.position = Vector3.zero;
        }
        else
        {
            // 背景のスクロール
            transform.Translate(10 * -Time.deltaTime, 0, 0);
        }
    }
}
