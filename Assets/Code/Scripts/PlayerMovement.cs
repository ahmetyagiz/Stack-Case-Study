using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Assignments")]
    Rigidbody rb;
    public Animator animator;
    [SerializeField] private Joystick joystick;

    [Header("Stats")]
    public float moveSpeed;
    Vector3 dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        dir = new Vector3(joystick.Direction.x * moveSpeed, 0, joystick.Direction.y * moveSpeed);
        Vector3 lookDir = dir + transform.position;
        transform.LookAt(lookDir);
        rb.velocity = dir;
        
        if (dir != Vector3.zero)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }
}