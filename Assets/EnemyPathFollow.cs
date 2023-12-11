using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform[] PathPoints;
    private int pointIndex;
    bool firstPoint;
    EnemyFollow enemyFollow;
    float enemySpeed;

    public bool pathActivated;
    void Start()
    {
        enemyFollow = GetComponent<EnemyFollow>();
        enemySpeed = enemyFollow.patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (pathActivated)
        {
            if (!firstPoint)
            {
                transform.position = PathPoints[pointIndex].transform.position;
                firstPoint = true;
                Debug.Log("Went to first point");
            }
            else
            {
                if (pointIndex <= PathPoints.Length)
                {
                    enemyFollow.transform.position = Vector3.MoveTowards(transform.position, PathPoints[pointIndex].transform.position, enemySpeed * Time.deltaTime);
                    Debug.Log("Went to another point");
                    if (transform.position == PathPoints[pointIndex].transform.position)
                    {
                        Debug.Log("Point got changed");
                        pointIndex++;
                    }
                    if (pointIndex >= PathPoints.Length)
                    {
                        Debug.Log("Back to start");
                        pathActivated = false;
                        pointIndex = 0;
                    }
                }
            }
        }
    }
}
