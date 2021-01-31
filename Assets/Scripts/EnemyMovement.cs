using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
  public float health = 3f;
  public float movementSpeed = 20f;
  public float knockbackForce = 40f;
  public float knockbackRadius = 5f;
  public float knockbackUpwardsMomentum = -.5f;

  private float currentHealth;

  private Transform target;
  private new Rigidbody rigidbody;
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip hurtSound;

  private NavMeshAgent navMeshAgent;

  private bool isDead
  {
    get => health < 1;
  }

  void Start()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    rigidbody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();

    var playerManager = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
    target = playerManager.getTransform();

    currentHealth = health;
  }

  void Update()
  {
    navMeshAgent.SetDestination(target.position);
    rigidbody.transform.LookAt(target);
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
      WaveManager.Instance.AddEnemyKillCount();
      Destroy(gameObject);
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Item")
    {
      var weaponItem = collision.gameObject.GetComponent<WeaponItem>();
      var itemRigidbody = collision.gameObject.GetComponent<Rigidbody>();

      if (Mathf.Abs(itemRigidbody.velocity.magnitude) < 4f)
      {
        return;
      }

      Knockback(collision);

      weaponItem.OnHit(1f);
      OnHit(1f);
    }

    if (collision.gameObject.tag == "Player")
    {
      Knockback(collision);
    }
  }

  void Knockback(Collision collision)
  {
    rigidbody.AddExplosionForce(
      knockbackForce,
      transform.position + (collision.gameObject.transform.position - transform.position),
      knockbackRadius,
      knockbackUpwardsMomentum,
      ForceMode.Impulse
    );
  }
}