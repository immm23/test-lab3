using DndPersonality.CharacterGenerator.Selectors;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using DndPersonality.Models.InputModels.CharaterGenerator;
using DndPersonality.Models.InputModels.Components;


namespace DndPersonality.CharacterGenerator
{
    public class Constructor
    {
        public Race Race { get; set; }
        public CharacterClass CharacterClass { get; set; }
        public Subrace? Subrace { get; set; }

        public readonly List<Selector> Selectors;

        public Constructor(Origin origin, List<Language> languages)
        {
            Race = origin.Race;
            CharacterClass = origin.CharacterClass;
            Subrace = origin.Subrace;

            Selectors = GenerateConstructor(languages);
        }

        public List<Selector> ValidateSelectionModel(SelectionInputModel inputModel)
        {
            List<Selector> selectedSelectors = new List<Selector>();


            foreach (var selector in Selectors)
            {
                var correspondingSelection =
                    inputModel.ListSelectedItems.FirstOrDefault(p => p.Id == selector.Id)?.Selections ??
                    inputModel.SpecifiedItems.FirstOrDefault(p => p.Id == selector.Id)?.Selection;

                //if (correspondingSelection is null)
                //{
                //    throw new ArgumentException($"There is no corresponding selection for {nameof(selector)}");
                //}

                //selector.Select(correspondingSelection);
                //selectedSelectors.Add(selector);

            }
            return selectedSelectors;

        }

        private List<Selector> GenerateConstructor(List<Language> languages)
        {
            return new List<Selector>();
            int selectorId = 0;
            List<Selector> selectors = new List<Selector>();

            selectors.Add(new ListSelector<Name>(
                selectorId++,
                Race.Names,
                1,
                nameof(Name)));
            selectors.Add(new ListSelector<Skill>(
                selectorId++,
                CharacterClass.Skills,
                CharacterClass.SelectedSkills,
                nameof(Skill)));

            selectors.Add(new ListSelector<Language>(
                selectorId++,
                languages,
                Race.AdditionalLanguages,
                nameof(Language)));

            foreach (var weaponSelector in CharacterClass.WeaponSelectors)
            {
                selectors.Add(new ListSelector<Weapon>(
                    selectorId++,
                    weaponSelector.Weapons,
                    1,
                    nameof(WeaponSelector)));
            }

            selectors.Add(new ItemSpecifier<AlignmentInputModel>(
                selectorId++,
                new AlignmentInputModel(),
                nameof(Alignment)));
            selectors.Add(new ItemSpecifier<int>(
                selectorId++,
                0,
                nameof(Character.Weight)));
            selectors.Add(new ItemSpecifier<int>(
                selectorId++,
                0,
                nameof(Character.Height)));
            selectors.Add(new ItemSpecifier<int>(
                selectorId++,
                0,
                nameof(Character.Age)));
            selectors.Add(new ItemSpecifier<List<int>>(
                selectorId++,
                new List<int>() { (int)CharacterClass.PrimaryAbilityId },
                nameof(Ability)));

            return selectors;
        }
    }
}
