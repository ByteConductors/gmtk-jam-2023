using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    [SerializeField]
    private GameObject SoundEmitterPrefab;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] public enum SoundType
    {
        MASTER,
        MUSIC,
        SFX,
        UI,
        ENVIRONMENT
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /*
        * Play a sound at a given position
        * @param clip: the clip to play
        * @param gameObject: the gameObject to attach the sound to
        * @param loop: whether the sound should loop or not. If true, the sound will not be destroyed when the clip is finished!!
        * @param volume: the volume of the sound

    */
    public SoundEmitter PlaySound(AudioClip clip, GameObject gameObject, bool loop, float volume)
    {
        if (clip == null)
        {
            Debug.LogError("No clip found");
            return null;
        }
        if (gameObject == null)
        {
            Debug.LogError("No gameObject found");
            return null;
        }
        GameObject soundEmitter = Instantiate(SoundEmitterPrefab, gameObject.transform);

        SoundEmitter soundEmitterComponent = soundEmitter.GetComponent<SoundEmitter>();
        AudioSource audioSource = soundEmitter.GetComponent<AudioSource>();

        soundEmitterComponent.loop = loop;
        soundEmitterComponent.audioSource = audioSource;


        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();

        return soundEmitterComponent;
    }

    public void SetVolume(SoundType soundType, float volume)
    {
        switch (soundType)
        {
            case SoundType.MASTER:
                audioMixer.SetFloat("MasterVol", volume);
                break;
            case SoundType.MUSIC:
                audioMixer.SetFloat("MusicVol", volume);
                break;
            case SoundType.SFX:
                audioMixer.SetFloat("SFXVol", volume);
                break;
            case SoundType.UI:
                audioMixer.SetFloat("UIVol", volume);
                break;
            case SoundType.ENVIRONMENT:
                audioMixer.SetFloat("EnvironmentVol", volume);
                break;
        }
    }
}
