using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject golfBallPrefab; // Prefab for golf balls
    [SerializeField] private int ballCount = 30; // Number of balls to generate
    [SerializeField] private Transform boundary1; // First boundary corner
    [SerializeField] private Transform boundary2; // Second boundary corner
    [SerializeField] private Transform npcStartPosition; // NPC’s starting point

    private const float SpawnYPosition = 18.0f; // Y position for ball spawning

    private readonly List<GameObject> _spawnedBalls = new(); // Store spawned balls

    public List<GameObject> GetSpawnedBalls()
    {
        return _spawnedBalls;
    }

    public List<GameObject> GetActiveSpawnedBalls()
    {
        return _spawnedBalls.Where(ball => ball.activeSelf).ToList();
    }

    public void SpawnBalls()
    {
        for (var i = 0; i < ballCount; i++)
        {
            var randomPosition = GetRandomPosition();
            randomPosition.y = SpawnYPosition; // Set Y position to 17.8

            // Set rotation with x rotation at -90 degrees
            var rotation = Quaternion.Euler(-90f, 0f, 0f);
            var newBall = Instantiate(golfBallPrefab, randomPosition, rotation);

            // Add the new ball to the list
            _spawnedBalls.Add(newBall);

            // Assign level and change material color based on level
            var golfBall = newBall.GetComponent<GolfBall>();
            var distanceToNpc = Vector3.Distance(randomPosition, npcStartPosition.position);

            switch (distanceToNpc)
            {
                case < 30:
                    golfBall.SetLevel(1); // Easy level
                    break;
                case < 55:
                    golfBall.SetLevel(2); // Medium level
                    break;
                default:
                    golfBall.SetLevel(3); // Hard level
                    break;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Calculate the boundaries based on two corner GameObjects
        var minX = Mathf.Min(boundary1.position.x, boundary2.position.x);
        var maxX = Mathf.Max(boundary1.position.x, boundary2.position.x);

        var minZ = Mathf.Min(boundary1.position.z, boundary2.position.z);
        var maxZ = Mathf.Max(boundary1.position.z, boundary2.position.z);

        // Generate a random position within the boundaries
        var randomX = Random.Range(minX, maxX);
        var randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, 0, randomZ); // Y set to 17.8 in SpawnBalls()
    }

    public void DestroyBalls()
    {
        // Destroy each ball in the list
        foreach (var golfBall in _spawnedBalls)
            if (golfBall != null)
                Destroy(golfBall);

        // Clear the list to remove references to destroyed objects
        _spawnedBalls.Clear();
    }
}