using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
  public PlayerManager playerManager;
  private PlayerSpeech playerSpeech;
  public new Rigidbody rigidbody;
  public float movementSpeed = 25f;
  public bool moveUsingPlayerRotation;
  private bool isWalking = false;

  private Vector3 targetPosition = Vector3.zero;


  void Start()
  {
    playerManager.GetActions().Player.Move.performed += OnMoveStarted;
    playerManager.GetActions().Player.Move.canceled += OnMoveCanceled;
    playerManager.GetActions().Player.Pause.performed += OnPausePress;
    playerSpeech = GetComponentInParent<PlayerSpeech>();
  }


  void Update()
  {
    var translate = targetPosition * movementSpeed * Time.deltaTime;

    if (moveUsingPlayerRotation)
    {
      translate = playerManager.GetCharacter().transform.rotation * translate;
    }

    rigidbody.AddForce(translate, ForceMode.VelocityChange);

    if(targetPosition.x != 0 || targetPosition.z != 0) {
      if(!isWalking) {
        StartCoroutine(PlayFootstepSound());
        isWalking = true;
      }
    } else {
      isWalking = false;
    }
  }

  private IEnumerator PlayFootstepSound()
  {
    yield return new WaitForSeconds(0.4f);
    playerSpeech.PlayFootstep();
    if(isWalking) {
      StartCoroutine(PlayFootstepSound());
    }
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
