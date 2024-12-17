using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public enum STATE_SCENE
    {
        TITLE = 0,    // �^�C�g�����
        TUTORIAL = 1, // �`���[�g���A�����
        NAMING = 2,   // �������
        PLAY = 3,     // �v���C���
        RANKING = 4,  // �����L���O
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

    [SerializeField, Header("�t�F�[�h�C��/�A�E�g�p�摜")]
    private Image imgFade;
    private bool isChangingScene; // true = �V�[���J�ڂ���, false = �V�[���J�ڂ��Ȃ�

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
            Destroy(gameObject);
        }

        // ��Ԃ̏�����
        state_scene = (STATE_SCENE)SceneManager.GetActiveScene().buildIndex;
        state_level = STATE_LEVEL.NONE;

        // �V�[���J�ڏ�Ԃ̏�����
        imgFade.fillAmount = 1;
        isChangingScene = false;
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

                Debug.Log("�V�[����؂�ւ��܂���");
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
    /// �C���X�^���X���擾����
    /// </summary>
    public static GameManager GetInstance() { return instance; }

    /// <summary>
    /// ���̃V�[����ݒ肷��
    /// </summary>
    /// <param name="_state_scene">���̃V�[��</param>
    public void SetNextScene(STATE_SCENE _state_scene = STATE_SCENE.NONE, STATE_LEVEL _state_level = STATE_LEVEL.NONE)
    {
        if(isChangingScene) return;

        switch (state_scene)
        {
            case STATE_SCENE.NAMING:
            case STATE_SCENE.RANKING:
                _state_level = state_level;
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

        // �J�ڐ�̌���
        state_scene = _state_scene;
        state_level = _state_level;
    }

    /// <summary>
    /// ��Փx���擾����
    /// </summary>
    public STATE_LEVEL GetLevelState() { return state_level; }

    /// <summary>
    /// �V�[���̐؂�ւ����J�n����
    /// </summary>
    public void StartChangingScene() { isChangingScene = true; }
}
