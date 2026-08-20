using UnityEngine;

public class Pulpit : MonoBehaviour
{
    public System.Action<Pulpit> OnDestroyed;

    private float lifeTime;
    private float timer;
    private bool destroyed;

    public void Initialize(float lifeTimeSeconds)
    {
        lifeTime = lifeTimeSeconds;
        timer = 0f;
    }

    void Update()
    {
        if (destroyed) return;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        if (destroyed) return;
        destroyed = true;
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    // Works whether the Pulpit's collider is a solid collider (OnCollisionEnter)
    // or a trigger (OnTriggerEnter) - use whichever setup you prefer in the editor.
    void OnCollisionEnter(Collision collision) => TryRegisterPlayer(collision.collider);
    void OnTriggerEnter(Collider other) => TryRegisterPlayer(other);

    void TryRegisterPlayer(Collider col)
    {
        if (destroyed) return;
        if (!col.CompareTag("Player")) return;

        PlayerController player = col.GetComponent<PlayerController>();
        player?.OnLandedOnPulpit(this);
    }
}
