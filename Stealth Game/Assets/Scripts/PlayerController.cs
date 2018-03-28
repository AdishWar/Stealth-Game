using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float moveSpeed = 7;
	
	void Update () {

        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float turnAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        transform.eulerAngles = Vector3.up * turnAngle;
        transform.Translate(inputDirection * moveSpeed * Time.deltaTime, Space.World);

	}
}
