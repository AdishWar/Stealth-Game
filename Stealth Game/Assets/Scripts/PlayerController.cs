using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8;

    Rigidbody rb;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update() {

        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;
        float turnAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        angle = Mathf.LerpAngle(angle, turnAngle, turnSpeed * inputMagnitude * Time.deltaTime);


        velocity = inputDirection * moveSpeed * smoothInputMagnitude;

    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);  // deltaTime is equal to fixedDeltaTime when called from fixedUpdate()
    }
};