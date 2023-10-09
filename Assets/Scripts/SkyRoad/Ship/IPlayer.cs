namespace SkyRoad.Ship
{
    public interface IPlayer
    {
        bool Boosted { get; set; }
        void EnableAllMeshes(bool enable);
        void ChangeState(ShipStates state);
    }
}