using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceController : MonoBehaviour
{
    public GameObject arrivalQueueControllerGO;
    ArrivalQueueController arrivalQueueController;
    public bool isServicing=false;
    public float waitForCustomer=.5f; // sec

    public float service_rate = 0.8f;// one service every .8 sec in avarage

    Queue_Utilities queue_Utilities; // = new Queue_Utilities();

    public Transform exitDoor;
    public Transform desk;

    // Start is called before the first frame update
    void Start()
    {
        //queue_Utilities = new Queue_Utilities();
        arrivalQueueController = arrivalQueueControllerGO.GetComponent<ArrivalQueueController>();
        exitDoor = GameObject.FindGameObjectWithTag("ExitDoor").transform;
        desk = GameObject.FindGameObjectWithTag("Desk").transform;
    }

    void Update()
    {
        if (!isServicing)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {

                StopCoroutine(DoService());
                isServicing = true;
                StartCoroutine(DoService());






            }
        }
    }


    IEnumerator DoService()
    {
        while (isServicing)
        {
            GameObject go = arrivalQueueController.GetFirstCustomer();
            if(go == null)
            {
                yield return new WaitForSeconds(waitForCustomer); // wait one frame
            }
            else
            {
                float service_time_in_seconds = Queue_Utilities.ExpDist(1f / service_rate); //this is in sec
                print("service_time_in_seconds:" + service_time_in_seconds);
                go.GetComponent<CustomerController>().SetDestination(desk);


                yield return new WaitForSeconds(service_time_in_seconds);
                go.GetComponent<CustomerController>().SetDestination(exitDoor);

            }
        }

    }

}
