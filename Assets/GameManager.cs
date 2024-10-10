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
        TITLE,    // �^�C�g�����
        TUTORIAL, // �`���[�g���A�����
        EASY,     // ����
        NORMAL,   // ����
        HARD,     // �㋉
        RANKING,  // �����L���O
        OVER,     // �Q�[���I�[�o�[
        NONE,     // �����Ȃ�
    }
    private STATE_SCENE state_scene;

    [SerializeField] Image imgFade;
    private bool isChangingScene;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("GameManager��Start��������ǂݍ��܂�Ă��܂�");
            return;
        }

        // �^�C�g���X�^�[�g
        state_scene = STATE_SCENE.NONE;

        imgFade.fillAmount = 1; // �t�F�[�h�C��
        isChangingScene = false;
    }

    // Update is called once per frame
    void Update()
    {
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
    public void SetNextScene(STATE_SCENE _state_scene = STATE_SCENE.NONE)
    {
        // null�`�F�b�N
        if(STATE_SCENE.NONE == _state_scene )
        {
            return;
        }

        state_scene = _state_scene;
        isChangingScene = true;
    }
}
