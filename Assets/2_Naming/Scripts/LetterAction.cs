using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData;

public class LetterAction : MonoBehaviour
{
    [SerializeField] private Text txtName;
    private string strLetter;

    private InputControl IC; // �C���v�b�g�A�N�V�������`

    // Start is called before the first frame update
    void Start()
    {
        strLetter = transform.GetComponentInChildren<Text>().text;

        // �C���v�b�g�A�N�V�������擾
        IC = new InputControl();

        // �A�N�V�����ɃC�x���g��o�^
        IC.Player.Decide.started += OnDecide;

        // �C���v�b�g�A�N�V�����̗L����
        IC.Enable();
    }

    /// <summary>
    /// ����{�^���̉�������
    /// </summary>
    /// <param name="context">����{�^��</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;

        if(strLetter == "�����Ă�")
        {

        }
        else if(strLetter == "��������")
        {
            if (txtName.text == "���O�����") return;

            // 1�����폜
            txtName.text = txtName.text.Substring(0, txtName.text.Length - 1);

            if(txtName.text.Length == 0)
            {
                // �W���ɖ߂�
                txtName.text = "���O�����";
            }
        }
        else if(txtName.text == "���O�����")
        {
            // 1�����ڂ��n�߂�
            txtName.text = strLetter;
        }
        else if (txtName.text.Length < 5)
        {
            // ������ǉ�����
            txtName.text += strLetter;
        }
    }

    private void OnDestroy()
    {
        // �C���v�b�g�A�N�V�����̒�~
        IC.Disable();
    }
}
