using UnityEngine;
using UnityEngine.Audio;

public sealed class InitAudioVolumeLevel : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string valueName = "MusicVolume";
    [SerializeField] private AudioSource player;
    [SerializeField] private AudioClip demoSound;
    
    private void Start()
    {
        var volume = PlayerPrefs.GetFloat(valueName, 0.5f);
        mixer.SetFloat(valueName, Mathf.Log10(volume) * 20);
        if (player != null && demoSound != null)
            player.PlayOneShot(demoSound, 1f);
    }
}
