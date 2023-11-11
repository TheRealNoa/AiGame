using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator anim;
    public string idle;
    public string attack;
    public bool attack_state;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (attack_state)
        {
            anim.SetBool("Test", true);
            //anim.Play(attack);
        }
        else
        {
            anim.SetBool("Test", false);
        }
    }
    public void setAttackState(bool state)
    {
        this.attack_state = state;
    }
}
