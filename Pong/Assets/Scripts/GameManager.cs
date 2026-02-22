using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject ballPrefab;
    private NetworkObject spawnedBall;
    [SerializeField] private int scoreLimit = 5;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;

    [SerializeField] private Button startButton;
    private float ballStartSpeed = 5f;
    
    public NetworkVariable<int> leftScore = new NetworkVariable<int>(0);
    public NetworkVariable<int> rightScore = new NetworkVariable<int>(0);
    public NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);
     private void Awake()
    {
        //auto-find UI if not assigned
        if (leftScoreText == null)
            leftScoreText = GameObject.Find("LeftScore")?.GetComponent<TextMeshProUGUI>();
        if (rightScoreText == null)
            rightScoreText = GameObject.Find("RightScore")?.GetComponent<TextMeshProUGUI>();
        if (winText == null)
            winText = GameObject.Find("WinText")?.GetComponent<TextMeshProUGUI>();
        if (startButton == null)
            startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
    }
    public override void OnNetworkSpawn()
    {
        leftScore.OnValueChanged += OnScoreChanged;
        rightScore.OnValueChanged += OnScoreChanged;
        UpdateUI();

        if (!IsServer) {
        //hide the button on clients
        if (startButton != null) startButton.gameObject.SetActive(false);
        return; }

        NetworkManager.OnClientConnectedCallback += OnClientConnected;
    }
    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer) return;

        if (NetworkManager.ConnectedClients.Count >= 2 && startButton != null) {
            //shows the button once both players connect
            startButton.gameObject.SetActive(true); }
    }
    private void OnScoreChanged(int oldValue, int newValue)
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (leftScoreText != null)
            leftScoreText.text = leftScore.Value.ToString();

        if (rightScoreText != null)
            rightScoreText.text = rightScore.Value.ToString();
    }

    private void SpawnBall()
    {
        if (spawnedBall != null) return;

        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        spawnedBall = ball.GetComponent<NetworkObject>();
        spawnedBall.Spawn();
    }
    public void StartGame()
    {
        if(!IsServer) return;
        //resets score and sets gameOver to false.
            leftScore.Value = 0;
            rightScore.Value = 0;
            gameOver.Value = false;
        //hide win message and start button once game starts
        if (winText != null)
            winText.gameObject.SetActive(false);
        if (startButton != null)
            startButton.gameObject.SetActive(false);
        SpawnBall();
    }
    //checks score for both left and right
    public void AddLeftScore()
    {
        if (!IsServer || gameOver.Value) return;

        leftScore.Value++;
        CheckWinCondition();
        ResetBall();
    }

    public void AddRightScore()
    {
        if (!IsServer || gameOver.Value) return;

        rightScore.Value++;
        CheckWinCondition();
        ResetBall();
    }
    //checks win condition to see if any player hits 5
    private void CheckWinCondition()
    {
        if (leftScore.Value >= scoreLimit) {
            ShowWinTextClientRpc("Left Player Wins!");
            gameOver.Value = true;
            StopBall();
            DespawnBall();
            if (startButton != null) startButton.gameObject.SetActive(true); }

        else if (rightScore.Value >= scoreLimit) {
            ShowWinTextClientRpc("Right Player Wins!");
            gameOver.Value = true;
            StopBall();
            DespawnBall();
            if (startButton != null) startButton.gameObject.SetActive(true); }
    }

    //displays the winner with this function
    [ClientRpc]
    private void ShowWinTextClientRpc(string message)
{
    if (winText != null)
    {
        winText.text = message;
        winText.gameObject.SetActive(true); // shows the message
    }
}

    //resets the ball after the point is scored
    private void ResetBall()
    {
        if (spawnedBall == null) return;

        Rigidbody2D rb = spawnedBall.GetComponent<Rigidbody2D>();

        //stops ball movement
        rb.velocity = Vector2.zero;
        rb.position = Vector2.zero;
        //randomly chooses direction
        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.Range(-0.5f, 0.5f);
        Vector2 randomDirection = new Vector2(x, y).normalized;
        rb.velocity = randomDirection * ballStartSpeed;
    }
    //stops the ball after the game is over
    private void StopBall()
    {
        if (spawnedBall == null) return;

        Rigidbody2D rb = spawnedBall.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.position = Vector2.zero;
    }
    private void DespawnBall()
    {
        if (spawnedBall == null) return;
        //once the game is over, despawn ball from host and client
        if (spawnedBall.IsSpawned)
            spawnedBall.Despawn(); 

            spawnedBall = null;
    }
}
