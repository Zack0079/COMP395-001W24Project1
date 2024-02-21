using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrivalProcess : MonoBehaviour
{

    public GameObject personPrefab;
    public Transform personSpawnPlace;

    public float arrivalRateAsPersonsPerHour = 396.2f; // person/hour
    public float interArrivalTimeInHours; // = 1.0 / arrivalRateAsPersonsPerHour;
    private float interArrivalTimeInMinutes;
    private float interArrivalTimeInSeconds;

    //public float arrivalRateAsPersonsPerHour = 20; // person/hour
    public bool generateArrivals = true;

    //New as of Feb.23rd
    //Simple generation distribution - Uniform(min,max)
    //
    public float minInterArrivalTimeInSeconds = 3; 
    public float maxInterArrivalTimeInSeconds = 60;

    //Ref: https://en.wikipedia.org/wiki/Triangular_distribution
    public float a=3, b=7, c=5; // You should have c in (a,b)   a<c<b
    //
    public enum ArrivalIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime,
        TriangularDistribution
    }

    public ArrivalIntervalTimeStrategy arrivalIntervalTimeStrategy=ArrivalIntervalTimeStrategy.UniformIntervalTime;

    //New as of Feb.25th
    QueueManager queueManager;


    // Start is called before the first frame update
    void Start()
    {
        queueManager = GameObject.FindGameObjectWithTag("CounterWindow").GetComponent<QueueManager>();
        interArrivalTimeInHours = 1.0f / arrivalRateAsPersonsPerHour;
        interArrivalTimeInMinutes = interArrivalTimeInHours * 60;
        interArrivalTimeInSeconds = interArrivalTimeInMinutes * 60;
        StartCoroutine(GenerateArrivals());
    }
   
    IEnumerator GenerateArrivals()
    {
        while (generateArrivals)
        {
            GameObject personGO=Instantiate(personPrefab, personSpawnPlace.position, Quaternion.identity);

            float timeToNextArrivalInSec = interArrivalTimeInSeconds;
            float U = Random.value;
            switch (arrivalIntervalTimeStrategy)
            {
                case ArrivalIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextArrivalInSec= interArrivalTimeInSeconds;
                    break;
                case ArrivalIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds, maxInterArrivalTimeInSeconds);
                    break;
                case ArrivalIntervalTimeStrategy.ExponentialIntervalTime:
                    
                    float Lambda = 1 / arrivalRateAsPersonsPerHour;
                    timeToNextArrivalInSec = Utilities.GetExp(U,Lambda);
                    break;
                case ArrivalIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;
                case ArrivalIntervalTimeStrategy.TriangularDistribution:
                    timeToNextArrivalInSec = Utilities.GetTriangularDistribution(U, a,b,c);
                    break;
                default:
                    print("No acceptable arrivalIntervalTimeStrategy:" + arrivalIntervalTimeStrategy);
                    break;

            }

            //New as of Feb.23rd
            //float timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds,maxInterArrivalTimeInSeconds);
            yield return new WaitForSeconds(timeToNextArrivalInSec);

            //yield return new WaitForSeconds(interArrivalTimeInSeconds);

        }

    }
}
