using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> waypoints;
    public float moveSpeed;
    public int target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[target].position, moveSpeed * Time.deltaTime);
    }


    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    private void FixedUpdate()
    {
        if(transform.position == waypoints[target].position)
        { if(target == waypoints.Count -1)
            {
                target = 0;
            } else
            {
                target += 1;
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        col.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        col.transform.SetParent(null);
    }
}
