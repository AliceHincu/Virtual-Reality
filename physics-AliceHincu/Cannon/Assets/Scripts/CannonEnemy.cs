using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    public GameObject cannonBallPrefab;
    public Transform firePoint;

    private Camera _cam;
    public Vector3 initialVelocity;

    float timePassed = 0f;
    float nrSecondsTillNextHit = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main; // optimization trick to avoid recomputing who the main camera is
        transform.LookAt(new Vector3(0, initialVelocity.y, initialVelocity.z));
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > nrSecondsTillNextHit)
        {
            // shoot every X seconds
            _Fire();
            timePassed = 0f;
        } 
        
    }

    private void _Fire()
    {
        // instantiate a cannon ball
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, Quaternion.identity); // = no rotation
        // apply some force
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(initialVelocity, ForceMode.Impulse); // add an instance force impulse to the rididbogy, using its mass.
    }
}
