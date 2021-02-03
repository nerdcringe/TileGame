using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMonitor : MonoBehaviour
{
    public PlayerHealth player;

    public Sprite health4;
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health >= 4)
        {
            image.sprite = health4;
        }
        else if (player.health == 3)
        {
            image.sprite = health3;
        }
        else if (player.health == 2)
        {
            image.sprite = health2;
        }
        else if (player.health == 1)
        {
            image.sprite = health1;
        }
        else
        {
            image.sprite = health0;
        }
    }
}
