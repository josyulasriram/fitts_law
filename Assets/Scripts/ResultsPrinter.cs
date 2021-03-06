using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsPrinter : MonoBehaviour {
    private List<string> output;
    // Use this for initialization
    void Start() {
        output = new List<string>();
    }

    void OnDestroy() {
        PrintResultsFile();
    }

    private void PrintResultsFile() {
		using (System.IO.StreamWriter resultsWriter = new System.IO.StreamWriter("Output/unsorted_input_main.csv")) {
            foreach (string s in output) {
                resultsWriter.WriteLine(s);
            }
        }
    }
    public void SetSimulationData(string name, string type) {
        output.Add("Name:," + name + ",");
        output.Add("Type:," + type + ",");
    }

    public void SetParameters(string hand, string gain) {
        output.Add("Hand:," + hand + ",");
        output.Add("Gain:," + gain + ",");
        output.Add(",");
        output.Add("Total Time,Cube Number,Time for Cube,,Number of Collisions,Time Colliding,Distance,," +
            "Player X,Player Y,Player Z,,Cube X,Cube Y, Cube Z,,X Scale, Y Scale, Z Scale,,");
    }

    public void RecieveData(float totalTime, int cubeNumber, float timeForCube, int numOfCollisions, float timeColliding, float distance, 
        Vector3 playerPosition, Vector3 cubePosition, Vector3 cubeScale) {

        System.Text.StringBuilder data = new System.Text.StringBuilder();

        data.Append(totalTime + ",");
        data.Append(cubeNumber + ",");
        data.Append(timeForCube + ",,");

        data.Append(numOfCollisions + ",");
        data.Append(timeColliding + ",");
        data.Append(distance + ",,");

        data.Append(playerPosition.x + ",");
        data.Append(playerPosition.y + ",");
        data.Append(playerPosition.z + ",,");

        data.Append(cubePosition.x + ",");
        data.Append(cubePosition.y + ",");
        data.Append(cubePosition.z + ",,");

        data.Append(cubeScale.x + ",");
        data.Append(cubeScale.y + ",");
        data.Append(cubeScale.z + ",,");

        output.Add(data.ToString());
    }
}
