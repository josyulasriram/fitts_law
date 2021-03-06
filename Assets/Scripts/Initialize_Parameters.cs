using UnityEngine;

public class Initialize_Parameters : MonoBehaviour {
    private CubeFactory cFactoryInstance;
    private ResultsPrinter rPrinter;
    private PlayerCubeController pCubeController;
    
	// Use this for initialization
	void Start () {
        cFactoryInstance = GameObject.Find("CubeFactory").GetComponent<CubeFactory>();
		rPrinter = GameObject.Find("Results").GetComponent<ResultsPrinter>();
        pCubeController = GameObject.Find("PlayerCube").GetComponent<PlayerCubeController>();
        ReadParametersFile();
	}
	
    void ReadParametersFile() {
        using (System.IO.StreamReader reader = new System.IO.StreamReader("Input/unsorted_input_main.csv")) {
            // Read in header information 
            var line = reader.ReadLine().Split(',');
            string name = reader.ReadLine().Split(',')[1];
            string type = reader.ReadLine().Split(',')[1];
			rPrinter.SetSimulationData(name, type);

            // Read in parameters 
            line = reader.ReadLine().Split(',');
            string hand = reader.ReadLine().Split(',')[1];
            string gain = reader.ReadLine().Split(',')[1];
            string[] playerSize = reader.ReadLine().Split(',');
            rPrinter.SetParameters(hand, gain);
            pCubeController.SetActiveController(hand);
            pCubeController.SetGain(float.Parse(gain));
            pCubeController.SetPlayerSize(float.Parse(playerSize[1]), float.Parse(playerSize[2]), float.Parse(playerSize[3]));
            line = reader.ReadLine().Split(',');
            line = reader.ReadLine().Split(',');

            while (!line[0].Equals("END")) {
                
                ParseCubes(line);
                line = reader.ReadLine().Split(',');
            }
        }
            
    }

    private void ParseCubes(string[] line) {
        string name = line[1];

        int x = int.Parse(line[2]);
        int y = int.Parse(line[3]);
        int z = int.Parse(line[4]);
        Vector3 pos = new Vector3(x, y, z);

        float xWidth = float.Parse(line[5]);
        float yHeight = float.Parse(line[6]);
        float zLength = float.Parse(line[7]);
        Vector3 size = new Vector3(xWidth, yHeight, zLength);

        cFactoryInstance.AddNewCube(pos, size);
    }
}
