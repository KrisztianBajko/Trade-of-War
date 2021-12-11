using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 cameraOffset;

    [Range(0.01f, 1f)]
    [Tooltip("How fast the camera will react for the player movement.")]
    public float smoothness = 0.5f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraOffset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector3 newPosition = player.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothness);
        }
        
    }
}
