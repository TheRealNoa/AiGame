using System.Collections;
using UnityEngine;

public class EnemyFuseBox : MonoBehaviour
{
    private GameObjectInteraction fuseBox;
    private GameObject fuseBoxObj;
    /// 
    /// The bellow public floats will be changed by player activity
    /// 
    public float activationInterval = 30f; // How many seconds between each attempt to turn off lights
    public float probabilityToTurnLightsOff = 0.3000f; // Adjust this value based on your desired probability

    void Start()
    {
        fuseBoxObj = GameObject.Find("LightSwitchUpstairs");
        fuseBox = fuseBoxObj.GetComponent<GameObjectInteraction>();
        StartCoroutine(TurnLightsOffPeriodically());
        activationInterval = 30f;
        probabilityToTurnLightsOff = 0.3000f;
    }


    private IEnumerator TurnLightsOffPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(activationInterval);

            float randomValue = Random.Range(0f, 1f);
            Debug.Log("Random Value: " + randomValue);
            if (fuseBox.hasItem) // just checking if fusebox was fixed by player
            {
                if (randomValue < probabilityToTurnLightsOff)
                {
                    if (fuseBox.on)
                    {
                        fuseBox.ToggleLightsOff(false);
                        Debug.Log("All lights have been toggled off");
                        fuseBox.on = false;
                        probabilityToTurnLightsOff = 0.3f;
                        if(activationInterval >= 15)
                        {
                            activationInterval -= 2f;
                        } 
                        /// When lights are turned off, the probability of them getting
                        /// turned off again gets reset, but the interval at which the activation
                        /// of the TurnLightsOffPeriodically() function happens gets shorter.
                    }
                }
            }
        }
    }
}
