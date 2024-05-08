namespace DndPersonality.Models.InputModels.Components
{
    public class WeaponSelectorInputModel
    {
        public int Id { get; set; }
        public List<WeaponInputModel> Weapons { get; set; } = 
            new List<WeaponInputModel>();
    }
}
