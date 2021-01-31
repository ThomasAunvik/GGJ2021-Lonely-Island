using UnityEngine;

namespace LonelyIsland.Characters
{
    public class CameraController : MonoBehaviour
    {
        private Transform cameraTransformer;

        [SerializeField] private User user;
        [SerializeField] private Vector3 positionAnchor;
        [Range(0, 1)]
        [SerializeField] private float smoothFactor = 1;

        private void Start()
        {
            cameraTransformer = transform;

            if (user == null)
                user = FindObjectOfType<User>();
        }

        private void LateUpdate()
        {
            Vector3 newPos = user.transform.position + positionAnchor;
            cameraTransformer.position = Vector3.Slerp(cameraTransformer.position, newPos, smoothFactor);
            transform.LookAt(user.transform);
        }
    }
}
