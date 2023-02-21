using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class BgmController : MonoBehaviour
{
    [SerializeField] SoundHolder _soundHolder;
    AudioSource[] _audioSources;

    [SerializeField] float _oceanLoopVolume = 1;
    [SerializeField] float _seagullLoopVolume = 1;
    [SerializeField] float _bgmVolume = 1;

    void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    void Start()
    {
        var oceanLoop = _audioSources[0];
        oceanLoop.clip = _soundHolder.OceanLoop;
        oceanLoop.volume = _oceanLoopVolume;
        oceanLoop.Play();

        var seagullLoop = _audioSources[1];
        seagullLoop.clip = _soundHolder.SeagullLoop;
        seagullLoop.volume = _seagullLoopVolume;
        seagullLoop.Play();

        var bgmLoop = _audioSources[2];
        bgmLoop.clip = _soundHolder.BgmIntro;
        bgmLoop.volume = _bgmVolume;
        bgmLoop.Play();
    }

    void Update()
    {
        if (!_audioSources[2].isPlaying)
        {
            _audioSources[2].clip = _soundHolder.BgmLoop;
            _audioSources[2].loop = true;
        }
    }
}
