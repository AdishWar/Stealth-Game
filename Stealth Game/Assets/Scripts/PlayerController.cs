using UnityEngine;

public class PlayerController : MonoBehaviour {

    public event System.Action OnReachingEnd;

    public float moveSpeed = 10;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8;

    Rigidbody rb;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    bool playerDisabled;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Guard.PlayerSpotted += disable;
    }

    private void Update() {

        Vector3 inputDirection = Vector3.zero;

        if (!playerDisabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }

        float inputMagnitude = inputDirection.magnitude;
        float turnAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        angle = Mathf.LerpAngle(angle, turnAngle, turnSpeed * inputMagnitude * Time.deltaTime);

        velocity = inputDirection * moveSpeed * smoothInputMagnitude;

    }

    void disable()
    {
        playerDisabled = true;
    }

    private void OnDestroy()
    {
        Guard.PlayerSpotted -= disable;
    }

    private void OnTriggerEnter(Collider hitCollider)
    {
        if(hitCollider.tag == "Finish")
        {
            disable();
            if (OnReachingEnd != null)
            {
                OnReachingEnd();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);  // deltaTime is equal to fixedDeltaTime when called from fixedUpdate()
    }
};