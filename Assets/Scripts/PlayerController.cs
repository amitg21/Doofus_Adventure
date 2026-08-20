using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("If Doofus's Y position drops below this, he has fallen off -> Game Over.")]
    public float fallYThreshold = -10f;

    private Rigidbody rb;
    private float speed;
    private Pulpit currentPulpit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Freeze rotation so the cube doesn't topple over when it bumps pulpit edges.
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Start()
    {
        speed = GameManager.Instance.Diary.player_data.speed;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        HandleMovement();

        if (transform.position.y < fallYThreshold)
        {
            Fall();
        }
    }

    void HandleMovement()
    {
        // Horizontal/Vertical are Unity's default input axes and already map to
        // both WASD and the Arrow Keys out of the box - no Input Manager changes needed.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == 0f && v == 0f) return;

        Vector3 move = new Vector3(h, 0f, v).normalized * speed * Time.deltaTime;
        // Move via transform, not physics force, so speed matches the diary's
        // "speed" value exactly and predictably (units/second).
        rb.MovePosition(transform.position + move);
    }

    // Called by Pulpit.cs when this collider lands on / stays on a pulpit.
    public void OnLandedOnPulpit(Pulpit pulpit)
    {
        if (pulpit == currentPulpit) return; // same pulpit as before - no new score
        currentPulpit = pulpit;
        GameManager.Instance.AddScore(1);
    }

    void Fall()
    {
        GameManager.Instance.EndGame();
    }
}
