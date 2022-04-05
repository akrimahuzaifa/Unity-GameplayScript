using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateRoatate : MonoBehaviour
{
    Rigidbody rb;
    //[Range (1f,5f)] 
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float zAxis = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        if (!Input.GetButton("Jump"))
        {
            transform.Translate(new Vector3(xAxis, 0, zAxis));
        }

        if (Input.GetButton("Jump"))
        {
            transform.Translate(new Vector3(0, 1, 0));
            //rb.velocity = new Vector3(rb.velocity.x, speed, rb.velocity.z);
        }
    }
}
