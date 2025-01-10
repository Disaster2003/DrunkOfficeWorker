using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Header("�ڕW�n�_")]
    private Vector2 positionGoal = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // �����z�u
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Hurdle")) return;

        // �w�i�̃X�N���[��
        transform.Translate(10 * -Time.deltaTime, 0, 0);

        if (transform.position.x <= positionGoal.x)
        {
            // �����ʒu��
            transform.position = Vector3.zero;
        }
    }
}
