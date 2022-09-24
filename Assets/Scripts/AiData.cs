public class AiData : LaboratoryData
{
    public override float Attack => (BaseAttack * 10 + LevelManager.CurrentLevelIndex) / 10;
    
    public override float Defence => (BaseDefence * 10 + LevelManager.CurrentLevelIndex) / 10;
    
    public override float Speed => (BaseSpeed * 100 + LevelManager.CurrentLevelIndex) / 100;

    public override float Reproduction => (BaseReproduction * 10 - LevelManager.CurrentLevelIndex) / 10;

    public override int AdditionalInjection => (BaseAdditionalInjection + LevelManager.CurrentLevelIndex) / 10;
}