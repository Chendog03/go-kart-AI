using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagement : MonoBehaviour
{
    public static gameManagement Singleton = null; // Validation if singleton already exists, clear it ready for startGame().

    [SerializeField] public bool userInput = false; 
    [SerializeField] public bool track = false; 
    [SerializeField] public bool firstPerson = false; 

    public playerKartMovement kartScript; // Getting script references from other game objects
    public cameraMovement camScript;

    [SerializeField] GameObject kartPrefab; // Getting kart prefab

    public int generationNo; // Creating generation count

    public int kartCount = 10; // Setting number of karts per evolution
    public List<neuralKartMovement> karts = new List<neuralKartMovement>(); // Preparing a new list for kart script references

    public neuralNet bestNeuralNetwork = null; // The best NeuralNetwork currently available
    int bestFitness = -1; // The Fitness of the best NeuralNetwork ever created

    public float gameSpeed = 1;

    // Run racing game.
    public void startGame() {
        // Creating a singleton reference for the gameManagement script so that the program knows this is the only one.
        if (Singleton == null) {
            Singleton = this;
        }

        // Toggles the user input.
        if (userInput) {
            kartScript.setEnable(true);
            camScript.userInput = true; 
            Debug.Log("Enabled player kart.");
        } else {
            kartScript.setEnable(false);
            camScript.userInput = false; 
            Debug.Log("Disabled player kart.");
        }

        // Toggles whether the camera moves or not, required for first person.
        if (track) {
            camScript.track = true;
        } else {
            camScript.track = false;
        }

        // Toggles the perspective of the camera
        if (firstPerson) {
            camScript.firstPerson = true; 
        } else {
            camScript.firstPerson = false;
        }

        camScript.setEnable(true); // Allows the camera to run its movement script.

        // Resetting all variables about the evolution to be default.
        generationNo = 0;
        bestNeuralNetwork = null;
        bestFitness = -1;
        karts = new List<neuralKartMovement>();

        newGeneration();
    }

    // Run neural network training algorithm.
    public void startTraining() {
        userInput = false;

        if (Singleton == null) {
            Singleton = this;
        }

        // Resetting all variables about the evolution to be default.
        generationNo = 0;
        bestNeuralNetwork = null;
        bestFitness = -1;
        karts = new List<neuralKartMovement>();

        newGeneration();
    }

    void newGeneration() {
        generationNo++;

        for (int i = 0; i < kartCount; i++) {
            GameObject newKart = Instantiate(kartPrefab, transform);
            karts.Add(newKart.GetComponent<neuralKartMovement>());
        }
    }

    public void removeKart(neuralKartMovement deadKart) {
        if (deadKart.fitness > bestFitness) {
            bestFitness = deadKart.fitness;
            bestNeuralNetwork = deadKart.network;
        }

        karts.Remove(deadKart);
        Destroy(deadKart.gameObject); // Destroy the dead car
        
        if (karts.Count <= 0) // If there are no cars left
            newGeneration(); // Create a new generation
    }

    public void changeKartNumber(string input) {
        bool changedKartNumber = int.TryParse(input, out kartCount);

        if (!(changedKartNumber)) {
            Debug.Log("Input value is not of integer type.");
            kartCount = 50;
        } 
    }

    public void changeGameSpeed(float input) {
        gameSpeed = input;
    }
}