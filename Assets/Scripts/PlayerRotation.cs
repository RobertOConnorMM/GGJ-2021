using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
  private PlayerManager player;

  public float rotationSpeed = 5f;

  void Start()
  {
    player = GetComponent<PlayerManager>();
    //player.GetActions().Player.MousePosition.performed += OnMousePosition;
    //player.GetActions().Player.Look.performed += OnLook;
  }


  // Update is called once per frame
  private void OnLook(InputAction.CallbackContext context)
  {
    Debug.Log(context);
  }

  private void OnMousePosition(InputAction.CallbackContext context)
  {
    Debug.Log(context);

    var plane = new Plane(Vector3.up, transform.position);
    var ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());

    if (plane.Raycast(ray, out var hitDistance))
    {
      var targetPoint = ray.GetPoint(hitDistance);
      var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

      targetRotation.x = 0;
      targetRotation.z = 0;

      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
  }
}
