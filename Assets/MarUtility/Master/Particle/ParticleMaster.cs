/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 6/27/2026
 * 
 * Manages all instantiated particles.
 */
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

    public GameObject Play(string pID, Vector3 position)
    {
        if (!_library.ContainsKey(pID))
        {
            DebugMessages.LibraryDoesNotContain("PARTICLE", pID);
            return null;
        }

        GameObject goPref; _library.TryGetValue(pID, out goPref);
        if (goPref.GetComponent<ParticleSystem>() == null)
        {
            Debug.LogError("PARTICLE MASTER particle " + pID + " does not have a particle system component.");
            return null;
        }

        return Instantiate(goPref, position, Quaternion.identity);
    }
}
