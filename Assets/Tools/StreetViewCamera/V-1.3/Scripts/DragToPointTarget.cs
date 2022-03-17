using System.Collections;
using UnityEngine;

namespace TLL.Cam.StreetViewCam
{
    public class DragToPointTarget : MonoBehaviour
    {
        #region Variables

        public static DragToPointTarget dragToPointTarget_instance;
        public GameObject CrosshairPointer;
        private LineRenderer line;

        private Camera cam;
        private CharacterController characterController;

        [Header("Bezire Line Settings")]
        public float lineStartYFactor = 1f;
        public float lineHeight = 2.5f;
        public float lineDeactiveWaitTime = 8f;

        [Header("Camera Rotation Option")]
        public bool camRotateOnRightClick = false;
        public bool rotationOnCollider = false;
        private bool onOffMovementMethod1 = false;
        private bool onOffMovementMethod2 = false;

        [Header("Camera Rotation speed on Mouse Movement")]
        public float camRotationSpeed = 0.15f;//0.5f;
        public float transitionDuration = 2;//2.5f;

        [Header("Camera FoV")]
        public float DefaultFov = 60;
        public float ChangedFovDuringTransition = 120;

        [Header("Time to Reset Camera FoV after Transition")]
        public float fieldOfViewResetDuration = 1f;//.1f;

        private float usedTransitionDuration;

        private float clicked = 0;
        private float clicktime = 0;
        private float clickdelay = 0.5f;

        private Vector3 target;

        #endregion

        private void Awake()
        {
            dragToPointTarget_instance = this;
            if (cam == null) cam = GetComponent<Camera>();
            characterController = GetComponent<CharacterController>();
            line = GetComponent<LineRenderer>();
            CrosshairPointer = GameObject.FindGameObjectWithTag("PointerSpriteForStreetViewCam");
        }

        void Update()
        {
            StreetViewCamRotationOnMeshCollider();
        }

        bool DoubleClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicked++;
                if (clicked == 1) clicktime = Time.time;
            }
            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                return true;
            }
            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
            return false;
        }

        void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            line.positionCount = 200;
            float t = 0f;
            Vector3 B = new Vector3(0, 0, 0);
            for (int i = 0; i < line.positionCount; i++)
            {
                B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                line.SetPosition(i, B);
                t += (1 / (float)line.positionCount);
            }
        }

        void DrawQuadraticBezierCurveLine(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            line.enabled = true;
            line.positionCount = 200;
            float t = 0f;
            Vector3 B = new Vector3(0, 0, 0);
            for (int i = 0; i < line.positionCount; i++)
            {

                B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                line.SetPosition(i, B);
                t += (1 / (float)line.positionCount);
            }
        }

        IEnumerator Transition()
        {
            line.enabled = false;

            characterController.enabled = false;
            float t = 0.0f;
            Vector3 startingPos = transform.position;
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / usedTransitionDuration);

                Camera.main.fieldOfView = Mathf.Lerp(DefaultFov, ChangedFovDuringTransition, t);
                transform.position = Vector3.Lerp(startingPos, target, t);
                yield return 0;
            }
            characterController.enabled = true;
            StartCoroutine(TransitionOfFieldOfView());
        }

        IEnumerator TransitionOfFieldOfView()
        {
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / fieldOfViewResetDuration);

                cam.fieldOfView = Mathf.Lerp(ChangedFovDuringTransition, DefaultFov, t);

                yield return 0;
            }
            line.enabled = true;
        }

        IEnumerator lineDisplayForSomeTime()
        {
            yield return new WaitForSeconds(lineDeactiveWaitTime);
            line.enabled = false;
        }

        void MousemovementToStreetViewCamRotation(bool onOff)
        {
            bool isMouseMovementOnXandY = (Input.GetAxis("Mouse X") == 0) && (Input.GetAxis("Mouse Y") == 0);
            bool isMouseClickedRight = Input.GetMouseButton(1);
            if (onOff)
            {
                if (camRotateOnRightClick == true)
                {
                    if (isMouseClickedRight)
                    {
                        StreetViewCamRotation();
                    }
                }
                else
                {
                    if (isMouseMovementOnXandY || isMouseClickedRight)
                    {
                        StreetViewCamRotationStop();
                    }
                    else
                    {
                        StreetViewCamRotation();
                    }
                }
            }
        }

        void StreetViewCamRotationOnMeshCollider()
        {
            if (rotationOnCollider == true)
            {
                onOffMovementMethod1 = false;
                onOffMovementMethod2 = true;
                StreetViewCamPositionRaycast(onOffMovementMethod1, onOffMovementMethod2);
            }
            else
            {
                onOffMovementMethod1 = true;
                onOffMovementMethod2 = false;
                StreetViewCamPositionRaycast(onOffMovementMethod1, onOffMovementMethod2);
            }
        }

        void StreetViewCamRotationStop()
        {
            var targetRotation = Quaternion.LookRotation(CrosshairPointer.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0);
        }

        void StreetViewCamRotation()
        {
            var targetRotation = Quaternion.LookRotation(CrosshairPointer.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, camRotationSpeed * Time.deltaTime);
        }

        void StreetViewCamPositionRaycast(bool onOff1, bool onOff2)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitRay;
            bool rotationDepandOnRayCast = Physics.Raycast(ray, out hitRay, Mathf.Infinity);
            MousemovementToStreetViewCamRotation(onOff1);
            if (rotationDepandOnRayCast)
            {
                MousemovementToStreetViewCamRotation(onOff2);
                Vector3 midpoint = new Vector3((hitRay.point.x + transform.position.x) / 2, ((hitRay.point.y + transform.position.y) / 2) + lineHeight, (hitRay.point.z + transform.position.z) / 2);
                DrawQuadraticBezierCurve(new Vector3(transform.position.x, transform.position.y + lineStartYFactor, transform.position.z), midpoint, hitRay.point);
                StartCoroutine(lineDisplayForSomeTime());

                Vector3 pos = hitRay.point;
                Vector3 normal = hitRay.normal;
                //Crosshair.transform.position = new Vector3(pos.x, pos.y + 0.5f, pos.z);

                if (DoubleClick())
                {
                    DrawQuadraticBezierCurveLine(new Vector3(transform.position.x, transform.position.y + lineStartYFactor, transform.position.z), midpoint, hitRay.point);
                    StopAllCoroutines();
                    target = hitRay.point + new Vector3(0, 2, 0);

                    bool distanceLessThan = Vector3.Distance(transform.position, hitRay.point) < 10;
                    bool distanceLessAndGreaterThan = Vector3.Distance(transform.position, hitRay.point) > 10 && Vector3.Distance(transform.position, hitRay.point) < 50;
                    if (distanceLessThan)
                    {
                        usedTransitionDuration = transitionDuration / 6;
                    }
                    else if (distanceLessAndGreaterThan)
                    {
                        usedTransitionDuration = transitionDuration / 4f;
                    }
                    else
                    {
                        usedTransitionDuration = transitionDuration;
                    }
                    StartCoroutine(Transition());
                }
            }
        }
    }
}
