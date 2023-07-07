using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector3 velocity;
    private Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        rotation = new Quaternion(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            velocity = Vector3.up;
            rotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            velocity = Vector3.down;
            rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            velocity = Vector3.left;
            rotation = Quaternion.Euler(0, 0, -90);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            velocity = Vector3.right;
            rotation = Quaternion.Euler(0, 0, 90);
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
        transform.rotation = rotation;
    }
}

