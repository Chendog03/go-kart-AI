using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour {
    public List<string> carLog = new List<string>();

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("CarCollider")) // If this object is a car
        {
            kartPhysics kart = other.gameObject.GetComponent<kartPhysics>();;

            if (!carLog.Contains(kart.guID)) {
                carLog.Add(kart.guID); 
                kart.checkpointHit();
            }
        }
    }
}