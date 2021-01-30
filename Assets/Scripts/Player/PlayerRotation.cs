using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
  public PlayerManager player;
  public float rotationSpeed = 2.75f;
  public float time = 1.175f;

  private Quaternion deltaRotation;
  private Quaternion targetRotation;
  private float velocity;

  void Start()
  {
    player.GetActions().Player.MousePosition.performed += OnMousePositionPerformed;
    player.GetActions().Player.Look.performed += OnLookPerformed;
    player.GetActions().Player.Look.canceled += OnLookCanceled;
  }

  void Update()
  {
    var useDeltaRotation = !deltaRotation.Equals(Quaternion.identity);

    if (useDeltaRotation)
    {
      transform.rotation = Quaternion.Slerp(
        transform.rotation,
        transform.rotation * deltaRotation,
        rotationSpeed * Time.deltaTime
      );

      return;
    }

    var delta = Quaternion.Angle(transform.rotation, targetRotation) * Time.deltaTime;

    if (delta == 0f)
    {
      return;
    }

    // Use angle to smooth damp the rotation
    var _time = Mathf.SmoothDampAngle(delta, 0, ref velocity, time);
    _time = 1 - (_time / delta);

    transform.rotation = Quaternion.Slerp(
      transform.rotation,
      targetRotation,
      _time
    );
  }

  private void OnLookPerformed(InputAction.CallbackContext context)
  {
    var input = context.ReadValue<Vector2>();

    deltaRotation = Quaternion.Euler(Vector3.zero);
    deltaRotation.y = input.x;
  }

  private void OnLookCanceled(InputAction.CallbackContext context)
  {
    targetRotation = transform.rotation;
    deltaRotation = Quaternion.identity;
  }

  private void OnMousePositionPerformed(InputAction.CallbackContext context)
  {
    deltaRotation = Quaternion.identity;

    var plane = new Plane(Vector3.up, transform.position);
    var ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());

    if (plane.Raycast(ray, out var hitDistance))
    {
      var targetPoint = ray.GetPoint(hitDistance);

      targetRotation = Quaternion.LookRotation(
        targetPoint - transform.position
      );

      targetRotation.x = 0;
      targetRotation.z = 0;
    }
  }

}
