using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed;
    public float speedModifier = 1f;
    public Sprite spriteUp;
    public Sprite spriteRight;
    public float extension = 0.1f;

    private Vector2 movement;

    private SoundEmitter soundEmitter_Move;
    [SerializeField]private AudioClip audioClip_Move;
    private SoundEmitter soundEmitter_Idle;
    [SerializeField]private AudioClip audioClip_Idle;

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

        Debug.Log("PlayerController Start");

        soundEmitter_Idle = SoundManager.instance.PlaySound(audioClip_Idle, instance.gameObject, true, 0.1f);
        Debug.Log("soundEmitter_Idle: " + soundEmitter_Idle);
        soundEmitter_Move = SoundManager.instance.PlaySound(audioClip_Move, instance.gameObject, true, 0.2f);
        Debug.Log("soundEmitter_Move: " + soundEmitter_Move);
    }

    private void PlayerController_bnjaiosgfbij()
    {
        throw new NotImplementedException();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (speed * speedModifier) * Time.fixedDeltaTime);
    }

    void Update(){
        if (movement.magnitude > 0)
        {
            soundEmitter_Idle.audioSource.Stop();
            if (!soundEmitter_Move.audioSource.isPlaying) soundEmitter_Move.audioSource.Play();
        }else{
            soundEmitter_Move.audioSource.Stop();
            if (!soundEmitter_Idle.audioSource.isPlaying) soundEmitter_Idle.audioSource.Play();
        }

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
        if (movement.magnitude > 0)
        {
            forward = movement.normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Level")) return;
        lastCollisionPoint = ContactPointAverage(collision.contacts) + (Vector2)forward * extension;
        if (LevelManager.Instance.TryGetCollisionTile(transform.position + forward * extension, out ICollisionTile tile))
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

