using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed;
    public Sprite spriteUp;
    public Sprite spriteRight;

    private Vector2 movement;

    [SerializeField] private Vector3 velocity;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private Vector3 lastCollisionPoint;
    [SerializeField] private Vector3 forward;

    public event Action<(BuildingResource,int)> PlayerResourceUpdated;
    [SerializeField] private BuildingResource ressource;
    [SerializeField] int count;
    public int maxCarry;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector3.zero;
        ressource = null;
        maxCarry = 4;
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void Update(){
        if (movement.x > 0)
        {
            
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = false;
            
        }
        else if (movement.y > 0)
        {
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
            
        }
        else if (movement.y < 0)
        {
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Level")) return;
        lastCollisionPoint = ContactPointAverage(collision.contacts) + (Vector2)forward * 0.5f;
        if (LevelManager.Instance.TryGetCollisionTile(transform.position + forward * 0.5f, out ICollisionTile tile))
        {
            tile.OnCollision();
        }
    }

    private Vector2 ContactPointAverage(ContactPoint2D[] points)
    {
        Vector2 point = Vector2.zero;
        for (int i = 0; i < points.Length; i++)
        {
            point += points[i].point;
        }
        return point /= points.Length;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lastCollisionPoint, 0.1f);
    }
    
    public BuildingResource getResource()
    {
        return ressource;
    }

    public void setResource(BuildingResource resource)
    {
        this.ressource = resource;
        count = maxCarry;
        PlayerResourceUpdated?.Invoke((resource, count));
    }
    public void UseResource()
    {
        if (ressource == null) return;
        if (count == 0) return;
        if (count == 1)
        {
            count = 0;
            ressource = null;
        }else {
            count--;
        }
        PlayerResourceUpdated?.Invoke((ressource, count));
    }

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}

