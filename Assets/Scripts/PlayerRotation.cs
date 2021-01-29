using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
  public float rotationSpeed = 5f;

  // Update is called once per frame
  void Update()
  {
    var plane = new Plane(Vector3.up, transform.position);
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
