using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{

    public bool loop;

    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if (!loop && audioSource.time >= audioSource.clip.length)
        {
            Destroy(this.gameObject);
        }
    }
}
