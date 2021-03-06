using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour {
    private List<GameObject> cubeObjects;

    public Material unselected;
    public Material selected;

    private int nextObject;

    void Start () {
		cubeObjects = new List<GameObject>();
        nextObject = 0;
	}

    public void AddNewCube(Vector3 position, Vector3 size) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = size;
        cube.GetComponent<Renderer>().material = unselected;
        cube.GetComponent<Collider>().isTrigger = true;
        //cube.AddComponent<BoxCollider>();
        cube.SetActive(false);
        cubeObjects.Add(cube);
    }

    public void SetSelectedMaterial(GameObject cube) {
        cube.GetComponent<Renderer>().material = selected;
    }

    public void SetUnselectedMaterial(GameObject cube) {
        cube.GetComponent<Renderer>().material = unselected;
    }

    public int GetCubeNumber() {
        return nextObject - 1;
    }

    public Vector3 GetCubePosition() {
        return cubeObjects[nextObject - 1].transform.position;
    }
    public Vector3 GetCubeScale()
    {
        return cubeObjects[nextObject - 1].transform.localScale;
    }

    public void destroyCurrentCube()
    {
        cubeObjects[nextObject - 1].SetActive(false);
    }

    public void NextCube() {
        if (nextObject < cubeObjects.Count) {
            cubeObjects[nextObject].SetActive(true);
            nextObject++;
        }
        else {
            Debug.Log("Here");
            Application.Quit();
        }
    }
}
