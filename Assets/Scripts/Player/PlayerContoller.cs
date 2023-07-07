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
        if (Input.GetKeyDown(KeyCode.W))
        {
            velocity = Vector3.up;
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            velocity = Vector3.down;
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            velocity = Vector3.left;
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            velocity = Vector3.right;
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        if (Input.GetKeyUp(KeyCode.W) && velocity == Vector3.up)
        {
            velocity = Vector3.zero;
        }
        if (Input.GetKeyUp(KeyCode.S) && velocity == Vector3.down)
        {
            velocity = Vector3.zero;
        }
        if (Input.GetKeyUp(KeyCode.A) && velocity == Vector3.left)
        {
            velocity = Vector3.zero;
        }
        if (Input.GetKeyUp(KeyCode.D) && velocity == Vector3.right)
        {
            velocity = Vector3.zero;
        }
        transform.position += velocity * speed * Time.deltaTime;
    }
}

