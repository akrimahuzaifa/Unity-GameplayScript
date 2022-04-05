using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtFunc : MonoBehaviour
{
    GameObject playerToFollow;

    // Start is called before the first frame update
    void Start()
    {
        playerToFollow = GameObject.Find("Cube");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerToFollow.transform); 
    }
}
