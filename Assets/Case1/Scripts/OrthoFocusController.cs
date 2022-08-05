using UnityEngine;

namespace Case1
{
    public class OrthoFocusController : MonoBehaviour
    {
        [SerializeField] private Camera camera;
    
        private Vector3 targetPosition;
        private float targetSize;

        private void Awake()
        {
            if (!camera) camera = Camera.main;
        
        }

        private void OnEnable()
        {
            EventBus.ViewBoundsChanged += ViewBoundsChanged;
        }

        private void OnDisable()
        {
            EventBus.ViewBoundsChanged -= ViewBoundsChanged;
        }

        private void ViewBoundsChanged(Bounds bounds,float aspectHeightMultiplier)
        {
            targetPosition = bounds.center;
            targetPosition.z = camera.transform.position.z;
            targetSize = CalculateOrthographicSize(bounds,aspectHeightMultiplier);
            camera.orthographicSize = targetSize;
            targetPosition.y += -targetSize * (1-aspectHeightMultiplier);
            camera.transform.position = targetPosition;
        }
    

        private float CalculateOrthographicSize(Bounds boundingBox,float aspectHeightMultiplier)
        {
            float orthographicSize;
            var size = boundingBox.size;
            var width = size.x /aspectHeightMultiplier;
            var height = size.y /aspectHeightMultiplier;
            var ratio = width / height;
            var aspect = Screen.width/(Screen.height*aspectHeightMultiplier);
            if (aspect < ratio)
            {
                orthographicSize = width/2/aspect;
            }
            else
            {
                orthographicSize = height/2;
            }

            return orthographicSize;
        }
    }
}