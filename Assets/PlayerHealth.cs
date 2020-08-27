using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHealth : MonoBehaviour
{
    public const float invincibilityInterval = 3;
    const int maxHealth = 4;
    public int health = maxHealth;

    public CharacterMovement mov;
    public Sprite hurtSprite;
    public Sprite deadSprite;
    public AudioManager audioManager;

    public TileDefs tileDefs;
    public Tilemap tilemap;

    Sprite defaultSprite;
    SpriteRenderer sr;
    Rigidbody2D rb;

    bool invincible;
    public float invincibilityTimer = 0;

    protected virtual void Cease()
    {
        mov.speed = 0;
        sr.sprite = deadSprite;
    }

    public void TakeDamage(Vector2 pos)
    {
        if (!invincible)
        {
            health -= 1;
            sr.sprite = hurtSprite;
            Vector2 diffPos = pos - rb.position;
            rb.AddForce(diffPos * 5);
            invincible = true;
        }

        if (health <= 0)
        {
            Cease();
        }

        invincibilityTimer += Time.deltaTime;
    }


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        defaultSprite = sr.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = new Vector3Int(Mathf.RoundToInt(mov.transform.position.x), Mathf.RoundToInt(mov.transform.position.y), 0);
        if (health > 0)
        {
            if (tilemap.GetTile(pos) == tileDefs.magmaTile)
            {
                TakeDamage(new Vector2(pos.x, pos.y));
            }

            if (invincibilityTimer == 0)
            {
                sr.sprite = defaultSprite;
            }
            else
            {
                invincibilityTimer += Time.deltaTime;

                if (invincibilityTimer > invincibilityInterval)
                {
                    invincible = false;
                    invincibilityTimer = 0;
                }
            }

            // Eat meat if over meat tile, healing 1 health.

            if (health < maxHealth)
            {
                if (tileDefs.cookedMeatTile.Equals(tilemap.GetTile(mov.targetPos)))
                {
                    audioManager.PlaySound(audioManager.eat, mov.targetPos);
                    health += 1;
                    tilemap.SetTile(mov.targetPos, null);
                }
            }
        }


        if (health <= 0)
        {
            Cease();
        }

        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (health > 0)
        {
            string tag = gameObject.tag;
            if ((collision.gameObject.tag.Equals("Enemy") && (tag.Equals("Player") || tag.Equals("Ally"))))
            {
                TakeDamage(collision.gameObject.transform.position);
            }
        }
    }

}
