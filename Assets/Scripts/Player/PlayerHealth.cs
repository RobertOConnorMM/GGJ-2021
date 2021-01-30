using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public float health = 3f;
  public float knockbackForce = 10f;
  public float knockbackRadius = 5f;
  public float knockbackUpwardsMomentum = 1f;

  private float currentHealth;

  public bool IsDead
  {
    get => currentHealth <= 0;
  }

  void Start()
  {
    currentHealth = health;
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag != "Enemy")
    {
      return;
    }

    health--;
  }
}
