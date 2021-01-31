using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
  Seek,
  FindWeapon,
  Idle
}

public class EnemyMovement : MonoBehaviour
{
  public float health = 3f;
  public float movementSpeed = 20f;
  public float knockbackForce = 40f;
  public float knockbackRadius = 5f;
  public float knockbackUpwardsMomentum = -.5f;
  public GameObject weapon;
  public Transform weaponTransform;
  public float throwingSpeed = 5f;
  public float attackRange = 5f;
  public float attackInterval = 10f;

  private float currentHealth;

  private Transform target;
  private new Rigidbody rigidbody;
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip hurtSound, spawnSound, throwSound;

  public GameObject hitParticlesPrefab;

  private NavMeshAgent navMeshAgent;

  private EnemyState enemyState = EnemyState.Seek;

  private bool isDead
  {
    get => health < 1;
  }

  void Start()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    rigidbody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
    audioSource.PlayOneShot(spawnSound, 0.6f);

    var playerManager = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
    target = playerManager.getTransform();

    currentHealth = health;

    InvokeRepeating("Think", 0, attackInterval);
  }

  void Update()
  {
    if (enemyState == EnemyState.Idle)
    {
      return;
    }

    // Walk to weapon or player
    if (enemyState == EnemyState.FindWeapon)
    {
      if (weapon.transform.parent == weaponTransform)
      {
        return;
      }

      navMeshAgent.SetDestination(weapon.transform.position);
      rigidbody.transform.LookAt(weapon.transform);

      // Pick up weapon if walking basically on top of it
      if ((weapon.transform.position - transform.position).magnitude < 1f)
      {
        var rigidbody = weapon.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        weapon.transform.parent = weaponTransform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
      }

      return;
    }

    navMeshAgent.SetDestination(target.position);
    rigidbody.transform.LookAt(target);

    // Attack when in range and weapon attached
    var isWeaponAttached = weapon.transform.parent == weaponTransform;
    var isInAttackRange = (target.position - transform.position).magnitude < attackRange;

    if (isWeaponAttached && isInAttackRange)
    {
      ThrowWeapon();
    }
  }

  private void Think()
  {
    var isWeaponAttached = weapon.transform.parent == weaponTransform;

    if (isWeaponAttached)
    {
      enemyState = EnemyState.Seek;
    }
    else
    {
      enemyState = EnemyState.FindWeapon;
    }
  }

  void ThrowWeapon()
  {
    var isWeaponAttached = weapon.transform.parent == weaponTransform;

    if (!isWeaponAttached)
    {
      rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
      return;
    }

    audioSource.PlayOneShot(throwSound);

    // Detach weapon
    weapon.transform.parent = null;

    var rigidBody = weapon.GetComponent<Rigidbody>();
    rigidBody.isKinematic = false;
    rigidBody.AddTorque(
        Quaternion.Lerp(
            Random.rotation,
            Quaternion.Euler(weaponTransform.forward),
            .9f
        ).eulerAngles * throwingSpeed,
        ForceMode.Impulse
    );
    rigidBody.AddForce(
      weaponTransform.forward * throwingSpeed,
      ForceMode.Impulse
    );

    enemyState = EnemyState.Idle;
  }

  public void OnHit(float damage)
  {
    if (isDead)
    {
      return;
    }

    currentHealth -= damage;
    audioSource.PlayOneShot(hurtSound, 1f);
    GameObject particles = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
    Destroy(particles, 1.5f);

    if (currentHealth <= 0)
    {
      WaveManager.Instance.AddEnemyKillCount();

      Destroy(weapon);
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

  void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(transform.position, attackRange);
  }
}