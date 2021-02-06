using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    const float volScale = 2.6f;
    const int volRadius = 18;
    const int panRadius = 16;

    public AudioSource audioSource;
    public Transform player;

    public Slider volumeSlider;
    public Text volumeText;

    public AudioClip doorOpened;
    public AudioClip doorClosed;
    public AudioClip botHit;
    public AudioClip eat;
    public AudioClip fish;
    public AudioClip meatCooked;
    public AudioClip cannonBallFired;
    public AudioClip pop;
    public AudioClip bell;

    public float volume;

    private void Start()
    {
        SetVolume();
    }

    public void PlaySound(AudioClip sound, Vector3 soundPos)
    {
        if (sound != null)
        {
            // Make volume louder the closer the sound is to player.
            float vol = 1 - (Vector2.Distance(soundPos, player.position) / volRadius);
            // Make the sound come from the direction on the x axis the position is.
            float pan = (soundPos.x - player.position.x) / panRadius;
            audioSource.panStereo = pan;

            if (vol > 0)
            {
                audioSource.PlayOneShot(sound, vol);
            }
        }
    }

    public void SetVolume()
    {
        volumeText.text = "VOLUME: " + volumeSlider.value;
        volume = volumeSlider.value;
        AudioListener.volume = (volume / 100.0f) * volScale;
    }

    public void PlayBells()
    {
        if (Time.deltaTime > 0.5f)
        {
            PlaySound(bell, player.position);
        }
    }
}
