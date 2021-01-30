using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [SerializeField]
    private Transform target;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    public float movementSpeed = 200f;
    public float damping = 1f;

    [SerializeField]
    private Rigidbody body;

    void Start () {

    }

    void Update () {
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.LookAt(target);
    }
}