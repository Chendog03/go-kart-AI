using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------------------------------------------------------------------------- //
// ---------------------------------------------------- Neural Network ------------------------------------------------- //
// --------------------------------------------------------------------------------------------------------------------- //


public class neuralNet : MonoBehaviour {
    int[] nodes = new int[] {6,4,3,2};

    void Awake() {
        double[][] weights = new double[nodes.Length][];

        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i]; j++)
            {
                weights[i][j] = Random.Range(-1,1);
                Debug.Log(weights);
            }
        }
    }

    public double[][] mutate(double[][] weights, double mutationAmount) {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = weights[i][j] + Random.Range(-1, 1) * mutationAmount;
            }
        }
        return weights;
    }    

    // public double[] getOutputs(double[] Input) {
        // // // Validation Checks
        // // if (Input == null)
        // //     throw new ArgumentException("The input array cannot be set to null.", "Input");
        // // else if (Input.Length != Weights.Length - 1)
        // //     throw new ArgumentException("The input array's length does not match the number of neurons in the input layer.", "Input");

        // // Initialize Output Array
        // double[] Output = new double[Weights[0].Length];

        // // Calculate Value
        // for (int i = 0; i < Weights.Length; i++) {
        //     for (int j = 0; j < Weights[i].Length; j++) {
        //         if (i == Weights.Length - 1) // If is Bias Neuron
        //             Output[j] += Weights[i][j]; // Then, the value of the neuron is equal to one
        //         else
        //             Output[j] += Weights[i][j] * Input[i];
        //     }
        // }

        // // Return Output
        // return Output;
    // }
}
