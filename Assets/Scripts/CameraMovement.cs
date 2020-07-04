using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float hardness;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetpos = target.position + initialPosition;
        transform.position = new Vector3
            (
            Mathf.Lerp(transform.position.x, targetpos.x, hardness),
            Mathf.Lerp(transform.position.y, targetpos.y, hardness),
            transform.position.z
            );
        transform.LookAt(target);
    }
}
