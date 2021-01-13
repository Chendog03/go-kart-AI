using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public bool userInput = true;
    public bool track = true;
    public bool firstPerson = false;
   
    void FixedUpdate() {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 carPos = player.GetComponent<Rigidbody>().position;
        Quaternion carRot = player.GetComponent<Rigidbody>().rotation;

        if (Input.GetButton("Jump")){
            firstPerson = true;
        }
        if (track) {
            if (userInput) {
                if (firstPerson) {
                    transform.position = carPos + 2*player.transform.up - 3*player.transform.right;
                    transform.LookAt(carPos + player.transform.up);
                } else {
                    transform.position = carPos + new Vector3(0,15,0);
                }
            } else {
                //Find best car and follow it
            }
        } else {
            
        }
    }

    private void findBest() {
       // Find best car.
    }
}
