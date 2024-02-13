using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffectManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        WaveEffect();
    }

    void WaveEffect()
    {
        if (playerMovement.animator.GetBool("Running"))
        {
            animator.SetBool("Wave", true);
        }
        else
        {
            animator.SetBool("Wave", false);
        }
    }
}
