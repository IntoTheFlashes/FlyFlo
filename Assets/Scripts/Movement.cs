using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    new Rigidbody rigidbody;
    [SerializeField] float relativeUpThrust = 2f;
    [SerializeField] float rotationThrust = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrustInput();
        ProcessRotationInput();
    }

    // Thrust upwards when user presses W, Space, or Up Arrow
    void ProcessThrustInput ()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Now thrusting!");
            rigidbody.AddRelativeForce(Vector3.up * relativeUpThrust * Time.deltaTime);
        }
    }

    // Rotate when user presses A, Left Arrow, D, or Right Arrow
    void ProcessRotationInput()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Rotating left!");
            ApplyRotation(rotationThrust);
        }

        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Rotating right!");
            ApplyRotation(-rotationThrust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rigidbody.freezeRotation = true;    // freeze physics system to allow priority to manual input
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rigidbody.freezeRotation = false;   // reactivate physics system
    }
}
