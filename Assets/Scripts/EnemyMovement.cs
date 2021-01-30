using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  public float health = 3f;
  public float movementSpeed = 20f;
  public float knockbackForce = 10f;
  public float knockbackRadius = 5f;
  public float knockbackUpwardsMomentum = 1f;

  private float currentHealth;

  private Transform target;
  private Rigidbody rigidBody;
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip hurtSound;

  private bool isDead
  {
    get => health < 1;
  }

  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
    PlayerManager playerManager = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
    target = playerManager.getTransform();

    currentHealth = health;
  }

  void Update()
  {
    var translate = target.position - (transform.position + Vector3.forward);

    rigidBody.AddForce(
      translate.normalized * movementSpeed * Time.deltaTime,
      ForceMode.VelocityChange
    );

    rigidBody.transform.LookAt(target);
  }

  public void OnHit(float damage)
  {
    if (isDead)
    {
      return;
    }

    currentHealth -= damage;
    audioSource.PlayOneShot(hurtSound, 1f);

    if (currentHealth <= 0)
    {
      Destroy(gameObject);
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Item")
    {
      var weaponItem = collision.gameObject.GetComponent<WeaponItem>();
      var rigidbody = collision.gameObject.GetComponent<Rigidbody>();

      if (Mathf.Abs(rigidbody.velocity.magnitude) < 4f)
      {
        return;
      }

      weaponItem.OnHit(1f);
      OnHit(1f);
    }
  }
}