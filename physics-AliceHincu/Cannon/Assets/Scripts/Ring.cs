using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private AudioSource bellSound;

    // Start is called before the first frame update
    void Start()
    {
        bellSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.tag == "Clapper")
            bellSound.Play();
    }
}