using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float strafeSpeed = 2f;

    public float mouseSensitivity = 10f;

    private float minimumX = -360f;
    private float maximumX = 360f;

    private float minimumY = -90f;
    private float maximumY = 90f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * strafeSpeed;

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        forward /= forward.magnitude;

        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z);
        right /= right.magnitude;

        transform.parent.Translate(z * forward);
        transform.parent.Translate(x * right);


        //if (Input.GetMouseButton(0))
        //{

        rotationX += Input.GetAxis ("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis ("Mouse Y") * mouseSensitivity;

        rotationX = ClampAngle (rotationX, minimumX, maximumX);
        rotationY = ClampAngle (rotationY, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        //}
    }

    public static float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp (angle, min, max);
    }
}
