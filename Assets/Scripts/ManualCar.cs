using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManualCar : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 90;
    [SerializeField] private float drive;
    [SerializeField] private float driveRot;
    [SerializeField] private float turn;
    [SerializeField] private float reciprocalAcceleration = 10;
    [SerializeField] private float testRot;
    [SerializeField] private float[] gears = new float[6];
    [SerializeField] private Vector2 vel;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private float driftFactor;
    [SerializeField] private float tempSpeed = 0;
    [SerializeField] TMP_Text clutchText;
    [SerializeField]
    public static float[] gearsPub = new float[6];
    private Rigidbody2D rb;
    private Quaternion deltaRot;
    private string surface;
    private TachometerNeedle speed = new TachometerNeedle();
    private Vector2 force;
    private Vector2 forwardVelocity;
    private Vector2 rightVelocity;
    private int gear = 0;
    public static float RPM;
    void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Road":
                surface = "Road";
                break;
            case "Grass":
                surface = "Grass";
                break;
            case "Mud":
                surface = "Mud";
                break;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fillGears();
        gearsPub = gears;
        driftFactor = 0.95f;
    }
    void Update()
    {
        vel = rb.velocity;
        if (Input.GetMouseButtonDown(0) && gear == -1 && Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.W)) { gear++; }
        if (Input.GetMouseButtonDown(0) && gear == 0 && Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.W)) { gear++; }
        if (Input.GetMouseButtonDown(0) && gear < 6 && Input.GetKey(KeyCode.C) && RPM > 2200 && RPM <= 2600 && !Input.GetKey(KeyCode.W)) { gear++; }
        if (Input.GetMouseButtonDown(1) && gear > -1 && Input.GetKey(KeyCode.C) && RPM >= -50 && RPM < 200 && !Input.GetKey(KeyCode.W)) { gear--; }
        textBox.text = "Gear: " + gear;
        force = transform.up * drive;
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.C) && gear != 0) {
            if (drive <= tempSpeed) {
                drive += 1 / reciprocalAcceleration;
            } else {
                drive -= 1 / reciprocalAcceleration;
            }
        }
        if (gear == 1) {
            RPM = (drive / tempSpeed) * 2500;
        } else if (gear > 1 && gear <= 6) {
            RPM = (drive / tempSpeed) * 2500;

        }
        switch (gear) {
            case -1:
                tempSpeed = -gears[1];
                break;
            case 0:
                tempSpeed = 0;
                break;
            case 1:
                tempSpeed = gears[0];
                break;
            case 2:
                tempSpeed = gears[1];
                break;
            case 3:
                tempSpeed = gears[2];
                break;
            case 4:
                tempSpeed = gears[3];
                break;
            case 5:
                tempSpeed = gears[4];
                break;
            case 6:
                tempSpeed = gears[5];
                break;
        }
        if (drive < 0 && Input.GetKey(KeyCode.Space)) {
            drive += 1/reciprocalAcceleration;
        }
        if (drive > 0 && Input.GetKey(KeyCode.Space)) {
            drive -= 1/reciprocalAcceleration;
        }
        driveRot = Input.GetAxis("Vertical");
        turn = Input.GetAxis("Horizontal");
        if (RPM > 2400 && RPM < 2600) {
            clutchText.text = "Clutch";
        } else if (RPM > -100 && RPM < 100) {
            clutchText.text = "Clutch";
        } else {
            clutchText.text = "";
        }
        Debug.Log(RPM);
    }
    void FixedUpdate() {
        rb.AddForce(force, ForceMode2D.Force);
        forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * ((drive / 1.1f) / tempSpeed);
        if (drive > 0.5f) {
            if (turn > 0 && turn <= 1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (tempSpeed * 2)) * -turn);
            }
            if (turn < 0 && turn >= -1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (tempSpeed * 2)) * -turn);
            }
            
        }
        if (drive < -0.75f) {
            if (turn < 0 && turn >= -1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (tempSpeed * 2)) * -turn);
            }
            if (turn > 0 && turn <= 1) {
                rb.MoveRotation(rb.rotation + rotSpeed * Time.fixedDeltaTime * (drive * tempSpeed * Time.fixedDeltaTime / (tempSpeed * 2)) * -turn);
            }
        }
        if (vel.magnitude > 0.5f) {
            switch (surface) {
            case "Road": 
                if (drive >= tempSpeed) {
                    drive -= 1 / reciprocalAcceleration;
                    tempSpeed -= 1 / reciprocalAcceleration;
                } else if (drive <= tempSpeed) {
                    drive += 1 / reciprocalAcceleration;
                    tempSpeed += 1 / reciprocalAcceleration;
                }
                break;
            case "Grass": 
                if (drive >= (tempSpeed * 3) / 7) {
                    drive -= 1 / reciprocalAcceleration * 1.7f;
                    tempSpeed -= 1/reciprocalAcceleration * 1.7f;
                } else if (drive <= (tempSpeed * 3) / 7) {
                    drive += 1 / reciprocalAcceleration * 1.7f;
                    tempSpeed += 1 / reciprocalAcceleration * 1.7f;
                }
                break;
            case "Mud": 
                if (drive >= tempSpeed / 7) {
                    drive -= 1 / reciprocalAcceleration * 3;
                    tempSpeed -= 1 / reciprocalAcceleration * 3;
                } else if (drive <= tempSpeed / 7) {
                    drive += 1 / reciprocalAcceleration * 3;
                    tempSpeed += 1 / reciprocalAcceleration * 3;
                }
                break;
            }
        }
    }
    void fillGears() {
        gears[0] = 2.88f;
        gears[1] = 4.33f;
        gears[2] = 10.1f;
        gears[3] = 15.87f;
        gears[4] = 18.75f;
        gears[5] = 20.19f;
    }
    void Tachometer() {
        TachometerNeedle.speed = drive;
        if (drive >= 0 && drive < gears[0]) {
            TachometerNeedle.rpmMode = 1;
        } else if (drive >= gears[0] && drive < gears[1]) {
            TachometerNeedle.rpmMode = 2;
        } else if (drive >= gears[1] && drive < gears[2]) {
            TachometerNeedle.rpmMode = 3;
        }else if (drive >= gears[2] && drive < gears[3]) {
            TachometerNeedle.rpmMode = 4;
        } else if (drive >= gears[3] && drive < gears[4]) {
            TachometerNeedle.rpmMode = 5;
        } else if (drive >= gears[4] && drive < gears[5]) {
            TachometerNeedle.rpmMode = 6;
        } else if (drive >= gears[5]) {
            TachometerNeedle.rpmMode = 7;
        }
    }
}