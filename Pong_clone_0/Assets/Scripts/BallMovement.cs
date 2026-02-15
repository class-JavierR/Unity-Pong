using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 3f;
    private Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float GetSpeed()
    {
        return speed;
    }
    public void SetSpeed(float value)
    {
        speed = value;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.Range(-0.5f, 0.5f);
        direction = new Vector2(x, y).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsServer) return;
        rb.velocity = direction * speed;
    }
    void OnCollisionEnter2D(Collision2D collision) {
        
        if (!IsServer) return;
        ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnHit(collision);
        }
        OnHit(collision);
    }
    
    public void OnHit(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction = new Vector2(-direction.x, direction.y);
        }
        else
        {
            direction = new Vector2(direction.x, -direction.y);
        }
    }
}
