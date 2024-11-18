using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    public Vector2 targetPosition;
    public float speed = 1.0f;  //速さ
    public bool isMoveflg = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveflg)
        {
            //現在地を取得
            Vector2 currentPosition = transform.position;

            //移動
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
                isMoveflg = false;
            }
        }
        
    }
}
