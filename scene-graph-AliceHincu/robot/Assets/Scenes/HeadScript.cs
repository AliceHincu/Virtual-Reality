using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Transforms a point from screen space into world space, where world space is defined as the coordinate system at the very top of your game's hierarchy.
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));        
        Vector3 forward = transform.position - mouseWorld; // The direction to look in.
        Vector3 upwards = Vector3.up; // The vector that defines in which direction up is.
        transform.rotation = Quaternion.LookRotation(forward, upwards);
    }
}
