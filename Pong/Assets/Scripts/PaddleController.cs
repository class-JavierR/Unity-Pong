using UnityEngine;

public abstract class PaddleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected float moveSpeed = 8f;
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        float input = GetInput();
        rb.velocity = new Vector2(0,input * moveSpeed);
    }
    protected abstract float GetInput();
}
