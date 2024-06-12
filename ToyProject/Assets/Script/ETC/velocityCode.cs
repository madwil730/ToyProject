using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velocityCode : MonoBehaviour
{
    public Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
		rigid.velocity = Vector2.right * 2;
	}

    // Update is called once per frame
    void Update()
    {
        //rigid.velocity = Vector2.right * 2;
    }
}
