using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform customerSpawnPlace;

    public float arrivalRateAsCustomersPerHour = (60f/9f); // customer/hour
    public float interArrivalTimeInHours; // = 1.0 / arrivalRateAsCustomersPerHour;
    private float interArrivalTimeInMinutes;
    private float interArrivalTimeInSeconds;


    public bool generateArrivals = true;

    //New as of Feb.23rd
    //Simple generation distribution - Uniform(min,max)
    //
    public float minInterArrivalTimeInSeconds = 5; 
    public float maxInterArrivalTimeInSeconds = 10;

    //Ref: https://en.wikipedia.org/wiki/Triangular_distribution
    public float a=3, b=7, c=5; // You should have c in (a,b)   a<c<b
  public enum InterArrivalProcessStrategy
  {
    None=0,
    Constant,
    Uniform,
    Exponential,
    Observed,
    Triangular
  }

  public InterArrivalProcessStrategy interArrivalProcessStrategy = InterArrivalProcessStrategy.Constant;

    // QueueManager queueManager;

    //UI debugging
// #if DEBUG_AP
//     public Text txtDebug;
// #endif

 // Start is called before the first frame update
    void Start()
    {
        // queueManager = GameObject.FindGameObjectWithTag("DriveThruWindow").GetComponent<QueueManager>();
        interArrivalTimeInHours = 1.0f / arrivalRateAsCustomersPerHour;
        interArrivalTimeInMinutes = interArrivalTimeInHours * 60;
        interArrivalTimeInSeconds = interArrivalTimeInMinutes * 60;
        StartCoroutine(GenerateArrivals());
// #if DEBUG_AP
//         print("proc#:" + System.Environment.ProcessorCount);
//         txtDebug.text = "\nproc#:" + System.Environment.ProcessorCount;
// #endif
    }
   
    IEnumerator GenerateArrivals()
    {
        while (generateArrivals)
        {
            GameObject customerGO=Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);
            //if (queueManager.Count() > 0)
            //{
            //    queueManager.Add(customerGO);
            //} //The first customer as added in the queue when in DriveThruWindow

            float timeToNextArrivalInSec = interArrivalTimeInSeconds;
            float U = Random.value;
            switch (interArrivalProcessStrategy)
            {
                case InterArrivalProcessStrategy.Constant:
                    timeToNextArrivalInSec= interArrivalTimeInSeconds;
                    break;

                case InterArrivalProcessStrategy.Uniform:
                    timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds, maxInterArrivalTimeInSeconds);
                    break;

                case InterArrivalProcessStrategy.Exponential:
                    float Lambda = 1 / arrivalRateAsCustomersPerHour;
                    timeToNextArrivalInSec = Utilities.GetExp(U,Lambda);
                    break;

                case InterArrivalProcessStrategy.Observed:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;

                case InterArrivalProcessStrategy.Triangular:
                    timeToNextArrivalInSec = Utilities.GetTriangularDistribution(U, a,b,c);
                    break;

                default:
                    print("No acceptable arrivalIntervalTimeStrategy:" + interArrivalProcessStrategy);
                    break;

            }

            //New as of Feb.23rd
            //float timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds,maxInterArrivalTimeInSeconds);
            yield return new WaitForSeconds(timeToNextArrivalInSec);

            //yield return new WaitForSeconds(interArrivalTimeInSeconds);

        }

    }
}
