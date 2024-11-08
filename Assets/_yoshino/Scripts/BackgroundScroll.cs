using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("�����ʒu")]
    private Vector3 positionInitialize;
    [SerializeField, Header("�ڕW�n�_")]
    private float position_xGoal;

    // Start is called before the first frame update
    void Start()
    {
        // �����z�u
        transform.position = positionInitialize;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // �w�i�̃X�N���[��
        transform.Translate(10 * -Time.deltaTime, 0, 0);

        if (transform.position.x <= position_xGoal)
        {
            // �����ʒu��
            transform.position = positionInitialize;
        }
    }
}
