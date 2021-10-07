namespace Code.Interfaces.Models
{
    public interface IUnitMeleeModel: IUnitModel
    {
        float Cooldown { get; set; }
    }
}