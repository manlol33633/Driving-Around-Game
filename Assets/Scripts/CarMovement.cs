using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float rotSpeed = 90;
    [SerializeField] private float drive;
    [SerializeField] private float driveRot;
    [SerializeField] private float turn;
    [SerializeField] private float reciprocalAcceleration = 10;
    [SerializeField] private float testRot;
    private Rigidbody2D rb;
    private float tempSpeed;
    private Quaternion deltaRot;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tempSpeed = maxSpeed;
    }
    void Update()
    {
        tempSpeed = maxSpeed;
        if (Input.GetKey(KeyCode.W) && drive <= maxSpeed) {
            drive += 1/reciprocalAcceleration;
        }
        if (Input.GetKey(KeyCode.S) && drive >= -maxSpeed/2) {
            drive -= 1/reciprocalAcceleration;
        }
        if (drive < 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            drive += 1/reciprocalAcceleration;
        }
        if (drive > 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            drive -= 1/reciprocalAcceleration;
        }
        driveRot = Input.GetAxis("Vertical");
        turn = Input.GetAxis("Horizontal");
        testRot = rb.rotation + rotSpeed * Time.fixedDeltaTime * driveRot * -turn;
    }
    void FixedUpdate() {
        rb.velocity = transform.up * drive * tempSpeed * Time.fixedDeltaTime;
        rb.angularVelocity = 0;
        if (drive > 0.75f) {
            if (turn > 0 && turn <= 1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (maxSpeed * 2)) * -turn);
            }
            if (turn < 0 && turn >= -1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (maxSpeed * 2)) * -turn);
            }
            
        }
        if (drive < -0.75f) {
            if (turn < 0 && turn >= -1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (maxSpeed * 2)) * -turn);
            }
            if (turn > 0 && turn <= 1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (maxSpeed * 2)) * -turn);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Road") {
            maxSpeed = 15;
        } else if (other.gameObject.tag == "grass") {
            maxSpeed = 6.42f;
        } else if (other.gameObject.tag == "mud") {
            maxSpeed = 2.14f;
        } else {
            maxSpeed = 15;
        }
    }
}