using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManagement
{

    public class DynamicCamera : 
    /// @cond SKIP_THIS
        MonoBehaviour
    /// @endcond
    {
        [Header("(Required) Camera object and target")]

        [SerializeField]
        private Camera CameraObject; // el target puede ser un jugador/NPC/objeto; totalmente dinamico.
        [SerializeField]
        private Transform target; // el target puede ser un jugador/NPC/objeto; totalmente dinamico.

        [Header("(Optional) Camera options")]

        [SerializeField]
        private Vector3 cameraOffset = new Vector3(10f, 3.5f, -1f);

        [SerializeField]
        private bool allowBounds = true; // Limites de la camara (No sigue al jugador hasta que haya pasado cierto punto)

        [SerializeField]
        private bool allowSmoothing = true; // Permite o no el suavizado de la camara

        [SerializeField]
        private float bounds = 3f; // Distancia a la que empieza moverse la camara.

        [Range(0, 10)]
        [SerializeField]
        private float smoothingAmount = 10f; // En un rango de (0 - 10), especifica la cantidad de suavizado de la camara.

        [Range(0, 10)]
        [SerializeField]
        private float cameraMoveMultiplier = 2.5f; // Cuanto se mueve la camara cuando se usa el joystick
        
        private Vector3 newCameraPosition; // La posicion a la que se movera la camara;
        private Vector3 lastCameraPosition;
        private string lastTargetTag;

        public bool FollowTarget { get; set; } = true;

        private Vector3 cameraMoveOffset = Vector3.zero;

        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        private void FixedUpdate()
        {
            // Base position of the camera : posicion inicial de la camara
            // Allow the camera to add offset even if target is null
            if (target == null || !FollowTarget)  
            {
                if(lastTargetTag.Equals("Player"))
                    newCameraPosition = (GameManager.Instance.LastDeathPosition - new Vector3(0f, 5f)) + cameraOffset + cameraMoveOffset;
            }
            else
                newCameraPosition = target.position + cameraOffset + cameraMoveOffset;

            if (target != null) {
                lastCameraPosition = target.position;
                lastTargetTag = target.tag;
            }

            // [Extras]
            // [Section][Bounds]
            if (allowBounds)
            {
                // Checkea si se esta dentro de los limites o no.
                // deltaX de la posicion de la camara y el target
                float deltaX = lastCameraPosition.x - transform.position.x;

                // if(camara dentro de limites)
                if (Mathf.Abs(deltaX) > bounds)
                {
                    // (dX > 0) ==> A la derecha de la pantalla
                    if (deltaX > 0)
                    {
                        // Alinea la pos. de la camara hacia la derecha de la pantalla
                        newCameraPosition.x =
                            lastCameraPosition.x - bounds + cameraOffset.x;
                    }
                    else
                    {
                        // Izquierda
                        newCameraPosition.x =
                            lastCameraPosition.x + bounds + cameraOffset.x;
                    }
                }
                else
                {
                    newCameraPosition.x =
                        lastCameraPosition.x - deltaX + cameraOffset.x;      
                }
            }

            // [End][Section][Bounds]
            // [Section][Smoothing]
            if (allowSmoothing)
                transform.position =
                    Vector3
                        .Lerp(transform.position,
                        newCameraPosition,
                        Time.deltaTime * smoothingAmount);
            else
                transform.position = newCameraPosition;
            // [End][Section][Smoothing]
        }

        public void ChangeTarget(Transform target)
        {
            this.target = target;
        }

        public void UpdateOffset(Vector2 offset)
        {
            cameraOffset = new Vector3(offset.x, offset.y, -1f);
        }


        public void UpdateOffsetX(float offsetX)
        {
           StartCoroutine(UpdateCameraOffsetX(offsetX));
        }

        public Vector2 GetOffsets()
        {
            return new Vector2(cameraOffset.x, cameraOffset.y);
        }


        public void UpdateOffsetY(float offsetY)
        {
            StartCoroutine(UpdateCameraOffsetY(offsetY));
        }

        public void UpdateSize(float size, float duration = 3f)
        {
            if(CameraObject != null)
                StartCoroutine(UpdateCameraSize(size, duration));
        }

        private IEnumerator UpdateCameraSize(float endValue, float duration)
        {
            float time = 0;
            float startValue = CameraObject.orthographicSize;

            while (time < duration)
            {
                CameraObject.orthographicSize =
                    Mathf.Lerp(startValue, endValue, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            CameraObject.orthographicSize = endValue;
        }

        private IEnumerator UpdateCameraOffsetX(float endValue, float duration = 0.3f)
        {
            float time = 0;
            float startValue = cameraOffset.x;

            while (time < duration)
            {
                cameraOffset.x =
                    Mathf.Lerp(startValue, endValue, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            cameraOffset.x = endValue;
        }

        private IEnumerator UpdateCameraOffsetY(float endValue, float duration = 0.3f)
        {
            float time = 0;
            float startValue = cameraOffset.y;

            while (time < duration)
            {
                cameraOffset.y =
                    Mathf.Lerp(startValue, endValue, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            cameraOffset.y = endValue;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void UpdateOffsetJoystick(Vector2 value)
        {
            cameraMoveOffset = new Vector3(value.x, value.y) * cameraMoveMultiplier;
        }
    }
}
// SEPTIMO INTENTO DE UNIT TEST CON ACTIONS
