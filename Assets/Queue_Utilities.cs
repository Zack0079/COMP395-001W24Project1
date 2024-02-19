using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue_Utilities : ScriptableObject
{
    public static string TimeUnit = "Min";
    public static float lambda = 20; //Arrival Rate (count/TimeUnit)
    public static float mu = 25; //Service Rate  (count/TimeUnit)

    private static float lambda_r; // inter-arrival time [TimeUnit]
    private static float mu_r; // = 1f / lambda; // service time [TimeUnit] 
    // Start is called before the first frame update

    public static float[] observerdData_xs=new[]{0f, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 100 };
    public static float[] observerdData_ys=new[] {0f,245,160,136,117,79,64,42,41,29,29,58 };
    public static bool observerdData_prepared = false;






    static void Start()
    {
        lambda_r = 1f / lambda;
        mu_r = 1f / mu;
    }

    public static float ExpDist(float lambda_r)
    {
        float r = Random.value;
        float expDist = -Mathf.Log(r) / lambda_r;
        return expDist;
    }
    private static int GetIndexOfUInYSTable(float u)
    {
        int i = -1;
        for(int j = 0; j < observerdData_ys.Length-1; j++)
        {
            float u0 = observerdData_ys[j];
            float u1 = observerdData_ys[j+1];

            if (u0<=u && u<u1)
            {
                return j;
            }
        }
        return i;
    }

    private static void PrepareObservedData()
    {
        float sum = 0;
        for(int i = 0; i < observerdData_ys.Length; i++)
        {
            sum += observerdData_ys[i];
            if (i > 0)
            {
                observerdData_ys[i] += observerdData_ys[i-1];
            }
        }

        for (int i = 1; i < observerdData_ys.Length; i++)
        {

            observerdData_ys[i] /=sum;
        }


    }
    //public static float ObservedDist(float[] xs, float[] ys, int n, float u)
    public static float ObservedDist()
    {
        if (!observerdData_prepared)
        {
            PrepareObservedData();
            observerdData_prepared = true;
        }
        float u = Random.value;
        float x; //the return value
        int i = GetIndexOfUInYSTable(u);
        if (i == -1)
        {
            x = -1f;
            Debug.Log("Data invalid; ys should be between 0 and 1");
        }
        float p = 0f;

        if(Mathf.Abs(u-observerdData_ys[i]) < .0001)
        {
            x = observerdData_xs[i];
        }
        else
        {
            p = (u - observerdData_ys[i]) / (observerdData_ys[i + 1] - observerdData_ys[i]);
            x = observerdData_xs[i] * (1 - p) + observerdData_xs[i + 1] * p;
        }



        //float expDist = -Mathf.Log(r) / lambda_r;
        return x;
    }

}
