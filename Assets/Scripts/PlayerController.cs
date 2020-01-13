using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float fallSpeed = 10f;
    public float floorPosition = -3.25f;
    public float jumpSpeed = 30f;
    public float jumpHeight = -1f;
    public float moveSpeed = 10f;

    private bool _jumping;

    public void Jump() {
        if (transform.position.y <= floorPosition)
            _jumping = true;
    }

    public void Move(float multiplier) {
        transform.position += multiplier * Time.deltaTime * moveSpeed * Vector3.right;
    }

    void Update() {
//        Move(Input.GetAxis("Horizontal"));

        if (transform.position.y >= jumpHeight) {
            _jumping = false;
        }

//        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= floorPosition) {
//            Jump();
//        }

        float deltaY = 0f;

        if (_jumping) {
            deltaY += jumpSpeed;
        }

        if (transform.position.y > floorPosition) {
            deltaY -= fallSpeed;
        }
        else {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(position.x, floorPosition, position.z);
            transform1.position = position;
        }
        transform.position += deltaY * Time.deltaTime * Vector3.up;
    }
}