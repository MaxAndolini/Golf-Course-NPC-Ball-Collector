using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private Text pointsText; // UI text to display points
    [SerializeField] private float animationDuration = 0.5f; // Animation süresi

    private int _totalPoints;

    // Reset function to clear points and update UI
    public void Reset()
    {
        _totalPoints = 0; // Reset total points to 0
        pointsText.text = _totalPoints.ToString(); // Update the UI text to reflect the reset points
    }

    // Function to add points with animation
    public void AddPoints(int points)
    {
        var startPoints = _totalPoints;
        _totalPoints += points;

        GameManager.Instance.AudioManager.PlaySfx("Gain");

        // Start the coroutine to animate the points increase
        StartCoroutine(AnimatePointsIncrease(startPoints, _totalPoints));
    }

    // Coroutine to smoothly increase points over time
    private IEnumerator AnimatePointsIncrease(int startPoints, int endPoints)
    {
        var elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / animationDuration); // Progress between 0 and 1

            // Calculate current points using Lerp (smooth interpolation)
            var currentPoints = Mathf.RoundToInt(Mathf.Lerp(startPoints, endPoints, t));
            pointsText.text = currentPoints.ToString();

            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set correctly after animation ends
        pointsText.text = endPoints.ToString();
    }
}