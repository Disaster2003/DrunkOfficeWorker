using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField, Header("�{�^���摜")]
    private Sprite[] UI_Button;
    [SerializeField, Header("���摜")]
    private Sprite[] UI_Arrow;

    [SerializeField, Header("�������E�A�ŃQ�[�W")]
    private Image imgInputGauge;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    [SerializeField, Header("�O�̃{�^��")]
    private GameObject imgBeforeButton;

    private enum KIND_BUTTON
    {
        NONE,               // ���I��
        PUSH,               // ����
        PUSH_LONG,          // ������
        PUSH_REPEAT_THREE,  // 3�A��
        PUSH_REPEAT_FIVE,   // 5�A��
    }
    [SerializeField, Header("�`���[�g���A���p�{�^���A�N�V�����̑I��")]
    private KIND_BUTTON kind_button;

    private float timerPushLong;
    private bool isPushed; // true = ������,false = not ����
    private int countPush;

    private static Dictionary<KIND_BUTTON, int> numberGenerate = new Dictionary<KIND_BUTTON, int>();

    // Start is called before the first frame update
    void Start()
    {
        // �{�^���̒��I
        int rand = Random.Range(0, 4);

        // �摜�̏�����
        if (Gamepad.all.Count > 0)
        {
            GetComponent<Image>().sprite = UI_Button[rand];
        }
        else
        {
            GetComponent<Image>().sprite = UI_Arrow[rand];
        }
        imgInputGauge.fillAmount = 0;

        // �C���v�b�g�A�N�V�������擾
        IC = new InputControl();
        // �A�N�V�����ɃC�x���g��o�^
        switch (rand)
        {
            case 0:
                IC.Player.UpKey.started += InputButton;
                IC.Player.UpKey.canceled += ReleaseButton;
                break;
            case 1:
                IC.Player.DownKey.started += InputButton;
                IC.Player.DownKey.canceled += ReleaseButton;
                break;
            case 2:
                IC.Player.LeftKey.started += InputButton;
                IC.Player.LeftKey.canceled += ReleaseButton;
                break;
            case 3:
                IC.Player.RightKey.started += InputButton;
                IC.Player.RightKey.canceled += ReleaseButton;
                break;
        }
        // �C���v�b�g�A�N�V�����̗L����
        IC.Enable();

        // ������Ԃ̏�����
        timerPushLong = 0;
        countPush = 0;
        isPushed = false;

        if (kind_button != KIND_BUTTON.NONE) return;

        if (numberGenerate.Count == 0)
        {
            // csv�̓ǂݍ���
            TextAsset csvFile = Resources.Load("level_adjust") as TextAsset; // Resources�ɂ���CSV�t�@�C�����i�[
            StringReader reader = new StringReader(csvFile.text); // TextAsset��StringReader�ɕϊ�
            List<string[]> csvData = new List<string[]>(); // CSV�t�@�C���̒��g�����郊�X�g
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine(); // 1�s���ǂݍ���
                csvData.Add(line.Split(',')); // csvData���X�g�ɒǉ�����
            }

            // �f�[�^���
            int levelIndex = (int)GameManager.GetInstance().GetLevelState();
            numberGenerate[KIND_BUTTON.PUSH] = int.Parse(csvData[levelIndex][2]);
            numberGenerate[KIND_BUTTON.PUSH_LONG] = int.Parse(csvData[levelIndex][3]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_THREE] = int.Parse(csvData[levelIndex][4]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_FIVE] = int.Parse(csvData[levelIndex][5]);
        }

        // �{�^���A�N�V�����̑I��
        do
        {
            rand = Random.Range(1, 4);
            if (numberGenerate[(KIND_BUTTON)rand] > 0)
            {
                numberGenerate[(KIND_BUTTON)rand]--;
                break;
            }
        }
        while (true);
        kind_button = (KIND_BUTTON)rand;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPushed) return;

        if (timerPushLong > 0.5f)
        {
            // ���g�̔j��
            Destroy(gameObject);
        }

        // �{�^�����������͌o�ߎ��Ԃ����Z
        timerPushLong += Time.deltaTime;
        imgInputGauge.fillAmount = timerPushLong / 0.5f;
    }

    private void OnDestroy()
    {
        // �C���v�b�g�A�N�V�����̖�����
        IC.Disable();
        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void InputButton(InputAction.CallbackContext context)
    {
        if (imgBeforeButton != null) return;

        // null�`�F�b�N
        if (kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("�{�^���A�N�V���������I���ł�");
            return;
        }

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                // ���g�̔j��
                Destroy(gameObject);
                break;
            case KIND_BUTTON.PUSH_LONG:
                isPushed = true;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                countPush++;
                break;
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    private void ReleaseButton(InputAction.CallbackContext context)
    {
        switch (kind_button)
        {
            case KIND_BUTTON.PUSH_LONG:
                // �{�^���������Ă��Ȃ��ƌo�ߎ��Ԃ̓[��
                imgInputGauge.fillAmount = 0;
                timerPushLong = 0;
                isPushed = false;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
                if(countPush >= 3)
                {
                    // ���g�̔j��
                    Destroy(gameObject);
                }
                else
                {
                    imgInputGauge.fillAmount = countPush / 3.0f;
                }
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                if (countPush >= 5)
                {
                    // ���g�̔j��
                    Destroy(gameObject);
                }
                else
                {
                    imgInputGauge.fillAmount = countPush / 5.0f;
                }
                break;
        }
    }
}
