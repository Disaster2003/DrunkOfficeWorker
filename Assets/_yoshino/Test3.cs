using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Color.black;
        //Color.blue;
        //Color.clear;
        //Color.cyan;
        //Color.gray;
        //Color.green;
        //Color.grey;
        //Color.magenta;
        //Color.red;
        //Color.white;
        //Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.contacts[0].point);
    }
}
