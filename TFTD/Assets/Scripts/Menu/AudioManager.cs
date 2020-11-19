using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const string MainMenuTheme = "MainMenu";

    private static readonly List<string> _geoscapeMusicNames = new List<string>
    {
        "Geosphere01"
    };

    private static readonly List<string> _combatThemeNames = new List<string>
    {
        "CombatAmbient01"
    };

    private static readonly List<string> _combatAmbientNames = new List<string>
    {
        "CombatAmbient01"
    };

    private string _lastPlayedGeoscapeMusic;
    private string _lastPlayedCombatTheme;
    private string _lastPlayedCombatAmbient;

    private string _currentMusic;

    [SerializeField] private AudioSource audioSource;

    public ActiveMusicType MusicType { get; private set; }

    void Awake()
    {
        PlayMenuAudio();
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        ContinuePlayingMusic();
    }

    private void ContinuePlayingMusic()
    {
        switch (MusicType)
        {
            case ActiveMusicType.MainMenu:
                PlayMenuAudio();
                break;
            case ActiveMusicType.Geoscape:
                PlayGeoscapeMusic();
                break;
            case ActiveMusicType.CombatAmbient:
                PlayCombatAmbient();
                break;
            case ActiveMusicType.CombatTheme:
                PlayCombatTheme();
                break;
        }
    }

    public void PlayMenuAudio()
    {
        MusicType = ActiveMusicType.MainMenu;
        _currentMusic = MainMenuTheme;
        SetAudioClipToSource(MainMenuTheme);
        PlayMusic();
    }

    public void PlayGeoscapeMusic()
    {
        MusicType = ActiveMusicType.Geoscape;
        _lastPlayedGeoscapeMusic = GetRandomMusicName(_geoscapeMusicNames, _lastPlayedGeoscapeMusic);
        _currentMusic = _lastPlayedGeoscapeMusic;
        SetAudioClipToSource(_currentMusic);
        PlayMusic();
    }

    public void PlayCombatAmbient()
    {
        MusicType = ActiveMusicType.CombatAmbient;
        _lastPlayedCombatAmbient = GetRandomMusicName(_combatAmbientNames, _lastPlayedCombatAmbient);
        _currentMusic = _lastPlayedCombatAmbient;
        SetAudioClipToSource(_currentMusic);
        PlayMusic();
    }

    public void PlayCombatTheme()
    {
        MusicType = ActiveMusicType.CombatTheme;
        _lastPlayedCombatTheme = GetRandomMusicName(_combatThemeNames, _lastPlayedCombatTheme);
        _currentMusic = _lastPlayedCombatTheme;
        SetAudioClipToSource(_currentMusic);
        PlayMusic();
    }

    private string GetRandomMusicName(List<string> musicNames, string lastPlayedMusicName)
    {
        var random = new System.Random();
        int index = random.Next(0, musicNames.Count());
        while (musicNames.ElementAt(index) == lastPlayedMusicName)
        {
            index = random.Next(0, musicNames.Count());
        }
        return musicNames.ElementAt(index);
    }

    private void SetAudioClipToSource(string audioFileName)
    {
        var clip = LoadBackgroundAudioClip(audioFileName);
        audioSource.clip = clip;
    }

    private void PlayMusic()
    {
        audioSource.Play();
    }

    private AudioClip LoadBackgroundAudioClip(string fileName)
    {
        return Resources.Load("Music/" + fileName) as AudioClip;
    }
}
