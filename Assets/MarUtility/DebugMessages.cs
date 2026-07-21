using UnityEngine;

public class DebugMessages : MonoBehaviour
{
    //Multiple instances of the same master.
    public static void MultipleMasterInstances()
        => MultipleMasterInstances("");
    public static void MultipleMasterInstances(string masterName)
        => Debug.LogError("Multiple instances of " + masterName + "MASTER exists. You can only have one.");

    //Master library does not contain contentID.
    public static void LibraryDoesNotContain(string contentID)
        => LibraryDoesNotContain("", contentID);
    public static void LibraryDoesNotContain(string masterName, string contentID)
        => Debug.Log(masterName + " MASTER library does not contain " + contentID + ".");

    //Set only works in playtest.
    public static void SetPlaytestOnly()
        => SetPlaytestOnly("");
    public static void SetPlaytestOnly(string setVarName)
        => Debug.Log("Setting the variable " + setVarName + "will only work while the game window is playing.");

    //Simulation only works in playtest.
    public static void SimulationPlaytestOnly()
        => SimulationPlaytestOnly("");
    public static void SimulationPlaytestOnly(string simulationName)
        => Debug.Log("Simulate " + simulationName + " will only work while the game window is playing.");

}
