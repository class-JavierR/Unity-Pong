using UnityEngine;

public class LeftPaddleController : PaddleController
{
    protected override float GetInput()
    {
        return Input.GetAxis("Player1Vertical");
    }
}
