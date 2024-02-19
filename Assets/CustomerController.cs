using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public Transform destination;

    public NavMeshAgent navMeshAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("WaitingRoom").transform;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(destination.position);

    }

    public void SetDestination(Transform transform)
    {
        destination = transform;
        navMeshAgent.SetDestination(destination.position);
    }
    public Transform GetDestination()
    {
        return destination ;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ExitDoor")
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
               
    }

}
