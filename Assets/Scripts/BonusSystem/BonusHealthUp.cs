public class BonusHealthUp : Bonus, IPickup
{
    public int healthUp = 1;

    public void Pickup()
    {
        OnPickUp();
        Destroy(gameObject);
    }

    public override void OnPickUp()
    {
        base.OnPickUp();

        FindObjectOfType<GameManager>().HealthCount += healthUp;
    }
}
