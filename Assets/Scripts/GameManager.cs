using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance to allow global access to the GameManager
    public static GameManager Instance;

    // References to various managers in the game, assigned via the Inspector
    [SerializeField] private BallManager ballManager;
    [SerializeField] private PointsManager pointsManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private MenuController menuController;
    [SerializeField] private AudioManager audioManager;

    // A reference to the NPCController, which will be assigned dynamically

    // Properties to access the various managers from other scripts
    public BallManager BallManager => ballManager; // Access BallManager
    public PointsManager PointsManager => pointsManager; // Access PointsManager
    public HealthManager HealthManager => healthManager; // Access HealthManager
    public MenuController MenuController => menuController; // Access MenuController
    public AudioManager AudioManager => audioManager; // Access AudioManager
    public NPCController NPCController { get; private set; }

    private void Awake()
    {
        // If no instance exists, make this the active instance
        if (Instance == null)
        {
            // Find the GameObject tagged as "Player"
            var player = GameObject.FindGameObjectWithTag("Player");

            // If a player GameObject is found, get its NPCController component
            if (player != null) NPCController = player.GetComponent<NPCController>();

            // Assign this GameManager instance as the singleton
            Instance = this;

            // Prevent this GameManager from being destroyed when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one to enforce the singleton pattern
            Destroy(gameObject);
        }
    }
}