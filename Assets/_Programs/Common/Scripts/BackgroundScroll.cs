using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("�ڕW�n�_")]
    private Vector2 positionGoal = Vector2.zero;

    private bool isStopped;
    /// <summary>
    /// �w�i�̃X�N���[����~���擾����
    /// </summary>
    public bool GetIsStopped { get { return isStopped; } }

    private bool isFinished;
    /// <summary>
    /// �w�i�̃X�N���[���I�����擾����
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
        // �����z�u
        transform.position = Vector3.zero;

        isStopped = false;
        isFinished = false;

        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = transform.GetChild(0).GetComponent<SpriteRenderer>();

        // �X�v���C�g�V�[�g�����[�h
        handle = Addressables.LoadAssetAsync<SpriteAtlas>("Background");

        LoadBackgroundSprite(namesNomiyagai);
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

        if (isFinished)
        {
            isStopped = true;
            return;
        }

        // ��Q������ʒu�łȂ��Ȃ�X�N���[����������
        GameObject hurdle = GameObject.FindGameObjectWithTag("Hurdle");
        if (hurdle != null)
        {
            if (Mathf.Abs(hurdle.transform.position.x) < 0.2f)
            {
                isStopped = true;
                return;
            }
        }

        // �w�i�̃X�N���[��
        isStopped = false;
        transform.Translate(5 * -Time.deltaTime, 0, 0);

        if (transform.position.x <= positionGoal.x)
        {
            if (spritesBackground.Length == nameStation.Length)
            {
                // �����I��
                isFinished = true;
                return;
            }
            else if (spawner == null)
            {
                // �w����
                spriteRenderer.sprite = spriteRendererChild.sprite;

                LoadBackgroundSprite(nameStation);
                spriteRendererChild.sprite = spritesBackground[0];
            }
            else
            {
                // �X��
                spriteRenderer.sprite = spriteRendererChild.sprite;
                spriteRendererChild.sprite = spritesBackground[Random.Range(0, namesNomiyagai.Length)];
            }

            // �����ʒu��
            transform.position = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        // ���[�h�����摜�̉��
        Addressables.Release(handle);
    }

    /// <summary>
    /// �A�j���[�V�����摜�����[�h����
    /// </summary>
    /// <param name="_namesSprite">���[�h����A�j���[�V�����摜��</param>
    private async void LoadBackgroundSprite(string[] _namesSprite)
    {
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SpriteAtlas spriteAtlas = handle.Result;

            // �X�v���C�g��z��Ɋi�[
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
