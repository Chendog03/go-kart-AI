using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kartPhysics : MonoBehaviour {
    public bool UseUserInput = false;

    public string guID { get; private set; }

    public float driveSpeed = 3;
    public float rotSpeed = 3;
    public Rigidbody rb;

    public float fitness = 0;

    public float startLap = 0;
    public float endLap = 0;
    public int lapCount = 0;

    public gameManagement gameManagement;
    public static neuralNet network = new neuralNet(); // public NeuralNetwork that refers to the next neural network to be set to the next instantiated car


    void Awake() {
        rb.useGravity = true;

        guID = System.Guid.NewGuid().ToString();
        // Debug.Log(guID);
    }

    // FixedUpdate calls every game frame.

    private void FixedUpdate() {
        // For player kart controls see below
        if (UseUserInput) { 
            Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")*Input.GetAxisRaw("Vertical"));

            // Raycasting only for debugging, provides limited use to the player.
            // Raycasting();
            
            
        } else {
            // Raycasting distances give neural network inputs
            Raycasting();

            Move(1, 0);
        }
    }

    public void Move (float v, float h) {
        rb.velocity = transform.right * v * driveSpeed;
        rb.angularVelocity = transform.up * h * rotSpeed;
    }

    private void Raycasting() {
        SendRay(Vector3.forward);
        SendRay(-Vector3.forward);
        SendRay(Vector3.left);
        SendRay(Vector3.right);

        // Averaging vectors to get diagonals because diagonals are not prebuilt.
        Vector3 northWest = ((Vector3.right + Vector3.forward)/2);
        // .Normalize() turns vector into a unit vector so that the raycasts are all the same length.
        SendRay(Vector3.Normalize(northWest));

        Vector3 northEast = ((Vector3.right - Vector3.forward)/2);
        SendRay(Vector3.Normalize(northEast));
    }

    private void SendRay(Vector3 rayNormVector) {
        // Bit shift the index of the layer (10) to get a bit mask
        int layerMask = 1 << 10;

        float rayLength = 5;

        RaycastHit hit;
        // Does the ray intersect any objects in environment layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(rayNormVector), out hit, rayLength, layerMask)) {
            Debug.DrawRay(transform.position, transform.TransformDirection(rayNormVector) * hit.distance, Color.red, 0.1f, true);
            // Debug.Log(hit.distance);
        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(rayNormVector) * rayLength, Color.green, 0.1f, true);
            // Debug.Log("Did not Hit");
        }
    }

    public void checkpointHit() {
        fitness++;
        // Debug.Log(fitness);
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Environment")) { // If hits a wall
            if (gameObject.tag == "Opponent") {
                gameManagement.Singleton.removeKart(this);
                Destroy(this.gameObject);
                Debug.Log("WALL HIT");
                Debug.Log(gameManagement.Singleton.karts);
            }
        }
    }
}


