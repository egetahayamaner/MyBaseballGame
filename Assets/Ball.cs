using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using TMPro;

public class Ball : MonoBehaviour
{
    public StrikerMovement strikerMovement;
    public Transform strikerPoint, baseGuy1, baseGuy2, baseGuy3, baseGuy4;
    bool isThrowed = false, catcher = false;
    public GameObject ballCatcher;
    public GameObject GameScore;
    public Transform[] randomLocations;
    private int currentRound = 1;
    private int maxRounds = 9;
    public TMPro.TextMeshProUGUI roundText;
    private bool roundTriggered = false;
    private float lastTriggerTime = 0f;
    private float triggerCooldown = 2f; // 2 seconds cooldown
    public GameObject gameUI;
    public GameObject gameOverPanel;
    public int blueTeamScore = 0;
    public int redTeamScore = 0;
    private bool ballReachedFinal = false;
    private bool strikerReachedFinal = false;
    public TextMeshProUGUI blueTeamScoreText;
    public TextMeshProUGUI redTeamScoreText;
    public TextMeshProUGUI blueTeamFinalScoreText;
    public TextMeshProUGUI redTeamFinalScoreText;
    private bool scoredThisRound = false;



    void FirstThrow()
    {
        transform.DOMove(strikerPoint.position, 2).OnComplete(BallHit);
    }

    void BallHit()
{
    transform.parent = null;
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.useGravity = true;
    rb.isKinematic = false;
    Invoke(nameof(BallCatcher), 1);

    // Generate a random direction
    Vector3 randomDirection = new Vector3(Random.Range(-0.5f, -0.3f), Random.Range(0.2f, 0.8f), Random.Range(-0.7f, -0.1f));
    Vector3 torque = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.9f, 1.1f), Random.Range(-0.1f, 0.1f));

    // Apply the random force and torque
    rb.AddForce(randomDirection.normalized * Random.Range(800, 1300));
    rb.AddTorque(torque * Random.Range(500, 1000));

    if (strikerMovement != null)
    {
        strikerMovement.StartRunning();
    }
    else
    {
        Debug.LogError("StrikerMovement script not assigned or found.");
    }
}


    void BallCatcher()
    {
        catcher = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastTriggerTime < triggerCooldown) return;
        if (other.tag == "Ball Catcher")
        {
            transform.parent = other.gameObject.transform;
            GetComponent<Rigidbody>().isKinematic = true;
            catcher = false;
            transform.DOLocalMoveY(0.3f, 0.25f);
            other.transform.DORotate(new Vector3(0, -75, 0), 1).OnComplete(BallToTheBases);
            
        }
        if (other.CompareTag("BallOut") && !scoredThisRound)
    {
        ScoreForRedTeam();
        scoredThisRound = true;
    }
    if (other.tag == "Final Point" && !roundTriggered)
    {
        OnBallReachedFinalBase();
        lastTriggerTime = Time.time;
        IncrementRound();
    }
    }
    void ScoreForRedTeam()
{
    redTeamScore++; // Increment the blue team score
    UpdateScoreUI();
    IncrementRound(); // Update the UI to reflect the new score
    // Any other logic you want to execute when blue team scores
}

    void BallToTheBases()
    {
        transform.parent = null;
        transform.DOMove(baseGuy1.position, 1.5f);
        transform.DOMove(baseGuy2.position, 1.5f).SetDelay(1.5f);
        transform.DOMove(baseGuy3.position, 1.5f).SetDelay(3.0f);
        transform.DOMove(baseGuy4.position, 1.5f).SetDelay(4.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))  // Press 'R' to reset the ball
    {
        ResetBall();
    }
        if (Input.GetKeyDown(KeyCode.Space) && !isThrowed)
        {
            isThrowed = true;
            FirstThrow();
        }

        if (catcher)
        {
            Debug.Log("Setting ball catcher's destination.");
            ballCatcher.GetComponent<NavMeshAgent>().destination = transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isThrowed)
    {
        isThrowed = true;
        FirstThrow();
    }

    if (catcher)
    {
        Debug.Log("Setting ball catcher's destination.");
        ballCatcher.GetComponent<NavMeshAgent>().destination = transform.position;
    }
    }
    
public void IncrementRound()
{
    // Increment the round number
    currentRound++;

    if (currentRound <= maxRounds)
    {
        UpdateRoundText();
        ResetRound();
    }
    else
    {
        EndGame();
    }
}


void UpdateRoundText()
{
    if (roundText != null)
    {
        roundText.text = "Round: " + currentRound;
    }
}
void ResetRound()
{
    ResetBall();
    strikerMovement.ResetPosition();
    ResetBallCatcher();

    isThrowed = false;
    catcher = false;
    roundTriggered = false; 

    Debug.Log("Round " + currentRound + " starts");
    ballReachedFinal = false;
    strikerReachedFinal = false;
    scoredThisRound = false;
}

void EndGame()
{
    gameUI.SetActive(false);
    strikerMovement.NotStartRunning();
    StopBallAndResetPosition();
    Debug.Log("Game over after " + maxRounds + " rounds.");
    // Implement any end game logic here
    gameOverPanel.SetActive(true);
    Invoke(nameof(BallCatcher), 0);
    StopBallCatcherMovement();
}
void ResetBall()
{
    Rigidbody rb = GetComponent<Rigidbody>();

    // Make sure the Rigidbody is not kinematic before setting its velocity
    rb.isKinematic = false;

    // Now reset the position and velocity
    transform.position = new Vector3(5.98f, 0.57f, 6.11f);
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    // If you need the Rigidbody to be kinematic after resetting it, 
    // you can set it back to kinematic here
    rb.isKinematic = true;

    isThrowed = false;
}

void ResetBallCatcher()
{
    // Set the ball catcher's position to its initial position
    Vector3 ballCatcherStartPosition = new Vector3(-12.98f, 0.46f, -15.21f);
    ballCatcher.transform.position = ballCatcherStartPosition;

    // If there are other properties or states of the ball catcher that need resetting, reset them here
    // For example, resetting velocity or any other dynamic properties
    Rigidbody rbCatcher = ballCatcher.GetComponent<Rigidbody>();
    if (rbCatcher != null)
    {
        rbCatcher.velocity = Vector3.zero;
        rbCatcher.angularVelocity = Vector3.zero;
    }

    NavMeshAgent agent = ballCatcher.GetComponent<NavMeshAgent>();
    if (agent != null)
    {
        agent.ResetPath(); // This clears the current path
        agent.SetDestination(ballCatcherStartPosition);
    }

    // Additional logic if the ball catcher needs to be reactivated or its state changed
}


public void CheckScore()
{
    if (ballReachedFinal)
    {
        blueTeamScore++;
        UpdateScoreUI();
    }
    else if (strikerReachedFinal)
    {
        redTeamScore++;
        UpdateScoreUI();
    }

    // Reset flags for the next round
    ballReachedFinal = false;
    strikerReachedFinal = false;
}

void UpdateScoreUI()
{
    blueTeamScoreText.text = "Blue Team: " + blueTeamScore;
    redTeamScoreText.text = "Red Team: " + redTeamScore;
    blueTeamFinalScoreText.text = "Blue Team: " + blueTeamScore;
    redTeamFinalScoreText.text = "Red Team: " + redTeamScore;
}

void OnBallReachedFinalBase()
{
    ballReachedFinal = true;
    CheckScore();
}


public void OnStrikerReachedFinalBase()
{
    strikerReachedFinal = true;
    CheckScore();
}
public void StopBallAndResetPosition()
{
    // Stop any ongoing movement
    transform.DOKill();

    // Reset the ball's position to its starting position
    transform.position = new Vector3(0.6519995f, -0.06f, 0.09599972f); // Replace with your actual start position

    // Reset Rigidbody's velocity and angular velocity
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
void StopBallCatcherMovement()
{
    NavMeshAgent agent = ballCatcher.GetComponent<NavMeshAgent>();
    if (agent != null)
    {
        agent.ResetPath(); // This will clear the current path of the NavMeshAgent
        agent.enabled = false; // Optionally, disable the NavMeshAgent component
    }
}
}
