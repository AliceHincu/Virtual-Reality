using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public GameObject cannonBallPrefab;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    private const int N_TRAJECTORY_POINTS = 10;
    private bool _pressingMouse = false;
    private float _zPlaneTrajectory = 0;
    private Camera _cam;
    private Vector3 _initialVelocity;

    float map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    float max(float a, float b) {
        return a > b ? a : b;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main; // optimization trick to avoid recomputing who the main camera is
        _zPlaneTrajectory = GameObject.Find("cannon").transform.position.z - Camera.main.transform.position.z; // same z-plane as the cannon
        lineRenderer.positionCount = N_TRAJECTORY_POINTS;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            _pressingMouse = true;
            lineRenderer.enabled = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            _pressingMouse = false;
            lineRenderer.enabled = false;
            _Fire();
        }

        if (_pressingMouse) // look at the mouse position
        {
            // coordinate transform screen > world
            _zPlaneTrajectory = map(max(Input.mousePosition.x, Screen.width / 2), Screen.width / 2, Screen.width, 35, 45); // calculate z position based on x pos of mouse
            Vector3 mousePos = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zPlaneTrajectory)); 
            transform.LookAt(mousePos);
            _initialVelocity = mousePos - firePoint.position;
            _UpdateLineRenderer();
        }
    }

    private void _Fire()
    {
        // instantiate a cannon ball
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, Quaternion.identity); // = no rotation
        // apply some force
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(_initialVelocity, ForceMode.Impulse); // add an instance force impulse to the rididbogy, using its mass.
    }

    private void _UpdateLineRenderer()
    {
        // newtonian physics stuff
        float g = Physics.gravity.magnitude;
        float velocity = _initialVelocity.magnitude;
        float angle = Mathf.Atan2(_initialVelocity.y, _initialVelocity.x);

        Vector3 start = firePoint.position;
        
        float timeStep = 0.1f;
        float fTime = 0f;
        for(int i = 0; i < N_TRAJECTORY_POINTS; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle);
            float dy = velocity * fTime * Mathf.Sin(angle) - (g * fTime * fTime /2f);
            Vector3 pos = new Vector3(start.x + dx, start.y + dy, 0);
            lineRenderer.SetPosition(i, pos);
            fTime += timeStep;
        }
    }
}
