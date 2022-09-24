public class NeutralData : LaboratoryData
{
    public override float Defence => (BaseDefence * 5 + LevelManager.CurrentLevelIndex) / 5;
}