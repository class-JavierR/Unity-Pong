using UnityEngine;
using Unity.Netcode;

public class LeftPaddleController : PaddleController, ICollidable
{
    protected override float GetInput()
    {
        return Input.GetAxis("Player1Vertical"); 
    }

    public void OnHit(Collision2D collision)
    {
        Debug.Log("Paddle OnHit triggered by: " + collision.gameObject.name);
    }
}
