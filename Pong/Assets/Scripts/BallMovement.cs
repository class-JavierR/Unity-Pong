using UnityEngine;

public class BallMovement : MonoBehaviour
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
        rb.velocity = new Vector2(3f, 3f);
        direction = new Vector2(1,1);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction = new Vector2(-direction.x, direction.y);
        }
        else
        {
            direction = new Vector2(direction.x, -direction.y);
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
}
