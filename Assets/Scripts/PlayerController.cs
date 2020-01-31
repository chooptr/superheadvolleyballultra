using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float fallSpeed = 10f;
    public float floorPosition = -3.25f;
    public float leftWallPostion = -5.8f;
    public float rightWallPostion = 5.8f;
    public float jumpSpeed = 30f;
    public float jumpHeight = -1f;
    public float moveSpeed = 10f;

    private bool _jumping;

    public void Jump() {
        if (transform.position.y <= floorPosition)
            _jumping = true;
    }

    public void Move(float multiplier) {
        var step = multiplier * Time.deltaTime * moveSpeed * Vector3.right;
        if ((transform.position + step).x > rightWallPostion  )
        {
            transform.position = new   Vector3(rightWallPostion, transform.position.y, transform.position.z);
            return; 
        }
        if ((transform.position + step).x < leftWallPostion)
        {
            transform.position = new Vector3(leftWallPostion, transform.position.y, transform.position.z);
            return; 
        }
        transform.position += step;
    }

    void Update() {
        if (transform.position.y >= jumpHeight) {
            _jumping = false;
        }

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

        var position1 = transform.position;
        var newY = Mathf.Clamp(position1.y + deltaY * Time.deltaTime, floorPosition, jumpHeight);
        position1 = new Vector2(position1.x, newY);
        transform.position = position1;
    }
}