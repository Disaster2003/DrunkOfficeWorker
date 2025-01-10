using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LetterAction : MonoBehaviour
{
    [SerializeField] private Text txtName;
    private string strLetter;

    private InputControl IC; // インプットアクションを定義

    // Start is called before the first frame update
    void Start()
    {
        // 文字列の取得
        strLetter = transform.GetChild(0).GetComponent<Text>().text;

        // インプットアクションを取得
        IC = new InputControl();

        // アクションにイベントを登録
        IC.Player.Decide.started += OnDecide;

        // インプットアクションの有効化
        IC.Enable();
    }

    /// <summary>
    /// 決定ボタンの押下処理
    /// </summary>
    /// <param name="context">決定ボタン</param>
    private void OnDecide(InputAction.CallbackContext context)
    {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;

        if(strLetter == "さくじょ")
        {
            if (txtName.text == "名前を入力") return;

            // 1文字削除
            txtName.text = txtName.text.Substring(0, txtName.text.Length - 1);

            if(txtName.text.Length == 0)
            {
                // 標準に戻す
                txtName.text = "名前を入力";
            }
        }
        else if(txtName.text == "名前を入力")
        {
            // 1文字目
            txtName.text = strLetter;
        }
        else if (txtName.text.Length < 5)
        {
            // 文字の追加
            txtName.text += strLetter;
        }
    }

    private void OnDestroy()
    {
        // インプットアクションの停止
        IC.Disable();
    }
}
