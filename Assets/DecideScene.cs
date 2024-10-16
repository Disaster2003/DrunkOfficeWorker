using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //新Inputシステムの利用に必要
using UnityEngine.UI;

public class DecideScene : MonoBehaviour
{
    [SerializeField] Image imgFade;

    [SerializeField] GameManager.STATE_SCENE state_scene;
    [SerializeField] GameManager.STATE_LEVEL state_level;

    [SerializeField] GameObject upButton;
    [SerializeField] GameObject downButton;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;
    public bool isSelected;

    private InputControl IC;
    private Vector2 direction;
    public float intervalKeyChange;

    [SerializeField] GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;

        if (state_scene == GameManager.STATE_SCENE.TITLE || state_scene == GameManager.STATE_SCENE.TUTORIAL)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }

        IC = new InputControl(); // インプットアクションを取得
        IC.Player.Direction.performed += OnDirection; // 全てのアクションにイベントを登録
        IC.Player.Decide.started += OnDecide;
        IC.Enable(); // インプットアクションを機能させる為に有効化する。
        direction = Vector2.zero;
        intervalKeyChange = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(imgFade.fillAmount == 0)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        if (isSelected)
        {
            if (Time.time % 1 <= 0.5f)
            {
                highlight.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                highlight.GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
        else
        {
            highlight.GetComponent<SpriteRenderer>().color = Color.clear;
        }

        if(intervalKeyChange > 0)
        {
            intervalKeyChange += -Time.deltaTime;
        }
    }

    private void OnDirection(InputAction.CallbackContext context)
    {
        if (!isSelected) return;

        direction = context.ReadValue<Vector2>();
        if (direction == Vector2.zero) return;

        if (intervalKeyChange <= 0)
        {
            if (direction.x > 0)
            {
                if (!rightButton) return;
                isSelected = false;
                rightButton.GetComponent<DecideScene>().isSelected = true;
                rightButton.GetComponent<DecideScene>().intervalKeyChange = 0.1f;
                return;
            }
            if (direction.x < 0)
            {
                if (!leftButton) return;
                isSelected = false;
                leftButton.GetComponent<DecideScene>().isSelected = true;
                leftButton.GetComponent<DecideScene>().intervalKeyChange = 0.1f;
                return;
            }
            if (direction.y > 0)
            {
                if (!upButton) return;
                isSelected = false;
                upButton.GetComponent<DecideScene>().isSelected = true;
                upButton.GetComponent<DecideScene>().intervalKeyChange = 0.1f;
                return;
            }
            if (direction.y < 0)
            {
                if (downButton == null) return;
                isSelected = false;
                downButton.GetComponent<DecideScene>().isSelected = true;
                downButton.GetComponent<DecideScene>().intervalKeyChange = 0.1f;
                return;
            }
        }
    }

    private void OnDecide(InputAction.CallbackContext context)
    {
        if (!isSelected) return;
        GameManager.GetInstance().SetNextScene(state_scene, state_level);
    }
}
