using DndPersonality.CharacterGenerator.Selectors;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.Components;

namespace DndPersonality.CharacterGenerator
{
    public class Character
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Speed { get; set; }
        public int Age { get; set; }
        public Alignment Alignment { get; set; }
        public HitPoints HitPoints { get; set; }
        public Ability SavingThrow { get; set; }
        public List<Language> Languages { get; set; }
        public List<ArmorType> ArmorTypes { get; set; }
        public List<WeaponType> WeaponTypes { get; set; }
        public List<Tool> Tools { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
        public List<Item> StartItems { get; set; }
        public string? ToolsBonus { get; set; }
        public string? AdditionalBonus { get; set; }
        public int ProficiencyBonus { get; set; }
        public string? DarkVision { get; set; }
        public string HitDice { get; set; }
        public string? AdditionalAbilities { get; set; }
        public string? SpecialAbility { get; set; }
        public Dictionary<AbilityId, int> AbilitiesTable { get; set; }
        public string? GeneratedStory { get; set; }
        public string? GeneratedImageUrl { get; set; }

        private readonly Ability _primaryAbility;

        public Character(Origin origin, List<Selector> selectors)
        {
            return;
            Name = GetSelection<List<Name>>(selectors, nameof(Models.DALModels.Components.Name)).First().RaceName;
            Height = GetSelection<int>(selectors, nameof(Height));
            Weight = GetSelection<int>(selectors, nameof(Weight));
            Age = GetSelection<int>(selectors, nameof(Age));
            Languages = origin.Race.NativeLanguages;
            Languages.AddRange(GetSelection<List<Language>>(selectors, nameof(Language)));
            Skills = GetSelection<List<Skill>>(selectors, nameof(Skill));

            var weaponSelectors = selectors.Where(p => p.Name == nameof(WeaponSelector)).ToList();
            weaponSelectors.ForEach(p =>
            {
                Weapons.AddRange((List<Weapon>)p.GetSelection());
            });

            var alignmentInput = GetSelection<AlignmentInputModel>(selectors, nameof(Models.DALModels.Components.Alignment));
            Alignment = new Alignment()
            {
                AlignmentSide = new()
                {
                    AlignmentSideId = alignmentInput.AlignmentSideId,
                    Name = alignmentInput.AlignmentSideId.ToString()
                },
                AlignmentStrength = new()
                {
                    AlignmentStrengthId = alignmentInput.AlignmentStrengthId,
                    Name = alignmentInput.AlignmentStrengthId.ToString()
                }
            };

            Speed = origin.Race.Speed;
            HitPoints = origin.CharacterClass.HitPoints;
            _primaryAbility = origin.CharacterClass.PrimaryAbility;
            SavingThrow = origin.CharacterClass.SavingThrow;
            ArmorTypes = origin.CharacterClass.ArmorTypes;
            WeaponTypes = origin.CharacterClass.WeaponTypes;
            Tools = origin.CharacterClass.Tools;
            StartItems = origin.CharacterClass.StartItems;
            ToolsBonus = origin.Race.ToolsBonus;
            AdditionalBonus = origin.Race.AdditionalBonus;
            ProficiencyBonus = origin.CharacterClass.ProficiencyBonus;
            DarkVision = origin.Race.DarkVision;
            HitDice = origin.CharacterClass.HitDice;
            AdditionalAbilities = origin.CharacterClass.AdditionalAbilities;
            AbilitiesTable = GenerateAbilities(GetSelection<List<int>>(selectors, nameof(Ability)));

            if (origin.Subrace is not null)
            {
                ApplyRaceBonus(origin.Subrace.RaceBonus);
                SpecialAbility = origin.Subrace.SpecialAbility;
            }

            ApplyRaceBonus(origin.Race.RaceBonus);
        }

        private T GetSelection<T>(List<Selector> selectors, string name)
        {
            return (T)selectors.First(p => p.Name == name).GetSelection();
        }

        private Dictionary<AbilityId, int> GenerateAbilities(List<int> abilities)
        {
            if ((AbilityId)abilities[0] != _primaryAbility.AbilityId)
            {
                throw new ArgumentException("Main ability can not differ from primary ability");
            }

            Dictionary<AbilityId, int> result = new Dictionary<AbilityId, int>();
            List<int> rolls = RollDice();

            for (int i = 0; i < rolls.Count; i++)
            {
                result.Add((AbilityId)abilities[i], rolls[i]);
            }

            return result;
        }

        private List<int> RollDice()
        {
            //according to dnd rules, player should roll 4 dices
            //and select 3 values
            List<int> rolled = new List<int>();
            List<int> tempRolls = new List<int>();
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tempRolls.Add(random.Next(1, 6));
                }

                rolled.Add(tempRolls
                    .OrderByDescending(p => p)
                    .Take(3)
                    .Sum());

                tempRolls.Clear();
            }

            return rolled.OrderByDescending(p => p).ToList();

        }

        private void ApplyRaceBonus(RaceBonus bonus)
        {
            AbilitiesTable[bonus.AbilityId] += bonus.Amount;
        }
    }
}
