using UnityEngine;

public enum AfterEffects
{
    Cold,
    Fire,
    Push,
    None
}
public class Weapon : MonoBehaviour
{
    public int damage = 20;
    public int speed = 20;
    private int impactCount = 0;
    private float movement;
    private bool hasLaunched;
    private bool destroyed = false;
    private Vector3 travelDirection;
    public AfterEffects afterEffects = AfterEffects.None;

    public virtual void Fire(float LifeTime, AfterEffects effects, Vector3 aimDirection)
    {
        if (destroyed) return;
        afterEffects = effects;
        travelDirection = aimDirection;
        hasLaunched = true;
        transform.parent = null;
        Destroy(gameObject, LifeTime);
    }

    private void Update()
    {
        if (hasLaunched)
            Travel();
    }

    private void Travel()
    {
        movement = Time.deltaTime * speed;
        transform.Translate(travelDirection.normalized * movement, Space.World);
    }

    public virtual void Contact()
    {
        destroyed = true;
    }

}
