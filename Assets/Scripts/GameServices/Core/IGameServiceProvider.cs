public interface IGameServiceProvider
{
    IObjectPoolService ObjectPoolService { get; }
    IGameEventService GameEventService { get; }
}