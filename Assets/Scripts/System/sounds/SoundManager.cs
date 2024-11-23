using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Player;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set;}
    [SerializeField]
    PlayerController player;
    public Sound[] Sounds;
    public AudioSource[] _tracks;

    public string PlayingMusic;

    void Start() {
        
        Instance = this;
    }
    public void playTo(string musicId, float x, float y, float distance, float pitch = 0) {
        if (Vector2.Distance(player.transform.position, new Vector2(x, y)) <= distance) {
            Play(musicId, true, pitch);
        }
    }

    public void Play(string musicId, bool seper = false, float pitch = 0)
    {
        Sound sound = Array.Find(Sounds, v => v.id == musicId);
        if (sound == null) return;

        StartCoroutine(OnPlay(sound, seper, pitch));
    }
    public void Stop(int track)
    {
        _tracks[track - 1].Stop();
        PlayingMusic = null;
    }

    public void Pause(int track)
    {
        _tracks[track - 1].Pause();
    }

    public void UnPause(int track)
    {
        if (_tracks[track - 1]) _tracks[track - 1].UnPause();
    }

    public IEnumerator OnPlay(Sound sound, bool seper, float pitch)
    {
        if (sound.track == 4)
        {
            if (PlayingMusic != null && PlayingMusic == sound.id) yield break;
            else PlayingMusic = sound.id;
        }
        AudioSource _audio = _tracks[sound.track - 1];

        if (seper) {
            _audio = Instantiate(_audio);
            Destroy(_audio.gameObject, sound.audio.length);
        }

        if (sound.audioIn)
        {
            _audio.clip = sound.audioIn;
            _audio.loop = false;
            _audio.volume = sound.volume;
            _audio.pitch = sound.pitch;
            _audio.time = sound.startTime;

            if (pitch != 0) {
                _audio.pitch = pitch;
            }
            _audio.Play();

            while (true)
            {

                yield return new WaitForSecondsRealtime(0.01f);
                if (!_audio.isPlaying)
                {
                    _audio.clip = sound.audio;
                    _audio.Play();
                    _audio.loop = sound.loop;

                    break;
                }
            }
        }
        else
        {
            _audio.clip = sound.audio;
            _audio.loop = sound.loop;
            _audio.volume = sound.volume;
            _audio.pitch = sound.pitch;
            _audio.time = sound.startTime;

            if (pitch != 0) {
                _audio.pitch = pitch;
            }
            
            _audio.Play();
        }
    }
}
