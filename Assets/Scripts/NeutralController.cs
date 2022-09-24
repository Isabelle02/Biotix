public class NeutralController : TeamController
{
    public NeutralController(int teamId) : base(teamId)
    {
        LaboratoryData = new NeutralData();
    }
}