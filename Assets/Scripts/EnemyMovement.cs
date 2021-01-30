using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

  public float movementSpeed = 200f;
  public float damping = 1f;
  public float health = 3f;

  private float currentHealth;

  private Transform target;
  private Rigidbody body;
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip hurtSound;

  private bool isDead
  {
    get => health < 1;
  }

  void Start()
  {
    body = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
    PlayerManager playerManager = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
    target = playerManager.getTransform();

    currentHealth = health;
  }

  void Update()
  {
    float step = movementSpeed * Time.deltaTime;
    body.transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    body.transform.LookAt(target);
  }

  public void OnHit(float damage)
  {
    Debug.Log(damage);

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

      Debug.Log(rigidbody.velocity.x);
      Debug.Log(rigidbody.velocity.y);
      Debug.Log(rigidbody.velocity.magnitude);

      if (Mathf.Abs(rigidbody.velocity.magnitude) < 4f)
      {
        return;
      }

      weaponItem.OnHit(1f);
      OnHit(1f);
    }
  }
}