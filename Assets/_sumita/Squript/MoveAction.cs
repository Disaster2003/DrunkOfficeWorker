using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    public Vector2 targetPosition;
    public float speed = 1.0f;  //ë¨Ç≥
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
            //åªç›ínÇéÊìæ
            Vector2 currentPosition = transform.position;

            //à⁄ìÆ
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
                isMoveflg = false;
            }
        }
        
    }
}
