using UnityEngine;

public class RightPaddleController : PaddleController, ICollidable
{
    protected override float GetInput()
    {
        return Input.GetAxis("Player2Vertical");
    }
    public void OnHit(Collision2D collision)
    {
        Debug.Log("Paddle OnHit triggered by: " + collision.gameObject.name);
    }
}

