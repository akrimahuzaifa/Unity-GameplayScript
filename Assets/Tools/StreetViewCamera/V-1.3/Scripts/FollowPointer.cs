using UnityEngine;
/// <summary>
/// Follow pointer on mouse
/// </summary>

namespace TLL.Cam.StreetViewCam
{
    public class FollowPointer : MonoBehaviour
    {
        #region Vars
        [Header("If not asssign then, main camera will be used")]
        public Camera cam;

        [Header("Keyboard Shortcut Key for snapping on vertex")]
        public KeyCode snappingShortKey = KeyCode.X;
        #endregion

        #region UnityEvents
        private void Start()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
        }

        void LateUpdate()
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitRay;

            if (Physics.Raycast(ray, out hitRay))
            {
                //if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.X))
                if (Input.GetKey(snappingShortKey))
                {
                    var verts = hitRay.transform.GetComponent<MeshFilter>().mesh.vertices;
                    Vector3 worldpt = hitRay.transform.TransformPoint(verts[GetClosestVertex(hitRay, hitRay.transform.GetComponent<MeshFilter>().mesh.triangles)]);

                    transform.position = worldpt;
                }
                else
                {
                    Vector3 pos = hitRay.point;
                    Vector3 normal = hitRay.normal;
                    this.transform.position = new Vector3(pos.x, pos.y + 0.005f, pos.z);
                    this.transform.rotation = Quaternion.FromToRotation(Vector3.forward, normal);

                }

            }
        }
        #endregion

        public static int GetClosestVertex(RaycastHit aHit, int[] aTriangles)
        {
            var b = aHit.barycentricCoordinate;
            int index = aHit.triangleIndex * 3;

            if (aTriangles == null || index < 0 || index + 2 >= aTriangles.Length)
                return -1;

            if (b.x > b.y)
            {
                if (b.x > b.z)
                {
                    return aTriangles[index]; //x
                }
                else
                {
                    return aTriangles[index + 2];//z
                }
            }
            else if (b.y > b.z)
            {
                return aTriangles[index + 1];//y
            }
            else
            {
                return aTriangles[index + 2];//z
            }
        }

    }
}