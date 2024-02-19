using UnityEngine;

public class CharacterFollow : MonoBehaviour
{
    public float turnSmoothing = 15f;   // Value for smooth rotation
    public float speedDampTime = 0.1f;  // Damping time for speed parameter

    private Animator anim;              // Reference to the Animator component
    private GameObject player;          // Reference to the player object
    public float followThreshold = 3f;  // Threshold for following

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool sneak = Input.GetButton("Sneak");

        MovementManagement(h, v, sneak);

        // Get the distance between the character and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // If the distance is greater than the threshold...
        if (distance > followThreshold)
        {
            // Calculate the direction vector from the character to the player
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();  // Normalize the direction vector

            // Move the character based on the direction vector
            transform.position += direction * 5.5f * Time.deltaTime;
        }
    }

    private void MovementManagement(float horizontal, float vertical, bool sneaking)
    {
        // Set the Sneaking parameter based on the sneak input
        anim.SetBool("Sneaking", sneaking);

        // If there is axis input...
        if (horizontal != 0f || vertical != 0f)
        {
            // ... set the character's rotation and set the Speed parameter to 5.5f.
            Rotating(horizontal, vertical);
            anim.SetFloat("Speed", 5.5f, speedDampTime, Time.deltaTime);
        }
        else
        {
            // Otherwise, set the Speed parameter to 0.
            anim.SetFloat("Speed", 0);
        }
    }

    private void Rotating(float horizontal, float vertical)
    {
        // Create a new vector using the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

        // Create a rotation based on this new vector, assuming the up direction is the global y-axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Create a new rotation that gradually increases from the character's rotation to the target rotation.
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);

        // Set the character's rotation to the new rotation.
        transform.rotation = newRotation;
    }
}