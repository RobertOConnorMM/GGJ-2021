using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  public PlayerManager playerManager;
  public new Rigidbody rigidbody;
  public float movementSpeed = 1f;
  public bool moveUsingPlayerRotation;

  private Vector3 targetPosition = Vector3.zero;


  void Start()
  {
    playerManager.GetActions().Player.Move.performed += OnMoveStarted;
    playerManager.GetActions().Player.Move.canceled += OnMoveCanceled;
    playerManager.GetActions().Player.Pause.performed += OnPausePress;
  }


  void Update()
  {
    var translate = targetPosition * movementSpeed;

    if (moveUsingPlayerRotation)
    {
      translate = playerManager.GetCharacter().transform.rotation * translate;
    }

    rigidbody.AddForce(translate, ForceMode.VelocityChange);
  }

  private void OnMoveStarted(InputAction.CallbackContext context)
  {
    var input = context.ReadValue<Vector2>();

    targetPosition.x = input.x;
    targetPosition.z = input.y;
  }

  private void OnMoveCanceled(InputAction.CallbackContext context)
  {
    targetPosition = Vector2.zero;
  }

  private void OnPausePress(InputAction.CallbackContext context)
  {
    PauseManager.Instance.OnPause();
  }
}
