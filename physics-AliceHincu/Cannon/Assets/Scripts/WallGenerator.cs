using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public GameObject brickPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = -5; i < 6; i++) 
        {
            for(int height = 0; height < 10; height++)
            {
                GameObject brick = Instantiate(brickPrefab) as GameObject;
                brick.transform.position = new Vector3(transform.position.x, transform.position.y + height + 0.5f, transform.position.z + i*2);
            }
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
