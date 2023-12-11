using System.Collections;
using System.Collections.Generic;
using UnityEngine;

        public class DoorTrigger : MonoBehaviour
        {
    // This function is called when any collider enters the trigger
        EnemyFollow enemyFollow;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
            enemyFollow.doorTriggered();
            }
        }
    
}

