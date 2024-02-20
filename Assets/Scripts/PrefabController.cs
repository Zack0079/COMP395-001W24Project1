using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabController : MonoBehaviour
{
    public GameObject[] path;
    //  is called before the first frame update

    public enum CustomerFSM
    {
        Spawned = 0,
        InQueue,
        InService,
        TowardsExit,
    }

    public CustomerFSM customerFSMState = CustomerFSM.Spawned;
    [Header("Tweekable Data")]
    public float parkingLotCuttinDistance = 3f;
    public float parkingLotMaxSpeed = 10f; //36 

    void Start()
    {
        GameObject go = GameObject.Find("CustomerPath");
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
        switch (customerFSMState)
        {
            case CustomerFSM.Spawned:
                HandleSpawned();
                break;
            case CustomerFSM.InQueue:
                HandleInQueue();
                break;
            case CustomerFSM.InService:
                HandleInService();
                break;
            case CustomerFSM.TowardsExit:
                HandleTowardsExit();
                break;
            default:
                throw new System.Exception("CustomerFSM statu unknown:" + customerFSMState);
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
        // customerFSMState = CustomerFSM.InQueue;
    }

    private void HandleInQueue()
    {
        // customerFSMState = CustomerFSM.InService;
    }

    private void HandleInService()
    {
        // customerFSMState = CustomerFSM.TowardsExit;
    }

    private void HandleTowardsExit()
    {
        // customerFSMState = CustomerFSM.Spawned;
    }
}
