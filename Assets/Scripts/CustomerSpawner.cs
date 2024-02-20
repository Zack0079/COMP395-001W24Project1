using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
  public GameObject carPrefab;
  public float carArrivalRate = 25f;//cars per hour
  float carInterArrivalRateHours;//hours
  float carInterArrivalRateMins;//min

  // Start is called before the first frame update
  public enum InterArrivalProcessStrategy
  {
    None=0,
    Constant,
    Uniform,
    Exponential,
    Normal,
    Observed
  }

  public InterArrivalProcessStrategy interArrivalProcessStrategy = InterArrivalProcessStrategy.Constant;

  void Start()
  {
    carInterArrivalRateHours = 1f / carArrivalRate;
    carInterArrivalRateMins = carInterArrivalRateHours * 60;
    SpawnCar();
  }

  private void SpawnCar()
  {
      Instantiate(carPrefab, this.gameObject.transform);
      Invoke("SpawnCar", carInterArrivalRateMins);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
