using System.Collections;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject inGame; // Reference to the In Game GameObject
    [SerializeField] private GameObject mainMenu; // Reference to the Main Menu GameObject
    [SerializeField] private GameObject gameOver; // Reference to the Game Over GameObject

    private void Start()
    {
        GameManager.Instance.AudioManager.PlayMainMenuMusic();

        mainMenu.SetActive(true);
        inGame.SetActive(false);
        gameOver.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        GameManager.Instance.AudioManager.PlaySfx("Click");
        GameManager.Instance.BallManager.SpawnBalls();
        GameManager.Instance.NPCController.Stop(true);
        GameManager.Instance.AudioManager.StopMusic();
        GameManager.Instance.AudioManager.PlaySfx("Nice");

        mainMenu.SetActive(false);
        inGame.SetActive(true);
        gameOver.SetActive(false);

        StartCoroutine(DelayedStart());
    }

    private static IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2); // Wait for 2 second

        GameManager.Instance.NPCController.MoveToBestBall(); // Start the ball search
        GameManager.Instance.AudioManager.PlayInGameMusic();
    }

    public void OnExitButtonClicked()
    {
        GameManager.Instance.AudioManager.PlaySfx("Click");

        // Check if running in the editor
        #if UNITY_EDITOR
        // Stop playing in the editor
        EditorApplication.isPlaying = false;
        #else
        // Quit the application if not in the editor
        Application.Quit();
        #endif
    }

    public void OnMenuButtonClicked()
    {
        GameManager.Instance.AudioManager.PlaySfx("Click");
        GameManager.Instance.BallManager.DestroyBalls();
        GameManager.Instance.NPCController.Reset();
        GameManager.Instance.AudioManager.PlayMainMenuMusic();

        mainMenu.SetActive(true);
        inGame.SetActive(false);
        gameOver.SetActive(false);
    }

    public void GameOver()
    {
        GameManager.Instance.AudioManager.StopMusic();
        GameManager.Instance.AudioManager.PlaySfx("Game Over");

        gameOver.SetActive(true);
    }
}