using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInfluence : MonoBehaviour
{
    int[] i = new int[] { 5, 3, 7, 2, 2 };
    int[] ii = new int[] { 1, 1, -1, -1, 1 };
    // Start is called before the first frame update
    void Start()
    {
        linearThresholdModel(i, ii, 3);
    }

    /*    There are many different mathematical formulas for calculating models of social influence.One common formula is the linear threshold model, which is given by the following equation:

    oi,t+1 = 1 if Σ wij * oj,t > θ
    0 otherwise
    where:

    -oi,t+1 is the opinion of agent i at time t+1
    -oj,t is the opinion of agent j at time t
    -wij is the influence weight of agent j on agent i
    -θ is the threshold value

    The linear threshold model assumes that an agent's opinion will change to 1 (adopt the new opinion) if the sum of the influence weights of its neighbors
    who have already adopted the new opinion is greater than the threshold value. Otherwise, the agent's opinion will remain unchanged.*/

    /*This function takes three arguments:

    - neighbors is an array of integers that represent the influence weights of the neighbors of the agent // Можно просто по близости вычислять
    - opinions is an array of integers that represent the opinions of the neighbors of the agent
    - threshold is an integer that represents the threshold value

    The function first loops through the neighbors array and adds the product of the influence weight and the opinion of each neighbor to the sumOfWeights variable.
    If the sumOfWeights variable is greater than the threshold value, the function returns 1. Otherwise, the function returns 0.*/

    int linearThresholdModel(int[] neighbors, int[] opinions, int threshold)
    {
        float sumOfWeights = 0;
        for (int i = 0; i < neighbors.Length; i++)
        {
            sumOfWeights += neighbors[i] * opinions[i];
        }
        Debug.Log("sumOfWeights: " + sumOfWeights);

        if (sumOfWeights >= threshold)
        {
            Debug.Log("return: 1");
            return 1;
        }
        else
        {
            Debug.Log("return: 0");
            return 0;
        }
    }
}
