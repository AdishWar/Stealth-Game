using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform pathHolder;
    public float speed = 10;
    public float waitTime = 0.5f;
    public float turnSpeed = 90;

    public Light spotlight;
    public float viewDistance;
    float viewAngle;

    public LayerMask Obstacle;
    Transform playerTransform;
    Color originalSpotlightColor;

    private void Start()
    {
        viewAngle = spotlight.spotAngle;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotlightColor = spotlight.color;

        Vector3[] wayPoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = new Vector3(pathHolder.GetChild(i).position.x, 1, pathHolder.GetChild(i).position.z);
        }

        StartCoroutine(FollowPath(wayPoints));
    }

    private void Update()
    {
        if (IsPlayerVisible())
        {
            spotlight.color = Color.red;
        } else
        {
            spotlight.color = originalSpotlightColor;
        }
    }

    bool IsPlayerVisible()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, playerTransform.position, Obstacle) )
                {
                    return true;
                }
            }
        }

        return false;

    }

    IEnumerator FollowPath(Vector3[] wayPoints) 
    {
        transform.position = wayPoints[0];

        int nextWaypointIndex = 1;
        Vector3 nextWaypoint = wayPoints[nextWaypointIndex];
        transform.LookAt(nextWaypoint);
        
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);
            if (transform.position == nextWaypoint)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % wayPoints.Length;
                nextWaypoint = wayPoints[nextWaypointIndex];

                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(nextWaypoint));
            }

            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 faceTarget)
    {
        Vector3 dirLookTarget = (faceTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirLookTarget.z, dirLookTarget.x) * Mathf.Rad2Deg;

        while(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) ) >= 0.5f )
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 prevPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(waypoint.position, prevPosition);
            prevPosition = waypoint.position;

        }

        Gizmos.DrawLine(prevPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

    }

}