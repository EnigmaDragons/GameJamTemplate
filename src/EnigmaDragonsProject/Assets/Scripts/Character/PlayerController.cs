using KinematicCharacterController.Walkthrough.BasicMovement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MyCharacterController Character;
    
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";
    
    private void Update()
    {
        HandleCharacterInput();
    }
    
    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
        characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);

        // Apply inputs to character
        Character.SetInputs(ref characterInputs);
    }
}
