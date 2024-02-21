using System;
using System.Runtime.InteropServices;
using UnityEngine;
internal class Utilities
{
    internal static float GetExp(float u, float lambda)
    {
        //throw new NotImplementedException();
        return -Mathf.Log(1 - u) / lambda;
    }

    internal static float GetTriangularDistribution(float u, float a, float b, float c)
    {
        //Ref: https://en.wikipedia.org/wiki/Triangular_distribution
        //public float a = 3, b = 7, c = 5; // You should have c in (a,b)   a<c<b

        //throw new NotImplementedException();
        float V = (c - a) / (b - a);
        //print("V=" + V);
        float res = 0f;
        if (u < V)
        {
            res = a + Mathf.Sqrt(u * (b - a) * (c - a));
        }
        else if (u>= V)
        {
            res = b - Mathf.Sqrt((1-u)* (b - a) * (b- c));
        } 
        else{
            //print("Wrong U, or a or b or c; u is in [0,1), c is in (a,b)");
            Console.WriteLine("Wrong U, or a or b or c; u is in [0,1), c is in (a,b)");
        }



        return res;
    }
}