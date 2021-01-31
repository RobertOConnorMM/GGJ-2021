using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public float health = 3f;
  public RectTransform healthBar;
  public float healthBarTranslateSmoothTime = .2f;
  public float knockbackForce = 10f;
  public float knockbackRadius = 5f;
  public float knockbackUpwardsMomentum = 1f;

  private float currentHealth;
  private float borderSize = 3f;
  private float healthBarTranslate;
  private float healthBarTranslateVelocity;
  private new Rigidbody rigidbody;
  private PlayerManager playerManager;

  public bool IsDead
  {
    get => currentHealth <= 0;
  }

  void Start()
  {
    currentHealth = health;
    rigidbody = GetComponent<Rigidbody>();
    playerManager = GetComponent<PlayerManager>();
  }

  void Update()
  {
    var position = Vector3.zero;

    position.x = Mathf.SmoothDamp(
      healthBar.transform.localPosition.x,
      healthBarTranslate,
      ref healthBarTranslateVelocity,
      healthBarTranslateSmoothTime
    );

    healthBar.transform.localPosition = position;
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag != "Enemy")
    {
      return;
    }

    var collisionRigidbody = collision.gameObject.GetComponent<Rigidbody>();

    if (Mathf.Abs(collisionRigidbody.velocity.magnitude) < 2f)
    {
      return;
    }

    currentHealth = Mathf.Max(0, currentHealth - 1);

    var healthRatio = currentHealth == 0 ? 0 : currentHealth / health;
    healthBarTranslate = -((healthBar.rect.width - borderSize) * (1 - healthRatio));

    rigidbody.AddExplosionForce(
      40f,
      transform.position + (collision.gameObject.transform.position - transform.position),
      5f,
      -.5f,
      ForceMode.Impulse
    );

    if (currentHealth <= 0)
    {
      UIManager.Instance.LoseGame();
    }
  }
}
