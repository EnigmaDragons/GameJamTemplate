using UnityEngine;

public class OnVolumeChangedSound : OnMessage<MixerVolumeChanged>
{
    [SerializeField] private AudioClip sound;
    [SerializeField] private AudioSource player;

    private bool _triggered;

    protected override void Execute(MixerVolumeChanged msg)
    {
        _triggered = true;
    }

    private void Update()
    {
        if (!_triggered || Input.GetMouseButton(0)) return;

        _triggered = false;
        player.PlayOneShot(sound);
    }
}