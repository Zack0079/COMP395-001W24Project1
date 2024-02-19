using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] path;
    // Start is called before the first frame update

    public enum CarFSM
    {
        Spawned = 0,
        InQueue,
        InService,
        TowardsExit,
    }

    public CarFSM carFSMState = CarFSM.Spawned;
    [Header("Tweekable Data")]
    public float parkingLotCuttinDistance = 3f;
    public float parkingLotMaxSpeed = 10f; //36 

    void Start()
    {
        GameObject go = GameObject.Find("CarPath");
        path = GameObject.FindGameObjectsWithTag("Waypoint");

        parkingLotCuttinDistance = 3f;
        parkingLotMaxSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        FSM();
    }

    private void FSM()
    {
        switch (carFSMState)
        {
            case CarFSM.Spawned:
                HandleSpawned();
                break;
            case CarFSM.InQueue:
                HandleInQueue();
                break;
            case CarFSM.InService:
                HandleInService();
                break;
            case CarFSM.TowardsExit:
                HandleTowardsExit();
                break;
            default:
                throw new System.Exception("CarFSM statu unknown:" + carFSMState);
                break;
        }
    }

    private void HandleSpawned()
    {
        RaycastHit hit;
        GameObject target;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            target = hit.collider.gameObject;
        }
        else
        {
            target = path[1];
        }

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > parkingLotCuttinDistance)
        {
            float newDist = parkingLotMaxSpeed * Time.deltaTime;
            if (newDist > dist - 3f)
            {
                newDist = dist - 3f;
            }
            if (newDist > dist / 2f)
            {
                newDist = dist / 2f;
            }
            Vector3 newPos = transform.position;
            newPos.z += newDist;
            transform.position = newPos;
        }
        // carFSMState = CarFSM.InQueue;
    }

    private void HandleInQueue()
    {
        // carFSMState = CarFSM.InService;
    }

    private void HandleInService()
    {
        // carFSMState = CarFSM.TowardsExit;
    }

    private void HandleTowardsExit()
    {
        // carFSMState = CarFSM.Spawned;
    }
}
