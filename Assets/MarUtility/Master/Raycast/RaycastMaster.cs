/*
 * Marlow Greenan
 * Created: 09/11/2026
 * Late Updated: 09/11/2026 by Marlow Greenan
 * 
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility
{
    public class RaycastMaster : Manager
    {
        private static RaycastMaster inst;

        [SerializeField, BoxGroup("Input Action IDs")]
        private string _actionPointID = "POINT";

        private InputAction point;

        #region GS
        public static RaycastMaster INST { get => inst; set => inst = value; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleMasterInstances("Raycast");

            InitializeInput();

            base.Initialize();
        }

        private void InitializeInput()
        {
            point = InputSystem.actions.FindAction(_actionPointID);
        }

        #region Raycast
        //Raycasts starting at the origin and going to a direction, then draws the ray.
        public static RaycastData VisualRaycast(Transform origin, float distance, Vector3 direction, string layer)
            => VisualRaycast(origin.position, distance, direction, layer);
        public static RaycastData VisualRaycast(Transform origin, float distance, Vector3 direction, LayerMask layer)
            => VisualRaycast(origin.position, distance, direction, layer);
        public static RaycastData VisualRaycast(Vector3 origin, float distance, Vector3 direction, string layer)
            => VisualRaycast(origin, distance, direction, 1 << LayerMask.NameToLayer(layer));
        public static RaycastData VisualRaycast(Vector3 origin, float distance, Vector3 direction, LayerMask layer)
        {
            RaycastHit hit;
            bool check = Physics.Raycast(origin, direction, out hit, distance, layer);

            if (check)
                Debug.DrawLine(origin, origin + direction, Color.green);
            else
                Debug.DrawLine(origin, origin + direction, Color.red);

            return new RaycastData(check, hit);
        }

        //Raycasts a ray then draws it.
        public static RaycastData VisualRaycast(Ray ray, float distance, string layer)
        {
            RaycastHit hit;
            bool check = Physics.Raycast(ray, out hit, distance, 1 << LayerMask.NameToLayer(layer));

            if (check)
                Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.green);
            else
                Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.red);

            return new RaycastData(check, hit);
        }

        //Raycasts starting at the player's mouse cursor and goes straight from that point.
        public RaycastData VisualRaycastAtCursor(float distance, string layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(point.ReadValue<Vector2>());

            RaycastHit hit;
            bool check = Physics.Raycast(ray, out hit, distance, 1 << LayerMask.NameToLayer(layer));

            if (check)
                Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.green);
            else
                Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.red);

            return new RaycastData(check, hit);
        }
        #endregion

        #region Spherecast
        public static RaycastData VisualSpherecast(Transform origin, float radius, float distance, Vector3 direction, string layer)
            => VisualSpherecast(origin.position, radius, distance, direction, layer);
        public static RaycastData VisualSpherecast(Vector3 origin, float radius, float distance, Vector3 direction, string layer)
        {
            RaycastHit hit;
            bool check = Physics.SphereCast(origin, radius, direction, out hit, distance, 1 << LayerMask.NameToLayer(layer));

            return new RaycastData(check, hit);
        }
        #endregion
    }
    //=================================================================================================================
    public class RaycastData
    {
        private bool raycastBool;
        private RaycastHit hit;

        public RaycastData(bool raycastBool, RaycastHit raycastHit)
        {
            this.raycastBool = raycastBool;
            hit = raycastHit;
        }

        #region GS
        public RaycastHit Hit { get => hit; set => hit = value; }
        public bool Bool { get => raycastBool; set => raycastBool = value; }
        #endregion
    }
}
