using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static readonly List<string> _backgroundMusicAudioClips = new List<string>
    {
        "Geosphere01",
        "background",
        "level"
    };

    private string _lastPlayedClip = _backgroundMusicAudioClips.ElementAt(0);

    [SerializeField] private AudioSource geoscapeBackgroundMusic;

    void Awake()
    {
        SetAudioClipToSource(_backgroundMusicAudioClips.ElementAt(0));
        PlayMusic();
    }

    void Update()
    {
        if (geoscapeBackgroundMusic.isPlaying)
        {
            return;
        }
        SwitchAudioClip();
        PlayMusic();
    }

    private void SetAudioClipToSource(string audioFileName)
    {
        var clip = LoadBackgroundAudioClip(audioFileName);
        geoscapeBackgroundMusic.clip = clip;
    }

    private void PlayMusic()
    {
        geoscapeBackgroundMusic.Play();
    }

    private AudioClip LoadBackgroundAudioClip(string fileName)
    {
        return Resources.Load("Music/" + fileName) as AudioClip;
    }

    private void SwitchAudioClip()
    {
        for (int i = 1; i < _backgroundMusicAudioClips.Count; i++)
        {
            if (_lastPlayedClip == _backgroundMusicAudioClips.ElementAt(i - 1))
            {
                SetAudioClipToSource(_backgroundMusicAudioClips.ElementAt(i));
            }
        }
    }
}
