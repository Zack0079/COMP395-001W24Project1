using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrivalProcess : MonoBehaviour
{

    public GameObject personPrefab;
    public Transform personSpawnPlace;

    public float arrivalRateAsPersonsPerHour = 396.2f; // person/hour
    public float interArrivalTimeInHours;
    private float interArrivalTimeInMinutes;
    private float interArrivalTimeInSeconds;

    public bool generateArrivals = true;

    public float minInterArrivalTimeInSeconds = 3;
    public float maxInterArrivalTimeInSeconds = 60;

    public float a = 3, b = 7, c = 5;
    //
    public enum ArrivalIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime,
        TriangularDistribution
    }

    public ArrivalIntervalTimeStrategy arrivalIntervalTimeStrategy = ArrivalIntervalTimeStrategy.UniformIntervalTime;

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

            GameObject personGO = Instantiate(personPrefab, personSpawnPlace.position, Quaternion.identity);

            float timeToNextArrivalInSec = interArrivalTimeInSeconds;
            float U = Random.value;
            switch (arrivalIntervalTimeStrategy)
            {
                case ArrivalIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;
                case ArrivalIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds, maxInterArrivalTimeInSeconds);
                    break;
                case ArrivalIntervalTimeStrategy.ExponentialIntervalTime:

                    float Lambda = 1 / arrivalRateAsPersonsPerHour;
                    timeToNextArrivalInSec = Utilities.GetExp(U, Lambda);
                    break;

                case ArrivalIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;

                case ArrivalIntervalTimeStrategy.TriangularDistribution:
                    timeToNextArrivalInSec = Utilities.GetTriangularDistribution(U, a, b, c);
                    break;
                default:
                    print("No acceptable arrivalIntervalTimeStrategy:" + arrivalIntervalTimeStrategy);
                    break;

            }
            print("timeToNextArrivalInSec: " + timeToNextArrivalInSec);

            yield return new WaitForSeconds(timeToNextArrivalInSec);


        }

    }
    public void UpdateStrategy(int index)
    {
        arrivalIntervalTimeStrategy = (ArrivalIntervalTimeStrategy)index;
    }

    public void UpdateTimeScale(int index)
    {
        int[] arr = { 1, 2, 3, 5, 8, 10 };
        Time.timeScale = arr[index];
    }
}
