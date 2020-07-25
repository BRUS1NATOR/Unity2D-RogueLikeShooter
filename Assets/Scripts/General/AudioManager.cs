using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum MusicType { main, battle, shop, ambient, none}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip bombExplClip;

    public List<AudioSource> liveSources;

    public List<AudioClip> music;

    public List<AudioClip> battleMusic;
    public List<AudioClip> shopMusic;

    public List<AudioClip> ambients;

    private float maxVolume;
    private Coroutine transitionCor;

    MusicType musicTypeNow;
    bool isMusic = true;

    private void Awake()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
        maxVolume = musicSource.volume;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayExplosion()
    {
        sfxSource.PlayOneShot(bombExplClip);
    }

    public void AddLiveSource(AudioSource s)
    {
        liveSources.Add(s);
    }

    public void RemoveLiveSource(AudioSource s)
    {
        liveSources.Remove(s);
    }

    public void PauseLiveSources()
    {
        foreach (AudioSource s in liveSources)
        {
            s.Pause();
        }
        sfxSource.Pause();
    }

    public void UnPauseLiveSources()
    {
        foreach (AudioSource s in liveSources)
        {
            s.UnPause();
        }
        sfxSource.UnPause();
    }

    public void PlayMusic(MusicType musicType)
    {
        if (transitionCor != null)
        {
            StopCoroutine(transitionCor);
        }
        transitionCor = StartCoroutine(AudioTransition(musicType));
    }

    IEnumerator AudioTransition(MusicType musicType)
    {
        if (musicTypeNow != musicType)      //Если музыка не играла или сменяется тип мелодии
        {
            yield return FadeVolume();
            switch (musicType)
            {
                case MusicType.main:
                    {
                        musicSource.clip = GetRandomClip(music);
                        musicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
                        break;
                    }
                case MusicType.battle:
                    {
                        musicSource.clip = GetRandomClip(battleMusic);
                        musicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
                        break;
                    }
                case MusicType.shop:
                    {
                        musicSource.clip = GetRandomClip(shopMusic);
                        musicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
                        break;
                    }
                case MusicType.ambient:
                    {
                        musicSource.clip = GetRandomClip(ambients);
                        musicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Ambient")[0];
                        break;
                    }
            }
        }
        musicTypeNow = musicType;
        Play();
        yield return IncreaseVolume();
        StopCoroutine(transitionCor);
    }

    IEnumerator FadeVolume()
    {
        while (musicSource.volume > 0)
        {
            musicSource.volume -= 0.025f;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator IncreaseVolume()
    {
        while (musicSource.volume < maxVolume)
        {
            musicSource.volume += 0.025f;
            yield return new WaitForSeconds(0.125f);
        }
        musicSource.volume = maxVolume;
    }

    private void Play()
    {
        if (musicSource.isPlaying == false)
        {
            musicSource.Play();
        }
    }

    private AudioClip GetRandomClip(List<AudioClip> clips)
    {
        int rnd = Random.Range(0, clips.Count);
        return clips[rnd];
    }
}
