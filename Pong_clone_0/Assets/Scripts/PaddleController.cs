using UnityEngine;
using Unity.Netcode;
public abstract class PaddleController : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected float moveSpeed = 8f;
    protected Rigidbody2D rb;
    public NetworkVariable<float> paddleYPosition = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    [SerializeField] private Transform topWall;
    [SerializeField] private Transform bottomWall;
    private float halfPaddleHeight;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        halfPaddleHeight = GetComponent<BoxCollider2D>().bounds.extents.y;

        if (topWall == null)
        topWall = GameObject.FindGameObjectWithTag("TopWall").transform;

    if (bottomWall == null)
        bottomWall = GameObject.FindGameObjectWithTag("BottomWall").transform;
    }
    public override void OnNetworkSpawn()
{
    if (OwnerClientId == 0)
    {
        transform.position = new Vector3(-9f, 0f, 0f);  // Left side
    }
    else
    {
        transform.position = new Vector3(8f, 0f, 0f);   // Right side
    }

    paddleYPosition.Value = transform.position.y;
}
    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsOwner) {
             // Owner: Read input and move
            float input = GetInput();
            float newY = transform.position.y + input * moveSpeed * Time.deltaTime;

            //adds how far the paddles go from top and bottom walls ; prevents clipping through walls
            float minY = bottomWall.position.y + halfPaddleHeight + 0.6f;
            float maxY = topWall.position.y - halfPaddleHeight - 0.6f;
            newY = Mathf.Clamp(newY, minY, maxY);

            // Update local position immediately (no lag for owner)
            transform.position = new Vector3(transform.position.x, newY, 0);
            // Update NetworkVariable so other clients can see it
            paddleYPosition.Value = newY;
        }
        else
        {
             // Non-owners: Read NetworkVariable and update visual position
            transform.position = new Vector3(transform.position.x, paddleYPosition.Value, 0);
        }
    }
    protected abstract float GetInput();
}
