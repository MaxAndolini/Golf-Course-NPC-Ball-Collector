using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    [SerializeField] private int level; // Level of the ball (1, 2, or 3)
    [SerializeField] private int points; // Points associated with this ball

    // Dictionary to store points and colors for each level
    private readonly Dictionary<int, (int points, Color color)> _levelData =
        new()
        {
            { 1, (10, Color.green) }, // Level 1: 10 points, green color
            { 2, (20, Color.yellow) }, // Level 2: 20 points, yellow color
            { 3, (30, Color.red) } // Level 3: 30 points, red color
        };

    private MeshRenderer _meshRenderer; // Cached MeshRenderer

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        AssignPointsAndColor();
    }

    private void AssignPointsAndColor()
    {
        if (_levelData.TryGetValue(level, out var data))
        {
            points = data.points;
            SetBallColor(data.color);
        }
        else
        {
            points = 0;
            Debug.LogWarning($"Invalid level {level} assigned!");
        }
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Clamp(newLevel, 1, 3); // Ensure the level is valid
        AssignPointsAndColor(); // Recalculate points and color based on new level
    }

    public int GetPoints()
    {
        return points;
    }

    private void SetBallColor(Color color)
    {
        if (_meshRenderer != null)
        {
            var ballMaterial = _meshRenderer.material;
            if (ballMaterial != null) ballMaterial.SetColor("_BaseColor", color); // Set the material color
        }
    }
}