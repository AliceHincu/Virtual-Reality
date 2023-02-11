using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public int maxNrOfHits = 3;
    private int _nrTimesHit = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "CannonBall")
            _nrTimesHit += 1;
        if (_nrTimesHit >= maxNrOfHits)
            Destroy(gameObject);
    }
}
