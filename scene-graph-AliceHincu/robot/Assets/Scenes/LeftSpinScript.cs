using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSpinScript : MonoBehaviour
{
    public float v = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dang = Time.deltaTime * v;
        transform.Rotate(Vector3.up, dang);
        transform.Rotate(Vector3.left, dang);
        transform.Rotate(Vector3.back, dang);
    }
}