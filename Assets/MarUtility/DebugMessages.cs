/*
 * Marlow Greenan
 * Created: 7/14/2026
 * Last Updated: 8/15/2026
 * 
 * Contains various re-used debug messages.
 */
using UnityEngine;

namespace MarUtility
{
    public class DebugMessages : MonoBehaviour
    {
        #region Multiple Instances
        //Multiple instances of the same master.
        public static void MultipleMasterInstances()
            => MultipleMasterInstances("");
        public static void MultipleMasterInstances(string masterName)
            => Debug.LogError("Multiple instances of " + masterName + "MASTER exists. You can only have one.");

        //Multiple instances of the same script.
        public static void MultipleScriptInstances()
            => MultipleScriptInstances("");
        public static void MultipleScriptInstances(string scriptName)
            => Debug.LogError("Multiple instances of " + scriptName + " exists. You can only have one.");
        #endregion

        #region Does Not Contain
        //Master library does not contain contentID.
        public static void LibraryDoesNotContain(string contentID)
            => LibraryDoesNotContain("", contentID);
        public static void LibraryDoesNotContain(string masterName, string contentID)
            => Debug.Log(masterName + " MASTER library does not contain " + contentID + ".");
        #endregion

        #region Playtest Only
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
        #endregion

        #region Material Has x
        //Returns false and debugs an error if the material does not have a texture called texID in it.
        public static bool MaterialHasTexture(MeshRenderer m, string texID)
        {
            if (!m.material.HasTexture(texID))
            {
                Debug.LogError("The material " + m.name + " does not have a texture with the name " + texID + ".");
                return false; 
            }
            return true;
        }

        //Returns false and debugs an error if the material does not have a color called colorID in it.
        public static bool MaterialHasColor(MeshRenderer m, string colorID)
        {
            if (!m.material.HasColor(colorID))
            {
                Debug.LogError("The material " + m.name + " does not have a color with the name " + colorID + ".");
                return false;
            }
            return true;
        }
        #endregion
    }

}
