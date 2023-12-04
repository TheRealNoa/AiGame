using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
        private Animator animator;
        public bool played;
        public bool activate;

        public int identifier;

        public string inCloset;
        public string outCloset;

        public bool playerHidden;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (activate)
            {
                activate = false;
                CheckAnimationIdentifier();
                
            }
        }
        void TryMoveCamera()
        {
            if (!played)
            {
            playerHidden = true;
                animator.Play(inCloset);
                played = !played;
            }
            else
            {
            playerHidden = false;
            animator.Play(outCloset);
                played = !played;
            }

        }

        void CheckAnimationIdentifier()
    {
        if(identifier == 0)
        {
            inCloset = "InCloset";
            outCloset = "OutCloset";
            TryMoveCamera();
        }else if(identifier == 1)
        {
            inCloset = "GInCloset";
            outCloset = "GOutCloset";
            TryMoveCamera();
        }
        else
        {
            inCloset = "GInCloset";
            outCloset = "GOutCloset";
            TryMoveCamera();
        }
    }
    }
