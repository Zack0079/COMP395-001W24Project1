using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PrefabController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Transform targetWindow;
    public Transform targetPerson=null;
    public Transform targetExit = null;

    public bool InService { get; set; }
    public GameObject counterWindow;
    public QueueManager queueManager;

    public enum PersonState
    {
        None=-1,
        Entered,  //going towards the DriveThruWindow (don't bump into fron cars)
        InService,
        Serviced
    }
    public PersonState personState = PersonState.None;
    // Start is called before the first frame update
    void Start()
    {
        counterWindow = GameObject.FindGameObjectWithTag("CounterWindow");
        targetWindow = counterWindow.transform;
        targetExit = GameObject.FindGameObjectWithTag("PersonExit").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        //
        personState = PersonState.Entered;
        FSMPerson();

    }

    void FSMPerson()
    {
        switch (personState)
        {
            case PersonState.None: //do nothing - shouldn't happen
                break;
            case PersonState.Entered:
                DoEntered();
                break;
            case PersonState.InService:
                DoInService();
                break;
            case PersonState.Serviced:
                DoServiced();
                break;
            default:
                print("personState unknown!:" + personState);
                break;

        }
    }
    void DoEntered()
    {
        targetPerson = targetWindow;

        queueManager = GameObject.FindGameObjectWithTag("CounterWindow").GetComponent<QueueManager>();
        queueManager.Add(this.gameObject);

        navMeshAgent.SetDestination(targetPerson.position);
        navMeshAgent.isStopped = false;
    }
    void DoInService()
    {
        navMeshAgent.isStopped = true;
    }
    void DoServiced()
    {
        navMeshAgent.SetDestination(targetExit.position);
        navMeshAgent.isStopped = false;
    }
    public void ChangeState(PersonState newPersonState)
    {
        this.personState = newPersonState;
        FSMPerson();
    }

    public void ExitService(Transform target)
    {
        queueManager.PopFirst();
        ChangeState(PersonState.Serviced);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Person")
        {
        }
        else if (other.gameObject.tag == "CounterWindow")
        {
            ChangeState(PersonState.InService);
        }
        else if (other.gameObject.tag == "PersonExit")
        {
            Destroy(this.gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, targetWindow.transform.position);
        if (targetPerson)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, targetPerson.transform.position);

        }
        if (targetExit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, targetExit.transform.position);

        }


    }

}
