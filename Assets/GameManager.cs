using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    /// <summary>
    /// �C���X�^���X���擾����
    /// </summary>
    public static GameManager GetInstance { get { return instance; } }

    public enum STATE_SCENE
    {
        TITLE = 0,    // �^�C�g�����
        TUTORIAL = 1, // �`���[�g���A�����
        NAMING = 2,   // �������
        PLAY = 3,     // �v���C���
        RESULT = 4,   // ���ʉ��
        OVER = 5,     // �Q�[���I�[�o�[
        NONE,         // �����Ȃ�
    }
    private STATE_SCENE state_scene;
    public enum STATE_LEVEL
    {
        NONE,   // �v���C��ʈȊO
        EASY,   // ����
        NORMAL, // ����
        HARD,   // �㋉
    }
    private STATE_LEVEL state_level;
    /// <summary>
    /// ��Փx���擾����
    /// </summary>
    public STATE_LEVEL GetLevelState {  get { return state_level; } }

    [SerializeField, Header("�t�F�[�h�C��/�A�E�g�p�摜")]
    private Image imgFade;
    private bool isChangingScene; // true = �V�[���J�ڂ���, false = �V�[���J�ڂ��Ȃ�

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // �C���X�^���X�̐���
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �P�ꉻ
            Destroy(gameObject);
        }

        // ��Ԃ̏�����
        state_scene = (STATE_SCENE)SceneManager.GetActiveScene().buildIndex;
        state_level = STATE_LEVEL.NONE;

        // �V�[���J�ڏ�Ԃ̏�����
        imgFade.fillAmount = 1;
        isChangingScene = false;

        // ���O�̏�����
        PlayerPrefs.SetString("PlayerName", "");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // null�`�F�b�N
        if (imgFade == null)
        {
            Debug.LogError("imgFade��inspector�Őݒ肵�Ă��܂���");
            return;
        }

        if (isChangingScene)
        {
            if (imgFade.fillAmount >= 1)
            {
                // ���̃V�[����
                isChangingScene = false;
                SceneManager.LoadSceneAsync((int)state_scene);
            }
            else
            {
                // �t�F�[�h�A�E�g
                imgFade.fillAmount += Time.deltaTime;
            }
        }
        else if(imgFade.fillAmount > 0)
        {
            // �t�F�[�h�C��
            imgFade.fillAmount += -Time.deltaTime;
        }
    }

    /// <summary>
    /// �Q�[�����I������
    /// </summary>
    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �Q�[���v���C�I��
#else
    Application.Quit(); // �Q�[���v���C�I��
#endif
    }

    /// <summary>
    /// ���̃V�[����ݒ肷��
    /// </summary>
    /// <param name="_state_scene">���̃V�[��</param>
    public void SetNextScene(STATE_SCENE _state_scene = STATE_SCENE.NONE, STATE_LEVEL _state_level = STATE_LEVEL.NONE)
    {
        if(isChangingScene) return;

        // �J�ڎ��̓��ʂȏ���
        switch (state_scene)
        {
            case STATE_SCENE.NAMING:
                _state_level = state_level;
                isChangingScene = true;
                break;
            case STATE_SCENE.PLAY:
                _state_level = state_level;
                break;
            case STATE_SCENE.RESULT:
                if(_state_scene == STATE_SCENE.TITLE)
                {
                    // ���O���Z�b�g
                    PlayerPrefs.SetString("PlayerName", "");
                }

                isChangingScene = true;
                break;
            case STATE_SCENE.OVER:
                isChangingScene = true;
                break;
        }

        // null�`�F�b�N
        if (_state_scene == STATE_SCENE.NONE)
        {
            Debug.LogError("�J�ڐ�̃V�[�������ݒ�ł�");
            return;
        }
        if (_state_scene == STATE_SCENE.PLAY && _state_level == STATE_LEVEL.NONE)
        {
            Debug.LogError("�J�ڐ�̓�Փx�����ݒ�ł�");
            return;
        }

        if (_state_scene == STATE_SCENE.PLAY)
        {
            if (state_scene != STATE_SCENE.NAMING)
            {
                if (PlayerPrefs.GetString("PlayerName") == "")
                {
                    // ���O���͉�ʂ�����
                    _state_scene = STATE_SCENE.NAMING;
                }
            }
        }

        // �J�ڐ�̌���
        state_scene = _state_scene;
        state_level = _state_level;
    }

    /// <summary>
    /// �V�[���̐؂�ւ����J�n����
    /// </summary>
    public void StartChangingScene() { isChangingScene = true; }

    /// <summary>
    /// SE��炷
    /// </summary>
    /// <param name="se">�炷SE</param>
    public void PlaySE(AudioClip se)
    {
        audioSource.PlayOneShot(se);
    }
}
