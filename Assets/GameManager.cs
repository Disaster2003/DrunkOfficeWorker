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
        PLAY = 2,     // �v���C���
        RANKING = 3,  // �����L���O
        OVER = 4,     // �Q�[���I�[�o�[
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
            Debug.LogError("GameManager��Start��������ǂݍ��܂�Ă��܂�");
            return;
        }

        // ��Ԃ̏�����
        state_scene = STATE_SCENE.NONE;
        state_level = STATE_LEVEL.NONE;

        imgFade.fillAmount = 1; // �t�F�[�h�C��
        isChangingScene = false;// �V�[���J�ڏ�Ԃ̏�����
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
            // �t�F�[�h�A�E�g
            imgFade.fillAmount += Time.deltaTime;
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
        // null�`�F�b�N
        if (_state_scene == STATE_SCENE.NONE) return;
        if (_state_scene == STATE_SCENE.PLAY && _state_level == STATE_LEVEL.NONE) return;

        // �J�ڐ�̌���
        state_scene = _state_scene;
        isChangingScene = true;
    }
}
