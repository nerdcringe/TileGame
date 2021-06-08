using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    public Transform player;
    public float moveRate;
    public float smoothnessCurve;
    Vector3 vel;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(NoiseGen.width/2, NoiseGen.height/2, transform.position.z);
        vel = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Center camera position on player smoothly.
        float speed = Vector3.Distance(transform.position, player.position) * moveRate;
        speed = Mathf.Pow(speed, smoothnessCurve);
        Vector3 camPos = Vector3.Lerp(transform.position, player.position, Time.fixedDeltaTime * speed);

        // Keep camera is at correct z position (-10).
        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);

        // Center camera position on player smoothly.
        /*float speed = Vector3.Distance(transform.position, player.position) * 0.25f;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, player.position, Time.deltaTime * speed);

        // Keep camera is at correct z position (-10).
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);*/
    }
}
