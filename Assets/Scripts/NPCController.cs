using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform golfCart; // Position of the golf cart
    public bool isStopped = true; // Checks if the NPC is stopped

    private NavMeshAgent _agent; // Reference to the NavMeshAgent
    private Animator _animator; // Reference to the Animator component

    private GameObject _currentBall; // The ball the NPC is collecting
    private GolfBall _currentGolfBall; // Stores the GolfBall component of the collected ball
    private bool _hasBall; // Checks if the NPC is carrying a ball

    public void Reset()
    {
        _hasBall = false; // Reset the state to not having a ball
        _currentBall = null; // Clear the current ball reference
        Stop(true); // Make the NPC in idle position

        // Reset health and points
        GameManager.Instance.HealthManager.Reset(); // Call the Reset function in HealthManager
        GameManager.Instance.PointsManager.Reset(); // Call the Reset function in PointsManager

        // Set the NPC's position and rotation
        transform.position = new Vector3(253.949997f, 17.6229687f, 195.300003f); // Set position
        transform.rotation = Quaternion.Euler(0, 270, 0); // Set rotation
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // Cache the NavMeshAgent component
        _animator = GetComponent<Animator>(); // Cache the Animator component
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the NPC has entered the trigger area of a golf ball
        if (other.CompareTag("Golf Ball") && !_hasBall && other.gameObject == _currentBall)
        {
            Stop(true);
            StartCoroutine(GatherBall());
        }

        // Check if the NPC has entered the trigger area of the golf cart
        if (other.CompareTag("Golf Cart") && _hasBall)
        {
            Stop(true);
            StartCoroutine(PutBallInCart());
        }
    }

    public void MoveToBestBall()
    {
        GameObject bestBall = null;
        var minDistance = Mathf.Infinity;
        float bestBallLevel = -1; // Initialize to an impossible level
        var golfBalls = GameManager.Instance.BallManager.GetActiveSpawnedBalls(); // Get only active balls

        if (golfBalls.Count == 0)
        {
            GameOver();
            return;
        }

        // Evaluate the best ball based on health and distance
        foreach (var ball in golfBalls)
        {
            var golfBallComponent = ball.GetComponent<GolfBall>();
            if (golfBallComponent is null) continue;

            var dist = Vector3.Distance(transform.position, ball.transform.position);
            float ballLevel = golfBallComponent.GetLevel();

            // Determine if the ball is a better choice based on health and level
            var prefersHigherLevel =
                GameManager.Instance.HealthManager.GetHealth() > 50; // Example threshold, adjust as needed

            if (prefersHigherLevel)
            {
                // If health is high, prefer higher-level balls regardless of distance
                if (ballLevel > bestBallLevel)
                {
                    bestBallLevel = ballLevel;
                    bestBall = ball;
                }
            }
            else
            {
                // If health is sufficient, prefer the closest ball
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestBall = ball;
                    bestBallLevel = ballLevel; // Track the level of the closest ball
                }
            }
        }

        if (bestBall is not null)
        {
            _currentBall = bestBall;
            _currentGolfBall = _currentBall.GetComponent<GolfBall>();
            Stop(false);
            _agent.SetDestination(_currentBall.transform.position);
        }
    }

    private IEnumerator GatherBall()
    {
        _animator.SetTrigger("Gather");
        yield return new WaitForSeconds(2); // Simulate gather time

        _hasBall = true;
        _currentBall.SetActive(false); // Disable the collected ball

        Stop(false);
        _agent.SetDestination(golfCart.position); // Move to the cart
    }

    private IEnumerator PutBallInCart()
    {
        _animator.SetTrigger("Put");
        yield return new WaitForSeconds(2); // Simulate gather time

        // Add the points from the collected ball
        GameManager.Instance.PointsManager.AddPoints(_currentGolfBall.GetPoints());

        _hasBall = false; // NPC no longer has a ball
        MoveToBestBall(); // Find the next ball
    }

    public void Stop(bool active)
    {
        isStopped = active;
        _agent.ResetPath();
        _animator.SetBool("Running", !active);
    }

    public void GameOver()
    {
        Stop(true);
        GameManager.Instance.MenuController.GameOver();
    }
}