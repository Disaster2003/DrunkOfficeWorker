using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        switch(GameManager.GetInstance().GetLevelState())
        {
            case GameManager.STATE_LEVEL.NONE: 
            case GameManager.STATE_LEVEL.EASY:
                // 1�̃{�^�����A�N�e�B�u��
                buttons[0].SetActive(true);
                break;
            case GameManager.STATE_LEVEL.NORMAL:
                // 3�̃{�^�����A�N�e�B�u��
                for (int i = 0; i < 3; i++)
                {
                    buttons[i].SetActive(true);
                }
                break;
            case GameManager.STATE_LEVEL.HARD:
                // �S�Ẵ{�^�����A�N�e�B�u��
                foreach (GameObject btn in buttons)
                {
                    btn.SetActive(true);
                }
                break;
        }

        foreach (GameObject btn in buttons)
        {
            if (!btn.activeSelf)
            {
                // ��A�N�e�B�u�̃{�^����j��
                Destroy(btn);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            // �v���C���[�̏�Ԑݒ�
            FindObjectOfType<PlayerComponent>().SetPlayerState(GetComponentInParent<HurdleComponent>().GetPlayerState());

            // ���g�̔j��
            Destroy(gameObject);
        }
        else
        {
            // ���͂����������A���ֈړ�������
            int cnt = 0;
            foreach (GameObject btn in buttons)
            {
                if (btn == null) cnt++;
                else break;       
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(3 - cnt, transform.localPosition.y), GetComponent<SpeedUP>().firstSpeed * Time.deltaTime);
        }
    }
}