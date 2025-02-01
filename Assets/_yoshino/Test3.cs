using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    Test3 instance;

    public struct Status
    {
        public int hp,
            atk,
            def;

        public Status(int _hp, int _atk, int _def)
        {
            hp = _hp;
            atk = _atk;
            def = _def;
        }
    }
    private Status status;

    // Start is called before the first frame update
    void Start()
    {
        status = new Status(0, 0, 0);

        Dictionary<int, string> a = new Dictionary<int, string>();
        if (a[1].Length == 0)return;

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
