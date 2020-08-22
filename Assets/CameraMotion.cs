using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(NoiseGen.width / 2, NoiseGen.height / 2, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Center camera position on player smoothly.
        float speed = Vector3.Distance(transform.position, player.position) * Time.deltaTime * 3;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed);

        // Keep camera is at correct z position (-10)
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
