using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKartMovement : MonoBehaviour {
    public string guID { get; private set; } // Allows the system to reach the guID from outside the class.

    // Sets global variables for the kart, adjustable from the editor.
    public float driveSpeed = 3;
    public float rotSpeed = 3;

    public Rigidbody rb; // Allows reference to the karts rigidbody script.

    public float fitness = 0; // Debugging use.

    // Sets global variables for the kart, adjustable from the editor and public to the classes that record lap times.
    public float startLap = 0;
    public float endLap = 0;
    public int lapCount = 0;

    public gameManagement gameManagement; // Allows reference to the gameManager script used in onCollisionEnter().
    
    // Sets conditions for the physics engine (e.g. gravity) and sets the uuid for the kart. This should be in awake since the object has to be instantiated first.
    void Awake() {
        rb.useGravity = true;

        guID = System.Guid.NewGuid().ToString();
    }

    // FixedUpdate calls every game frame. 
    // Purely runs the move function using keyboard input (-1, 0 or 1) as paramters.
    private void FixedUpdate() {
        // For player kart controls see below
        Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")*Input.GetAxisRaw("Vertical"));
    }

    // Sets the velocity of the kart to the front facing vector times the vertical keyboard input times the drive speed.
    // Sets the angular velocity (speed of rotation) to normal vector of the car times the keyboard input times the turn speed.
    public void Move (float v, float h) {
        float gameSpeed = gameManagement.Singleton.gameSpeed;

        rb.velocity = transform.right * v * driveSpeed * gameSpeed;
        rb.angularVelocity = transform.up * h * rotSpeed * gameSpeed;
    }

    // Increases the fitness for the kart when called from the checkpoint collision event.
    public void checkpointHit() {
        fitness++;
        // Debug.Log(fitness);
    }

    // Event function that runs when the kart collides with another collider.
    // The function checks that the collision was with a wall and logs that in the console. Debugging only for now.
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Environment")) { // If hits a wall
            // Debug.Log("WALL HIT");
            Debug.Log(gameManagement.Singleton.karts);
        }
    }

    public void setEnable(bool torf) {
        this.gameObject.SetActive(torf);
    }
}


