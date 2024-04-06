using UnityEngine;

public sealed class InitCurrentGameState : MonoBehaviour
{
    void Awake() => CurrentGameState.Init();
}
