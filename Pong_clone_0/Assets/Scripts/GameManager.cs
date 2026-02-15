using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    private bool ballSpawned = false;


    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        NetworkManager.OnClientConnectedCallback += OnClientConnected;

    }
    private void OnClientConnected(ulong clientId)
    {
        // Spawn when 2 players are connected
        if (NetworkManager.ConnectedClients.Count >= 2 && !ballSpawned)
        {
            SpawnBall();
            ballSpawned = true;
        }
    }
    private void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ball.GetComponent<NetworkObject>().Spawn();
    }
}
