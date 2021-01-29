using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  public PlayerManager player;

  public float movementSpeed = 750f;
  public float damping = 1f;

  private Vector3 targetPosition = Vector3.zero;
  private Vector3 velocity = Vector3.zero;

  void Start()
  {
    player = GetComponent<PlayerManager>();
    player.GetActions().Player.Move.performed += OnMoveStarted;
    player.GetActions().Player.Move.canceled += OnMoveCanceled;
  }

  // Update is called once per frame
  void Update()
  {
    transform.position = Vector3.SmoothDamp(
      transform.position,
      transform.position + (targetPosition * movementSpeed * Time.deltaTime),
      ref velocity,
      damping
    );
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
