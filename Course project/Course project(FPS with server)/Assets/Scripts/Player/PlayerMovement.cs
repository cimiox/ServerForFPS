using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Scripts/PlayerMovement")]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;

    public CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformVector(movement);

        _characterController.Move(movement);
    }
}
