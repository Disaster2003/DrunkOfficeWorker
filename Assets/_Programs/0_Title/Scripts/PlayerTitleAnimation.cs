using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class PlayerTitleAnimation : MonoBehaviour
{
    private enum STATE_PLAYER
    {
        WAIT,   // �ҋ@
        AMAZING,// ����
        RUN,    // ���s
    }
    private STATE_PLAYER state_player;

    [SerializeField, Header("�ڕW�n�_��x���W")]
    private float position_xGoal;

    private SpriteRenderer spriteRenderer;
    private AsyncOperationHandle<SpriteAtlas> handle;
    private Sprite[] spritesAnimation;
    [Header("�ҋ@�A�����A���s")]
    [SerializeField] private string[] namesSpriteWait;
    [SerializeField] private string[] namesSpriteRun;
    [SerializeField] private string[] namesSpriteAmazing;
    private float fTimerAnimation;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION;
    [SerializeField, Header("�����̃C���^�[�o��")]
    private float INTERVAL_AMAZING;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    [SerializeField, Header("�ړ����x")]
    private float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        state_player = STATE_PLAYER.WAIT;

        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �X�v���C�g�V�[�g�����[�h
        handle = Addressables.LoadAssetAsync<SpriteAtlas>("PlayerAnimation");

        LoadAnimationSprite(namesSpriteWait);

        IC = new InputControl(); // �C���v�b�g�A�N�V�������擾
        IC.Player.Decide.started += OnDecide; // �A�N�V�����ɃC�x���g��o�^
        IC.Enable(); // �C���v�b�g�A�N�V�������@�\������ׂɗL��������B
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
            // �V�[���J�ڂ̊J�n
            GameManager.GetInstance.StartChangingScene();

            // ���g�̔j��
            Destroy(gameObject);
            return;
        }

        // �E�ړ�
        transform.Translate(speedMove * Time.deltaTime, 0, 0);
    }

    private void OnDestroy()
    {
        // ���[�h�����摜�̉��
        Addressables.Release(handle);
    }

    /// <summary>
    /// ����{�^���̉�������
    /// </summary>
    /// <param name="context">����{�^��</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (state_player != STATE_PLAYER.WAIT) return;

        // �ړ��J�n
        state_player = STATE_PLAYER.AMAZING;
        LoadAnimationSprite(namesSpriteAmazing);
    }

    /// <summary>
    /// �v���C���[�̃A�j���[�V��������
    /// </summary>
    private void PlayerAnimation()
    {
        if (spritesAnimation == null) return;

        if (fTimerAnimation < INTERVAL_ANIMATION)
        {
            // �A�j���[�V�����̃C���^�[�o����
            fTimerAnimation += Time.deltaTime;
            return;
        }

        // �C���^�[�o���̏�����
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

                    // �ŏ��̉摜��
                    spriteRenderer.sprite = spritesAnimation[0];
                    return;
                }
                else
                {
                    // ���̉摜��
                    spriteRenderer.sprite = spritesAnimation[i + 1];
                    return;
                }
            }
            else if (i == spritesAnimation.Length - 1)
            {
                // �摜��ύX����
                spriteRenderer.sprite = spritesAnimation[0];
            }
        }
    }

    /// <summary>
    /// �A�j���[�V�����摜�����[�h����
    /// </summary>
    /// <param name="_namesSprite">���[�h����A�j���[�V�����摜��</param>
    private async void LoadAnimationSprite(string[] _namesSprite)
    {
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SpriteAtlas spriteAtlas = handle.Result;

            // �X�v���C�g��z��Ɋi�[
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
