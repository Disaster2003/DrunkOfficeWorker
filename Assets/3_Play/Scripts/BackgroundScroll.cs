using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("�ڕW�n�_")]
    private Vector2 positionGoal = Vector2.zero;

    private bool isFinished;
    /// <summary>
    /// �w�i�̃X�N���[���I�����擾����
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
        // �����z�u
        transform.position = Vector3.zero;

        isFinished = false;

        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // null�`�F�b�N
        if(spriteRenderer is null || spriteRendererChild is null)
        {
            Debug.LogError("�w�i��SpriteRenderer�����擾�ł�");
            return;
        }

        if (GameObject.FindGameObjectWithTag("Hurdle") || isFinished) return;
       
        if (transform.position.x <= positionGoal.x)
        {
            if (spriteRendererChild.sprite == station)
            {
                // �����I��
                isFinished = true;
                return;
            }
            else if (spawner == null)
            {
                // �w����
                spriteRenderer.sprite = spriteRendererChild.sprite;
                spriteRendererChild.sprite = station;
            }
            else
            {
                // �X��
                spriteRenderer.sprite = spriteRendererChild.sprite;
                spriteRendererChild.sprite = nomiyagai[Random.Range(0, nomiyagai.Length)];
            }

            // �����ʒu��
            transform.position = Vector3.zero;
        }
        else
        {
            // �w�i�̃X�N���[��
            transform.Translate(10 * -Time.deltaTime, 0, 0);
        }
    }
}
