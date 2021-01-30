using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  public PlayerManager player;

  public float movementSpeed = 750f;
  public float damping = 1f;
  public bool moveUsingPlayerRotation = false;

  private Vector3 targetPosition = Vector3.zero;
  private Vector3 velocity = Vector3.zero;

  private Rigidbody rigidbody;

  void Start()
  {
    player.GetActions().Player.Move.performed += OnMoveStarted;
    player.GetActions().Player.Move.canceled += OnMoveCanceled;

    rigidbody = GetComponent<Rigidbody>();
  }

  void Update()
  {
    var translate = targetPosition * movementSpeed;

    if (moveUsingPlayerRotation)
    {
      translate = player.GetCharacter().transform.rotation * translate;
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
}
