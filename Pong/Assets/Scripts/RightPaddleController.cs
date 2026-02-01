using UnityEngine;

public class RightPaddleController : PaddleController
{
    protected override float GetInput()
    {
        return Input.GetAxis("Player2Vertical");
    }
}

