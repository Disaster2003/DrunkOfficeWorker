using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // �VInput�V�X�e���̗��p�ɕK�v
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private Sprite[] UI_Button;
    [SerializeField] private Image imgInputGauge;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    [SerializeField] private GameObject imgBeforeButton;

    private enum KIND_BUTTON
    {
        NONE,       // ���I��
        PUSH,       // ����
        PUSH_LONG,  // ������
        PUSH_REPEAT,// �A��
    }
    [SerializeField, Header("�`���[�g���A���p�{�^���A�N�V�����̑I��")]
    private KIND_BUTTON kind_button;
    private float timerPushLong;
    private bool isPushed; // true = ������, false = not ����
    private int countPush;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 4);

        // �摜�̏�����
        GetComponent<Image>().sprite = UI_Button[rand];
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

        if (kind_button == KIND_BUTTON.NONE)
        {
            // �{�^���A�N�V�����̑I��
            rand = Random.Range(1, 4);
            kind_button = (KIND_BUTTON)rand;
        }

        // ������Ԃ̏�����
        timerPushLong = 0;
        countPush = 0;
        isPushed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPushed)
        {
            if (timerPushLong > 0.5f)
            {
                // ���g�̔j��
                Destroy(gameObject);
            }

            // �{�^�����������͌o�ߎ��Ԃ����Z
            timerPushLong += Time.deltaTime;
            imgInputGauge.fillAmount = timerPushLong / 0.5f;
        }

        if (imgBeforeButton != null) return;

        if (transform.position.x > 5)
        {
            // �{�^�������炷
            transform.Translate(-Time.deltaTime, 0, 0);
            return;
        }
    }

    private void OnDestroy()
    {
        // �C���v�b�g�A�N�V�����̖�����
        IC.Disable();
        Destroy(imgInputGauge.gameObject);
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void InputButton(InputAction.CallbackContext context)
    {
        // null�`�F�b�N
        if(kind_button == KIND_BUTTON.NONE)
        {
            Debug.LogError("�{�^���A�N�V���������I���ł�");
            return;
        }

        if (imgBeforeButton != null) return;

        switch (kind_button)
        {
            case KIND_BUTTON.PUSH:
                // ���g�̔j��
                Destroy(gameObject);
                break;
            case KIND_BUTTON.PUSH_LONG:
                isPushed = true;
                break;
            case KIND_BUTTON.PUSH_REPEAT:
                countPush++;
                break;
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    private void ReleaseButton(InputAction.CallbackContext context)
    {
        // �{�^���������Ă��Ȃ��ƌo�ߎ��Ԃ̓[��
        imgInputGauge.fillAmount = 0;
        timerPushLong = 0;
        isPushed = false;

        if (countPush > 5)
        {
            // ���g�̔j��
            Destroy(gameObject);
        }
    }
}
