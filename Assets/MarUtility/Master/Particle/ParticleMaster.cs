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
    public static ParticleMaster PARTICLE_MASTER;

    [SerializeField, Tooltip("Keys are case sensitive. If a key is changed, make sure all instances where it is used are updated.")]
        private Dictionary<string, GameObject> _library;

    public override void Initialize()
    {
        //Set singleton.
        if (PARTICLE_MASTER == null)
            PARTICLE_MASTER = this;
        else
            Debug.LogError("Multiple instances of PARTICLE_MASTER exists, you can only have one.");
        base.Initialize();
    }

    public GameObject Play(string pN, Vector3 position)
    {
        if (!_library.ContainsKey(pN))
        {
            Debug.LogError("PARTICLE MASTER library does not contain " + pN + ".");
            return null;
        }

        GameObject goPref; _library.TryGetValue(pN, out goPref);
        if (goPref.GetComponent<ParticleSystem>() == null)
        {
            Debug.LogError("PARTICLE MASTER particle " + pN + " does not have a particle system component.");
            return null;
        }

        return Instantiate(goPref, position, Quaternion.identity);
    }
}
