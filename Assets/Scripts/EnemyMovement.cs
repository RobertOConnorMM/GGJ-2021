using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

  [SerializeField]
  private int health = 3;
  private int currentHealth = 3;
  private bool isDead = false;

  private Transform target;
  private Vector3 targetPosition = Vector3.zero;
  private Vector3 velocity = Vector3.zero;
  public float movementSpeed = 200f;
  public float damping = 1f;

  [SerializeField]
  private Rigidbody body;

  void Start()
  {
    body = GetComponent<Rigidbody>();
    PlayerManager pm = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
    target = pm.getTransform();
    currentHealth = health;
  }

  void Update()
  {
    float step = movementSpeed * Time.deltaTime;
    body.transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    body.transform.LookAt(target);
  }

  public void OnHit(int damage)
  {
    if (isDead)
    {
      return;
    }

    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      currentHealth = 0;
      isDead = true;

      Destroy(gameObject);
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Item")
    {
      OnHit(1);
    }
  }
}