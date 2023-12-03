using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    private Animator animator;
    bool played;
    public bool activate;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(activate)
        {
            activate = false;
            TryMoveCamera();
        }
}
    void TryMoveCamera()
    {
        if(!played)
        {
            animator.Play("InCloset");
            played = !played;
        }
        else
        {
            animator.Play("OutCloset");
            played = !played;
        }
    }
}
