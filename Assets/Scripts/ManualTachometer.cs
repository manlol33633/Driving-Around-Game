using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTachometer : MonoBehaviour
{
    private Quaternion rotation;
    [SerializeField] private float rotationSpeed = 50;
    public static int rpmMode;
    private Rigidbody2D rb;
    public static float speed;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
    }

    void FixedUpdate() {
        switch (rpmMode) {
            case 1:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * (speed / CarMovement.gearsPub[0]))));
                ManualCar.RPM = 2500 * (speed / CarMovement.gearsPub[0]);
                break;
            case 2:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * ((speed - CarMovement.gearsPub[0]) / (CarMovement.gearsPub[1] - CarMovement.gearsPub[0])))));
                ManualCar.RPM = 2500 * ((speed - CarMovement.gearsPub[0]) / (CarMovement.gearsPub[1] - CarMovement.gearsPub[0]));
                break;
            case 3:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * ((speed - CarMovement.gearsPub[1]) / (CarMovement.gearsPub[2] - CarMovement.gearsPub[1])))));
                ManualCar.RPM = 2500 * ((speed - CarMovement.gearsPub[1]) / (CarMovement.gearsPub[2] - CarMovement.gearsPub[1]));
                break;
            case 4:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * ((speed - CarMovement.gearsPub[2]) / (CarMovement.gearsPub[3] - CarMovement.gearsPub[2])))));
                ManualCar.RPM = 2500 * ((speed - CarMovement.gearsPub[2]) / (CarMovement.gearsPub[3] - CarMovement.gearsPub[2]));
                break;
            case 5:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * ((speed - CarMovement.gearsPub[3]) / (CarMovement.gearsPub[4] - CarMovement.gearsPub[3])))));
                ManualCar.RPM = 2500 * ((speed - CarMovement.gearsPub[3]) / (CarMovement.gearsPub[4] - CarMovement.gearsPub[3]));
                break;
            case 6:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * ((speed - CarMovement.gearsPub[4]) / (CarMovement.gearsPub[5] - CarMovement.gearsPub[4])))));
                ManualCar.RPM = 2500 * ((speed - CarMovement.gearsPub[4]) / (CarMovement.gearsPub[5] - CarMovement.gearsPub[4]));
                break;
            case 7:
                rotation = Quaternion.Euler(0, 0, (7.3f - (90 * ((speed - CarMovement.gearsPub[5]) / CarMovement.gearsPub[5]))));
                ManualCar.RPM = 2500 * ((speed - CarMovement.gearsPub[5]) / CarMovement.gearsPub[5]);
                break;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
    }
}
