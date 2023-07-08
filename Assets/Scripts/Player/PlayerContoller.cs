using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Sprite spriteUp;
    public Sprite spriteRight;

    private Vector3 velocity;

    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if ((velocity.y == -1 && Input.GetKey(KeyCode.W)) || (velocity.y == 1 && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)))
        {
            velocity.y = 0;
        }
        else if ((velocity.x == 1 && Input.GetKey(KeyCode.A)) || (velocity.x == -1 && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)))
        {
            velocity.x = 0;
        }
        else if(Input.GetKey(KeyCode.W))
        {
            velocity = Vector3.up;
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
         else if(Input.GetKey(KeyCode.S))
        {
            velocity = Vector3.down;
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = true;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            velocity = Vector3.left;
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = false;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            velocity = Vector3.right;
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        else if (Input.GetKey(KeyCode.W) == false && velocity == Vector3.up)
        {
            velocity = Vector3.zero;
        }
        else if(Input.GetKey(KeyCode.S) == false && velocity == Vector3.down)
        {
            velocity = Vector3.zero;
        }
        else if(Input.GetKey(KeyCode.A) == false && velocity == Vector3.left)
        {
            velocity = Vector3.zero;
        }
        else if(Input.GetKey(KeyCode.D) == false && velocity == Vector3.right)
        {
            velocity = Vector3.zero;
        } 
        
        transform.position += velocity * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Level")) return;
        var tile = LevelManager.Instance.GetTileAtPosition(collision.contacts[0].point);
        if (tile == null) return;
        if (typeof(ICollisionTile).IsAssignableFrom(tile.GetType()))
        {
            ICollisionTile _t = (ICollisionTile)tile;
            if (_t == null) return;
            _t.OnCollision();
        }
    }
}

