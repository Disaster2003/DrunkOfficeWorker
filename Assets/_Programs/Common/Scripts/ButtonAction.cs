using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField, Header("�{�^���摜")]
    private Sprite[] spriteArrayXboxButton;
    [SerializeField, Header("���摜")]
    private Sprite[] spriteArrayArrow;

    private Image imgButton;
    [SerializeField, Header("�������Q�[�W�̃x�[�X")]
    private GameObject imgInputGaugeBase;
    [SerializeField, Header("�������Q�[�W")]
    private Image imgInputGauge;
    [SerializeField, Header("�A�ŃQ�[�W")]
    private Image imgRepeatGauge;
    [SerializeField] private Sprite[] spriteArrayBeer;

    private InputControl IC; // �C���v�b�g�A�N�V�������`
    [SerializeField, Header("�O�̃{�^��")]
    private GameObject goNotesBefore;

    private enum KIND_BUTTON
    {
        NONE,               // ���I��
        PUSH,               // ����
        PUSH_LONG,          // ������
        PUSH_REPEAT_THREE,  // 3�A��
        PUSH_REPEAT_FIVE,   // 5�A��
    }
    [SerializeField, Header("�`���[�g���A���̂ݑI��")]
    private KIND_BUTTON kind_button;
    private int iIndexMaxKind_Button = (int)KIND_BUTTON.PUSH_REPEAT_FIVE;

    private float fTimerPushLong;
    private bool isPushed; // true = ������, false = not ����
    private int iCountPush;

    [SerializeField] private TextAsset level_adjust;
    private static Dictionary<KIND_BUTTON, int> numberGenerate = new Dictionary<KIND_BUTTON, int>();

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        imgButton = GetComponent<Image>();

        DecideButton();

        // ������Ԃ̏�����
        fTimerPushLong = 0;
        isPushed = false;
        iCountPush = 0;

        DecideKindOfButton();

        // �s�K�v�ȉ摜���\��
        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                imgInputGaugeBase.SetActive(false);
                imgRepeatGauge.gameObject.SetActive(false);
                break;
            case KIND_BUTTON.PUSH_LONG:
                imgRepeatGauge.gameObject.SetActive(false);
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
                imgInputGaugeBase.SetActive(false);

                // Sprite�z���List�ɕϊ�
                List<Sprite> spriteList = new List<Sprite>(spriteArrayBeer);

                // 2�Ԗ�(�C���f�b�N�X1)��4�Ԗ�(�C���f�b�N�X3)���폜
                spriteList.RemoveAt(1);
                spriteList.RemoveAt(3);

                // List���Ă�Sprite�z��ɕϊ�
                spriteArrayBeer = spriteList.ToArray();
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                imgInputGaugeBase.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < 4)
        {
            // �擪3��\��
            imgButton.color = Color.white;
        }
        //else if(transform.position.x < 6)
        //{
        //    // 4�߂𔼓���
        //    imgButton.color = new Color(1, 1, 1, 0.5f);
        //}
        else
        {
            // �ȍ~��\��
            imgButton.color = Color.black;
        }

        if (!isPushed) return;

        if (fTimerPushLong >= 0.5f)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            // �{�^�����������͌o�ߎ��Ԃ����Z
            fTimerPushLong += Time.deltaTime;
            imgInputGauge.fillAmount = fTimerPushLong / 0.5f;
        }
    }

    private void OnDestroy()
    {
        // �C���v�b�g�A�N�V�����̖�����
        IC.Disable();
    }

    /// <summary>
    /// �L�[�̌���
    /// </summary>
    private void DecideButton()
    {
        // �{�^���̒��I
        int rand = Random.Range(0, spriteArrayXboxButton.Length);

        // �摜�̏�����
        if (Gamepad.all.Count > 0)
        {
            imgButton.sprite = spriteArrayXboxButton[rand];
        }
        else
        {
            imgButton.sprite = spriteArrayArrow[rand];
        }
        imgInputGauge.fillAmount = 0;
        imgRepeatGauge.sprite = spriteArrayBeer[0];

        // �C���v�b�g�A�N�V�������擾
        IC = new InputControl();
        // �A�N�V�����ɃC�x���g��o�^
        switch (rand)
        {
            case 0:
                InitializedButton(IC.Player.UpKey);
                break;
            case 1:
                InitializedButton(IC.Player.DownKey);
                break;
            case 2:
                InitializedButton(IC.Player.LeftKey);
                break;
            case 3:
                InitializedButton(IC.Player.RightKey);
                break;
        }
        // �C���v�b�g�A�N�V�����̗L����
        IC.Enable();
    }

    /// <summary>
    /// �L�[�̏�����
    /// </summary>
    /// <param name="inputAction">�@�\������L�[</param>
    private void InitializedButton(InputAction inputAction)
    {
        SetInputAndRelease(inputAction, IC.Player.UpKey);
        SetInputAndRelease(inputAction, IC.Player.DownKey);
        SetInputAndRelease(inputAction, IC.Player.LeftKey);
        SetInputAndRelease(inputAction, IC.Player.RightKey);
    }

    /// <summary>
    /// �����Ɨ��������̃C�x���g��ݒ肷��
    /// </summary>
    /// <param name="inputAction">���͂��ꂽ�L�[</param>
    /// <param name="keyKind">�L�[�̎��</param>
    private void SetInputAndRelease(InputAction inputAction, InputAction keyKind)
    {
        keyKind.started += (inputAction == keyKind) ? InputButton : MissButton;
        if (inputAction == keyKind) keyKind.canceled += ReleaseButton;
    }


    /// <summary>
    /// �{�^���̎�ނ�����
    /// </summary>
    private void DecideKindOfButton()
    {
        if (kind_button != KIND_BUTTON.NONE) return;

        if (numberGenerate.Count == 0)
        {
            // csv�̓ǂݍ���
            StringReader reader = new StringReader(level_adjust.text); // TextAsset��StringReader�ɕϊ�
            List<string[]> csvData = new List<string[]>(); // CSV�t�@�C���̒��g�����郊�X�g
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine(); // 1�s���ǂݍ���
                csvData.Add(line.Split(',')); // csvData���X�g�ɒǉ�����
            }

            // �f�[�^���
            int iIndexLevel = (int)GameManager.GetInstance.GetLevelState;
            numberGenerate[KIND_BUTTON.PUSH] = int.Parse(csvData[iIndexLevel][2]);
            numberGenerate[KIND_BUTTON.PUSH_LONG] = int.Parse(csvData[iIndexLevel][3]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_THREE] = int.Parse(csvData[iIndexLevel][4]);
            numberGenerate[KIND_BUTTON.PUSH_REPEAT_FIVE] = int.Parse(csvData[iIndexLevel][5]);
        }

        // �{�^���A�N�V�����̑I��
        int rand = 0;
        do
        {
            rand = Random.Range(1, iIndexMaxKind_Button + 1);
            if (numberGenerate[(KIND_BUTTON)rand] > 0)
            {
                numberGenerate[(KIND_BUTTON)rand]--;
                break;
            }
        }
        while (true);
        kind_button = (KIND_BUTTON)rand;
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void InputButton(InputAction.CallbackContext context)
    {
        if (goNotesBefore != null) return;

        // null�`�F�b�N
        if (kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("�{�^���A�N�V���������I���ł�");
            return;
        }

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                Destroy(transform.parent.gameObject);
                break;
            case KIND_BUTTON.PUSH_LONG:
                isPushed = true;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                iCountPush++;
                break;
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    private void ReleaseButton(InputAction.CallbackContext context)
    {
        if (goNotesBefore != null) return;

        // null�`�F�b�N
        if (kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("�{�^���A�N�V���������I���ł�");
            return;
        }

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH_LONG:
                // �{�^���������Ă��Ȃ��ƌo�ߎ��Ԃ̓[��
                imgInputGauge.fillAmount = 0;
                fTimerPushLong = 0;
                isPushed = false;
                break;
            case KIND_BUTTON.PUSH_REPEAT_THREE:
                CountPushNumber(3);
                break;
            case KIND_BUTTON.PUSH_REPEAT_FIVE:
                CountPushNumber(5);
                break;
        }
    }

    /// <summary>
    /// �����񐔂𐔂���
    /// </summary>
    /// <param name="iCountMax">�������</param>
    private void CountPushNumber(int iCountMax)
    {
        if (iCountPush >= iCountMax) Destroy(transform.parent.gameObject);
        else imgRepeatGauge.sprite = spriteArrayBeer[iCountPush];
    }

    /// <summary>
    /// �L�[���͂̎��s����
    /// </summary>
    private void MissButton(InputAction.CallbackContext context)
    {
        SpeedAdjust.AddMissCnt();
    }

    /// <summary>
    /// �{�^���̐������̍��v����ɂ���
    /// </summary>
    public static void ClearNumberGenerate() { numberGenerate.Clear(); }
}
