using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed;
    public Sprite spriteUp;
    public Sprite spriteRight;

    private Vector3 velocity;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Vector3 lastCollisionPoint;
    private Vector3 forward;

    private ScriptableObject ressource;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector3.zero;
        ressource = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocity = Vector3.up;
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity = Vector3.down;
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity = Vector3.left;
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity = Vector3.right;
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        if (!Input.GetKey(KeyCode.W) && velocity == Vector3.up)
        {
            velocity = Vector3.zero;
        }
        if (!Input.GetKey(KeyCode.S) && velocity == Vector3.down)
        {
            velocity = Vector3.zero;
        }
        if (!Input.GetKey(KeyCode.A) && velocity == Vector3.left)
        {
            velocity = Vector3.zero;
        }
        if (!Input.GetKey(KeyCode.D) && velocity == Vector3.right)
        {
            velocity = Vector3.zero;
        }
        if((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) && (velocity == Vector3.down || velocity == Vector3.up))
        {
            velocity = Vector3.zero;
        }
        if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) && (velocity == Vector3.left || velocity == Vector3.right))
        {
            velocity = Vector3.zero;
        }
        rb.MovePosition(transform.position + velocity * speed * Time.deltaTime);
        if (velocity.magnitude > 0) forward = velocity.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Level")) return;
        lastCollisionPoint = ContactPointAverage(collision.contacts) + (Vector2)forward * .1f;
        if (LevelManager.Instance.TryGetCollisionTile(transform.position + forward * .1f, out ICollisionTile tile))
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
        Gizmos.DrawWireCube(lastCollisionPoint, Vector3.one * 0.1f);
    }
    public ScriptableObject getResource()
    {
        return ressource;
    }
    public void setResource(ScriptableObject resource)
    {
        this.ressource = resource;
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

