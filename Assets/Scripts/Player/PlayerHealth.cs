using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public float health = 3f;
  public RectTransform healthBar;
  public float healthBarTranslateSmoothTime = 1;
  public float knockbackForce = 10f;
  public float knockbackRadius = 5f;
  public float knockbackUpwardsMomentum = 1f;

  private float currentHealth;
  private float borderSize = 3f;
  private float healthBarTranslate;
  private float healthBarTranslateVelocity;

  public bool IsDead
  {
    get => currentHealth <= 0;
  }

  void Start()
  {
    currentHealth = health;
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

    currentHealth = Mathf.Max(0, currentHealth - 1);

    var healthRatio = currentHealth == 0 ? 0 : currentHealth / health;
    healthBarTranslate = -((healthBar.rect.width - borderSize) * (1 - healthRatio));
  }
}
