using UnityEngine;

public class PlayerWeapon : Weapon
{
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Fire(float LifeTime, AfterEffects effects, Vector3 aimDirection)
    {
        try
        {
            _animator.SetBool("isFly", true);
            base.Fire(LifeTime, effects, aimDirection);
        }
        catch
        {
           
        }
    }

    public override void Contact()
    {
        base.Contact();
        try
        {
            _animator.SetBool("isDie", true);
            Invoke(nameof(DeleteFromScene), 0.09f);
        }
        catch
        {
           
        }
    }

    private void DeleteFromScene()
    {
        Destroy(gameObject);
    }
}
