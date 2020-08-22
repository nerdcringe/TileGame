using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{

    public Vector3 vel;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        rb2d.velocity = vel;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
        {
            BotHealth health = collision.gameObject.GetComponent<BotHealth>();
            if (health != null)
            {
                health.Cease();
            }
            Destroy(gameObject);
        }
    }

}
