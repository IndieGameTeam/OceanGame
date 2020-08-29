public class BonusHealthUp : PickupObject
{
    public int healthUp = 1;

    protected override void OnPickup(GameManager manager)
    {
        manager.HealthCount += healthUp;
    }
}
