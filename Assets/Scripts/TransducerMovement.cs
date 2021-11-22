using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransducerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;
    private Vector3 movement;
    private float rotation;
    private void Update()
    {
        movement = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;
        transform.Translate(movement * _moveSpeed * Time.deltaTime, Space.Self);
        if (Input.GetKey(KeyCode.E))
            rotation = _rotationSpeed;
        else if (Input.GetKey(KeyCode.Q))
            rotation = -_rotationSpeed;
        else
            rotation = 0;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }
}
