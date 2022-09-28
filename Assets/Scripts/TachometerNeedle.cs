using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TachometerNeedle : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
    }

    void FixedUpdate() {
        rb.MoveRotation(56);
    }
}
