using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
public class neuralNet : MonoBehaviour {
 
   // Create array for the number of nodes in each layer.
   public int[] nodes = new int[] {6,4,4,4,2};
 
   // Set multiplier for the mutation function.
   double mutationAmount = 0.1;
 
   // Allow acces of sections from the whole class.
   List<Section> sections = new List<Section>();
 
   // Object for each subsection.
   public class Section {
       // Make weights accessible from everywhere in the section class.
       public double[][] weights;
 
       int sectionIndex;
       int[] sectionNodes;
 
       // Main class forms the subsection, defining a new array for the weights
       public Section(int index, int[] nodes) {
           sectionIndex = index;
           sectionNodes = nodes;
 
           int inputSize = nodes[index];
           int outputSize = nodes[index+1];
 
           weights = new double[inputSize][];
          
           for (int i = 0; i < inputSize; i++)
           {
               weights[i] = new double[outputSize];
 
               for (int j = 0; j < outputSize; j++)
               {
                   double randNum = (double) Random.Range(-100,100);
                   weights[i][j] = randNum/100;
               }
           }
       }
 
       // Deep copy of the section
       public Section sectionCopy() {
           Section copy = new Section(this.sectionIndex, this.sectionNodes);
 
           // Initialize Weights
           copy.weights = new double[this.weights.Length][];
 
           for (int i = 0; i < copy.weights.Length; i++)
               copy.weights[i] = new double[this.weights[0].Length];
 
           // Set Weights
           for (int i = 0; i < copy.weights.Length; i++)
           {
               for (int j = 0; j < copy.weights[i].Length; j++)
               {
                   copy.weights[i][j] = this.weights[i][j];
               }
           }
 
           return copy;
       }
 
       // Mutates the section
       public void mutate(double[][] weights, double mutationAmount) {
           for (int i = 0; i < weights.Length; i++)
           {
               for (int j = 0; j < weights[i].Length; j++)
               {
                   double randNum = (double) Random.Range(-100, 100);
                   weights[i][j] = weights[i][j] + randNum/100  * mutationAmount + mutationAmount/20;
               }
           }
       }   
 
       // Feeds through the section
       public double[] getOutputs(double[] Input) {
 
           // Initialize Output Array
           double[] Output = new double[weights[0].Length];
 
           // Calculate Value
           for (int i = 0; i < weights.Length; i++) {
               for (int j = 0; j < weights[i].Length; j++) {
                   Output[j] += weights[i][j] * Input[i];
               }
           }
 
           // Return Output
           return Output;
       }
   }
 
   // Runs when network script is created/enabled.
   // Creates a section for the connections between node layers
   void Awake() {
       // Get reference to game manager.
       gameManagement gameManager = GameObject.Find("gameManager").GetComponent<gameManagement>();
 
       if (gameManager.generationNo == 1) {
           // Add the new section to the array.
           for (int a = 0; a < nodes.Length-1; a++) {
               Section section = new Section(a, nodes);
               sections.Add(section);
           }
       } else {
           neuralNet bestNN = gameManager.bestNeuralNetwork.networkCopy();
           bestNN.mutate();
 
           sections = bestNN.sections;
       }
   }
 
   // Deep copy of neural network
   public neuralNet networkCopy() {
       neuralNet copyNetwork = new neuralNet();
 
       copyNetwork.nodes = new int[] {6,4,4,4,2};
 
       copyNetwork.mutationAmount = 0.1;
 
       List<Section> sections = new List<Section>();
 
       for (int a = 0; a < this.sections.Count; a++) {
           copyNetwork.sections.Add(this.sections[a].sectionCopy());
       } 
 
       return copyNetwork;
   }
 
   // Changes each weight in the section's weight array by a random offset.
   public void mutate() {
       // Loops over the sections and mutates each one.
       for (int i = 0; i < sections.Count; i++) {
           sections[i].mutate(sections[i].weights, mutationAmount);
       }
   }
 
   // Feeds the inputs through each section until to return the two outputs.
   public double[] getOutputs(double[] Input) {
       double[] Output = Input;
 
       // Loops over the sections and finds the values of each node in the latter layer of the section.
       // If this is the final section it will be the output of the neural network and will be returned.
       for (int i = 0; i < sections.Count; i++) {
           Output = sections[i].getOutputs(Output);
       }
 
       return Output;
   }
}
