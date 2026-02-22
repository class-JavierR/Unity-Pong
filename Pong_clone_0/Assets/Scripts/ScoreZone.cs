using Unity.Netcode;
using UnityEngine;

public class ScoreZone : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (!collision.CompareTag("Ball")) return;

        if (CompareTag("LeftScoreZone"))
        {
            FindFirstObjectByType<GameManager>().AddRightScore();
        }
        else if (CompareTag("RightScoreZone"))
        {
            FindFirstObjectByType<GameManager>().AddLeftScore();
        }
    }
}