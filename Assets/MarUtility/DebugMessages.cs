using UnityEngine;

public class DebugMessages : MonoBehaviour
{
    public static void SimulationPlaytestOnly()
        => SimulationPlaytestOnly("");
    public static void SimulationPlaytestOnly(string simulationName)
        => Debug.Log("Simulate" + simulationName + " will only work while the game window is playing.");

}
