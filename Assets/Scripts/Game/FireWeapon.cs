public class FireWeapon : Weapon
{
    public override void Contact()
    {
        Destroy(gameObject);
        base.Contact();
    }
}
