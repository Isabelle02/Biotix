public class NeutralController : TeamController
{
    public NeutralController(int teamId) : base(teamId)
    {
    }

    public override void Init()
    {
        SetDefence(LevelManager.CurrentLevelIndex / 5f);
    }

    public override void SetDefence(float rate)
    {
        Defence += rate;
    }
}