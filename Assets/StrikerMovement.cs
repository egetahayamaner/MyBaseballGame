using Unity.VisualScripting;
using UnityEngine;

public class StrikerMovement : MonoBehaviour
{
    public Ball ball;
    public Transform[] bases;
    private int currentBaseIndex = 0;
    public float speed = 5.0f;
    public bool shouldRun = false;


    public Rigidbody strikerRigidbody;

void Start() {
    strikerRigidbody.isKinematic = true;
}

    void Update()
    {
        if (shouldRun)
        {
            MoveToNextBase();
        }
    }

    public void StartRunning()
    {
        Debug.Log("StartRunning called");
        shouldRun = true;
        currentBaseIndex = 0; // Reset to start from the first base
    }
    public void NotStartRunning()
    {
        shouldRun = false;
        currentBaseIndex = 0; // Reset to start from the first base
    }

    void MoveToNextBase()
    {
        if (bases.Length == 0 || currentBaseIndex >= bases.Length) return;

        Transform targetBase = bases[currentBaseIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetBase.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetBase.position) < 0.1f)
        {
            currentBaseIndex++;
            if (currentBaseIndex >= bases.Length)
            {
                shouldRun = false; // Stop running after last base
                currentBaseIndex = 0; // Reset for next run
            }   
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Final Point"))
        {
            // Notify the Ball script that the striker has reached the final point
            ball.StopBallAndResetPosition();
            if (ball != null)
        {
            ball.OnStrikerReachedFinalBase();
            ball.IncrementRound();
        }
        }
        
    }
    public void ResetPosition()
{
    transform.position = new Vector3(9.929f, 0.54f, 9.965f);
    currentBaseIndex = 0;
    shouldRun = false;
}
// This method should be called when the striker reaches its final destination
}
