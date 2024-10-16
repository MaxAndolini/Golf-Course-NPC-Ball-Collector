using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuMusic; // Music for the main menu
    [SerializeField] private AudioClip inGameMusic; // Music for in-game

    [Tooltip("Sound effects list. Add new sounds and assign names.")] [SerializeField]
    private List<SoundEffect> soundEffects = new(); // List of sound effects

    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.2f; // Lower volume for background music
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 0.7f; // Higher volume for sound effects

    private AudioSource _musicSource; // Audio source for background music

    private Dictionary<string, AudioClip> _sfxDictionary; // To quickly access sound effects by name
    private AudioSource _sfxSource; // Audio source for sound effects

    private void Awake()
    {
        // Create audio sources
        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();

        // Apply initial volume settings
        _musicSource.volume = musicVolume;
        _sfxSource.volume = sfxVolume;

        // Initialize the sound effects dictionary
        InitializeSfxDictionary();
    }

    // Initialize the dictionary with sound effect names and clips
    private void InitializeSfxDictionary()
    {
        _sfxDictionary = new Dictionary<string, AudioClip>();

        foreach (var effect in soundEffects.Where(effect => !_sfxDictionary.ContainsKey(effect.name)))
            _sfxDictionary.Add(effect.name, effect.clip);
    }

    // Play Main Menu Music
    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    // Play In-Game Music
    public void PlayInGameMusic()
    {
        PlayMusic(inGameMusic);
    }

    // Play the specified music clip
    private void PlayMusic(AudioClip clip)
    {
        if (clip is null) return;

        _musicSource.clip = clip;
        _musicSource.loop = true; // Set to loop
        _musicSource.Play(); // Play the selected clip
    }

    // Stop the current music
    public void StopMusic()
    {
        _musicSource.Stop();
    }

    // Play a sound effect by name
    public void PlaySfx(string name)
    {
        if (_sfxDictionary.TryGetValue(name, out var clip))
            _sfxSource.PlayOneShot(clip); // Play the sound effect
        else
            Debug.LogWarning($"Sound effect '{name}' not found!");
    }

    // Set the volume of the music
    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
    }

    // Set the volume of the sound effects
    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
    }
}