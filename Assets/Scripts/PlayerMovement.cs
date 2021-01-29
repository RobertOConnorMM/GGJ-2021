using UnityEngine;

public class InputStrategy : MonoBehaviour
{

}

public class PlayerMovement : MonoBehaviour
{
  public float movementSpeed = 750f;
  public float damping = 1f;

  private Vector3 targetPosition = Vector3.zero;
  private Vector3 velocity = Vector3.zero;

  // Update is called once per frame
  void Update()
  {
    targetPosition = Vector3.zero;

    // TODO Choose strategy
    if (Input.GetKey(KeyCode.W))
    {
      targetPosition += Vector3.forward;
    }

    if (Input.GetKey(KeyCode.A))
    {
      targetPosition += Vector3.left;
    }

    if (Input.GetKey(KeyCode.S))
    {
      targetPosition += Vector3.back;
    }

    if (Input.GetKey(KeyCode.D))
    {
      targetPosition += Vector3.right;
    }

    transform.position = Vector3.SmoothDamp(
      transform.position,
      transform.position + (targetPosition.normalized * movementSpeed * Time.deltaTime),
      ref velocity,
      damping
    );
  }
}
