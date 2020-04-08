using UnityEngine;

public sealed class InitGameMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameMusicPlayer musicPlayer;

    private void OnAwake() => musicPlayer.InitIfNeeded(musicSource);
}
