using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            // ���g�̔j��
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // �v���C���[�̏�Ԑݒ�
        FindObjectOfType<PlayerComponent>().SetPlayerState(GetComponentInParent<HurdleComponent>().GetPlayerState());
    }
}
