using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class neuralKartMovement : MonoBehaviour {
   public string guID { get; private set; } // Allows the system to reach the guID from outside the class.
 
   // Sets global variables for the kart, adjustable from the editor.
   public float driveSpeed = 3;
   public float rotSpeed = 3;
 
   public Rigidbody rb; // Allows reference to the karts rigidbody script.
 
   public int fitness = 0; // Debugging use.
 
   // Sets global variables for the kart, adjustable from the editor and public to the classes that record lap times.
   public float startLap = 0;
   public float endLap = 0;
   public int lapCount = 0;
 
   public gameManagement gameManagement; // Allows reference to the gameManager script used in onCollisionEnter().
  
   public neuralNet network;
 
   // Sets conditions for the physics engine (e.g. gravity) and sets the uuid for the kart.
   // This should be in awake since the object has to be instantiated first.
   void Awake() {
       rb.useGravity = true;
       guID = System.Guid.NewGuid().ToString();
 
       network = gameObject.AddComponent<neuralNet>(); // public NeuralNetwork that refers to the next neural network to be set to the next instantiated car.
 
       StartCoroutine(IsNotImproving());
   }
 
   // FixedUpdate calls every game frame.
   // Runs getNeuralAxis() and Move(). Move currently takes the outputs of the N.N algorithm as forward and angular velocities.
   private void FixedUpdate() {
       getNeuralAxis (out float v, out float h);
 
       // Debug.Log(v.ToString() + h.ToString());
       Move(v, h);
   }
 
   // Checks each few seconds if the car didn't make any improvement
   IEnumerator IsNotImproving ()
   {
       while(true)
       {
           int oldFitness = fitness; // Save the initial fitness
           yield return new WaitForSeconds(5); // Wait for some time
           if (oldFitness == fitness) // Check if the fitness didn't change yet
               gameManagement.Singleton.removeKart(this); // Kill this car
       }
   }
 
    // Casts all the rays, puts them through the NeuralNetwork and outputs the Move Axis
   void getNeuralAxis (out float Vertical, out float Horizontal)
   {
       // Get the hit distances from the raycating and store them as inputs
       double[] NeuralInput = Raycasting();
 
       // Feed through the network
       double[] NeuralOutput = network.getOutputs(NeuralInput);
      
       // Get Vertical Value
       if (NeuralOutput[0] <= 4.0f)
           Vertical = -1;
       else if (NeuralOutput[0] >= 8.0f)
           Vertical = 1;
       else
           Vertical = 0;
 
       // Get Horizontal Value
       if (NeuralOutput[1] <= 4.0f)
           Horizontal = -1;
       else if (NeuralOutput[1] >= 8.0f)
           Horizontal = 1;
       else
           Horizontal = 0;
 
       // If the output is just standing still, then move the car forward
       if (Vertical == 0 && Horizontal == 0)
           Vertical = -1;
   }
 
   // Sets the velocity of the kart to the front facing vector times the vertical keyboard input times the drive speed.
   // Sets the angular velocity (speed of rotation) to normal vector of the car times the keyboard input times the turn speed.
   public void Move (float v, float h) {
       float gameSpeed = gameManagement.Singleton.gameSpeed;
 
       rb.velocity = transform.right * v * driveSpeed * gameSpeed;
       rb.angularVelocity = transform.up * h * rotSpeed * gameSpeed;
   }
 
   // Calls SendRay() in 6 directions for use as inputs to the N.N.
   private double[] Raycasting() {
       double[] neuralInput = new double[6];
 
       SendRay(Vector3.forward, out neuralInput[0]);
       SendRay(-Vector3.forward, out neuralInput[1]);
       SendRay(Vector3.left, out neuralInput[2]);
       SendRay(Vector3.right, out neuralInput[3]);
 
 
       // Averaging vectors to get diagonals because diagonals are not prebuilt.
        // .Normalize() turns vector into a unit vector so that the raycasts are all the same length.
       Vector3 northWest = ((Vector3.right + Vector3.forward)/2);
       SendRay(Vector3.Normalize(northWest), out neuralInput[4]);
 
       Vector3 northEast = ((Vector3.right - Vector3.forward)/2);
       SendRay(Vector3.Normalize(northEast), out neuralInput[5]);
 
       return neuralInput;
   }
 
   // Sends a ray in the direction of the vector parsed.
   // Determiens whether the ray hits and changes the debug colour accordingly.
   private void SendRay(Vector3 rayNormVector, out double rayDistance) {
       // Bit shift the index of the layer (10) to get a bit mask
       int layerMask = 1 << 10;
 
       float rayLength = 10;
 
       // Allows the access of the hit to be accessed outside of the if statement, will be parsed into the N.N algorithm.
       RaycastHit hit;
       rayDistance = rayLength;
 
       // Does the ray intersect any objects in environment layer
       if (Physics.Raycast(transform.position, transform.TransformDirection(rayNormVector), out hit, rayLength, layerMask)) {
           Debug.DrawRay(transform.position, transform.TransformDirection(rayNormVector) * hit.distance, Color.red, 0.1f, true);
           rayDistance = hit.distance;
       } else {
           Debug.DrawRay(transform.position, transform.TransformDirection(rayNormVector) * rayLength, Color.green, 0.1f, true);
       }
 
   }
 
   // Increases the fitness for the kart when called from the checkpoint collision event.
   public void checkpointHit() {
       fitness++;
       // Debug.Log(fitness);
   }
 
   // Event function that runs when the kart collides with another collider.
   // The function checks that the collision was with a wall and destroys the instance.
   void OnCollisionEnter(Collision other) {
       if (other.gameObject.layer == LayerMask.NameToLayer("Environment")) { // If hits a wall
           gameManagement.Singleton.removeKart(this);
           // Debug.Log("WALL HIT");
       }
   }
 
  
}
