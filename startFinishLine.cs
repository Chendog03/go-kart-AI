using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class startFinishLine : MonoBehaviour {
    List<string> carLog = new List<string>();

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("CarCollider")) // If this object is a car
        {
            kartPhysics kart = other.gameObject.GetComponent<kartPhysics>();
            
            if (!carLog.Contains(kart.guID)) {

                foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint")) {
                    checkpoint checkpointScript = checkpoint.gameObject.GetComponent<checkpoint>();
                    if (!checkpointScript.carLog.Contains(kart.guID)) {
                        //Lap invalid - not all checkpoints covered.
                        Debug.Log("Invalid Lap.");
                        kart.startLap = Time.time;
                        return;
                    }
                }
                
                kart.lapCount++;
                kart.endLap = Time.time;

                float lapTime = kart.endLap - kart.startLap;
                string times = getLapTimes();

                string fileText = times + lapTime.ToString();
                writeLapTime(fileText);

                foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint")) {
                    checkpoint checkpointScript = checkpoint.gameObject.GetComponent<checkpoint>();
                    checkpointScript.carLog = new List<string>();
                }

                List<string> carLog = new List<string>(); //Reset own list of tracked cars.
                kart.startLap = Time.time;
            }
        }
    }
    string getLapTimes() {
    // List<double> getLapTimes() {

       StreamReader sr = new StreamReader("/Users/Shared/Unity/New Unity Project/Assets/LapTimes.txt");
       string text = sr.ReadToEnd();
       sr.Close();

    //    List<string> lines = new List<string>(text.Split('\n'));

    //    List<double> times = new List<double>();
    //    foreach (string line in lines) {
    //         times.Add(System.Convert.ToDouble(line));
    //    }

    //    return times;

        return text;
    }

    void writeLapTime(string lap) {
        StreamWriter sw = new StreamWriter("/Users/Shared/Unity/New Unity Project/Assets/LapTimes.txt");
        sw.WriteLine(lap);
        sw.Close();
    }
}
