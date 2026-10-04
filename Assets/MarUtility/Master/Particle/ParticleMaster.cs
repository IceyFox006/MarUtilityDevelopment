/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 10/04/2026
 * 
 * Manages all instantiated particles.
 */
using MarUtility;
using MarUtility.ExecutionManagement;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaster : Manager
{
    public static ParticleMaster INST;

    [SerializeField, Tooltip("Keys are case sensitive. If a key is changed, make sure all instances where it is used are updated.")]
        private Dictionary<string, GameObject> _library;

    public override void Initialize()
    {
        //Set singleton.
        if (INST == null)
            INST = this;
        else
            DebugMessages.MultipleMasterInstances("PARTICLE");
        base.Initialize();
    }

    private bool PlaySetup(string pID, out GameObject goPref)
    {
        goPref = null;

        if (!ContainsKeyCheck(pID)) return false;
        if (!KeyHasParticleSystem(pID, out goPref)) return false;

        return true;
    }

    //Spawns the particle system at position.
    public GameObject Play(string pID, Vector3 position)
    {
        GameObject goPref; if (!PlaySetup(pID, out goPref)) return null;

        return Instantiate(goPref, position, Quaternion.identity);
    }

    //Spawns the particle system as a child of parent
    public GameObject Play(string pID, Transform parent)
    {
        GameObject goPref; if (!PlaySetup(pID, out goPref)) return null;

        return Instantiate(goPref, parent);
    }

    //Returns false if the library does not contain the ID.
    private bool ContainsKeyCheck(string pID)
    {
        if (!_library.ContainsKey(pID))
        {
            DebugMessages.LibraryDoesNotContain("PARTICLE", pID);
            return false;
        }
        return true;
    }

    //Returns false if the key does not have a gameobject with a particle system component.
    private bool KeyHasParticleSystem(string pID, out GameObject goPref)
    {
        _library.TryGetValue(pID, out goPref);
        if (goPref.GetComponent<ParticleSystem>() == null)
        {
            Debug.LogError("PARTICLE MASTER particle " + pID + " does not have a particle system component.");
            return false;
        }
        return true;
    }
}
