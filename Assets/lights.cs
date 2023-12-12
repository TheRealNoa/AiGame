using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLightController : MonoBehaviour
{
    [SerializeField]
    private List<Light> lightsList = new List<Light>();
    [SerializeField]
    private BoxCollider targetCollider;
    [Range(0f, 0.1f)]
    public float percentageAddition = 0.05f;

    EnemyFuseBox enemyFuseBox;
    GameObject enemy;

    private void Start()
    {
        percentageAddition = 0.013f;
        enemy = GameObject.Find("Enemy");
        enemyFuseBox = enemy.GetComponent<EnemyFuseBox>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryToggleLights();
        }
    }

    void TryToggleLights()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hit.collider == targetCollider)
            {
                enemyFuseBox.probabilityToTurnLightsOff += percentageAddition;
                Debug.Log("Added to probability of enemy turning the light off");
                foreach (Light lightObject in lightsList)
                {
                    LightToggle lightToggle = lightObject.GetComponent<LightToggle>();
                    //enemyFuseBox.probabilityToTurnLightsOff += percentageAddition;// adds x chance for enemy to turn off all lights
                    // It was too much so I changed it to "Per activation"
                                                                      ///--------------------NOTE
                                                                      /// This is PER LIGHT SOURCE in the list of lights
                                                                      /// ... meaning, if a switch activates 1 light, we add x%;
                                                                      /// if a switch activates 5 lights, we add 5x%

                    if (lightToggle != null)
                    {
                        if (lightToggle.canToggle)
                        {
                            lightToggle.ToggleLight();
                            Debug.Log("Toggled light with LightToggle script on object: " + lightObject.gameObject.name);
                        }
                        else
                        {
                            Debug.Log("All lights have been toggled off and player can't turn them on");
                        }
                    }
                    else
                    {
                        Debug.Log("LightToggle script not found on object: " + lightObject.gameObject.name);
                    }
                }
            }
        }
    }
}
