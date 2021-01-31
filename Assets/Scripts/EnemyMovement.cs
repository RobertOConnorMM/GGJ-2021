using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
  Seek,
  FindWeapon
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
  public float throwingSpeed = 12f;
  public float attackRange = 5f;

  private float currentHealth;

  private Transform target;
  private new Rigidbody rigidbody;
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip hurtSound;

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

    var playerManager = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
    target = playerManager.getTransform();

    currentHealth = health;

    InvokeRepeating("Think", 0, 1.5f);
  }

  void Update()
  {
    // Walk to weapon or player
    if (enemyState == EnemyState.FindWeapon)
    {
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

        enemyState = EnemyState.Seek;
      }

      return;
    }

    navMeshAgent.SetDestination(target.position);
    rigidbody.transform.LookAt(target);

    // Attack when in range and weapon attached
    var isWeaponAttached = weapon != null && weapon.transform.parent == weaponTransform;
    var isInAttackRange = (target.position - transform.position).magnitude < attackRange;

    if (isWeaponAttached && isInAttackRange)
    {
      ThrowWeapon();
    }
  }

  private void Think()
  {
    var isInAttackRange = (target.position - transform.position).magnitude < attackRange;
    var isWeaponAttached = weapon != null && weapon.transform.parent == weaponTransform;
    var isWeaponDestroyed = weapon == null;

    if (isInAttackRange || isWeaponAttached || isWeaponDestroyed)
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
      return;
    }

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