using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        float rotationInput = Input.GetAxis("Rotation");

        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * rotationInput);
    }
}
