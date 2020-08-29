public class Coin : Bonus, IPickup
{
    public void Pickup()
    {
        OnPickUp();
    }

    public override void OnPickUp()
    {
        base.OnPickUp();

        //Added coin
    }
}
