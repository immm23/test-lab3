using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using DndPersonality.Models.DALModels;
using Xunit;
using DndPersonality.CharacterGenerator;
using FluentAssertions;
using DndPersonality.CharacterGenerator.Selectors;
using DndPersonality.Models.InputModels.Components;
using DndPersonality.Models.InputModels.CharaterGenerator;
using System.Text.Json;

namespace DndPersonality.Tests
{
    public class ConstructorTests
    {
        private readonly CharacterClass CharacterClass = new()
        {
            Id = 1,
            Name = "test 2",
            Description = "description",
            PrimaryAbilityId = AbilityId.Dexterity,
            SavingThrowId = AbilityId.Strength,
            HitDice = "test",
            HitPoints = new()
            {
                AbilityId = AbilityId.Dexterity,
                Default = 5
            },
            ImagePath = "imagePath",
            Skills = new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "test"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "test2"
                    }
                },
            SelectedSkills = 1,
            WeaponSelectors = new()
                {
                    new()
                    {
                        Weapons = new()
                        {
                            new()
                            {
                                Name = "weapon 1",
                                Id = 1
                            },
                            new()
                            {
                                Name = "weapon 2",
                                Id = 2
                            }
                        }
                    }
                }
        };
        private readonly Subrace Subrace = new()
        {
            Id = 1,
            Name = "test 2",
            RaceBonus = new()
            {
                AbilityId = AbilityId.Strength,
                Amount = 100
            },
            SpecialAbility = "ability",
            Description = "description",
            ImagePath = "imagePath",
            RaceId = 0
        };
        private readonly Race Race = new()
        {
            Id = 1,
            Name = "test 2",
            Names = new()
                {
                    new()
                    {
                        Id = 1,
                        RaceName = "Name"
                    },
                    new()
                    {
                        Id = 2,
                        RaceName = "Name"
                    }
                },
            RaceBonus = new()
            {
                AbilityId = AbilityId.Strength,
                Amount = 100,
            },
            Alignment = new()
            {
                AlignmentSideId = AlignmentSideId.Any,
                AlignmentStrengthId = AlignmentStrengthId.Lawful
            },
            SizeDescription = "test",
            Description = "test",
            ImagePath = "test",
            AgeDescription = "test",
            AdditionalLanguages = 2
        };
        private readonly List<Language> Languages = new()
            {
                new()
                {
                    Id = 1,
                    Name = "test1",
                },
                new()
                {
                    Id = 2,
                    Name = "test1",
                }
            };

        [Fact]
        public void GenerateConstructor_ShouldRetrunSelectors()
        {
            Origin origin = new(Race, CharacterClass, Subrace);
            Constructor constructor = new(origin, Languages);

            var selectors = constructor.Selectors;

            selectors.Should().Contain(selector => selector.Name == nameof(Name));
            var nameSelector = selectors.First(p => p.Name == nameof(Name));
            nameSelector.Should().BeOfType<ListSelector<Name>>();
            ((ListSelector<Name>)nameSelector).Items.Count.Should().Be(2);
            ((ListSelector<Name>)nameSelector).Amount.Should().Be(1);

            selectors.Should().Contain(selector => selector.Name == nameof(Skill));
            var skillSelector = selectors.First(p => p.Name == nameof(Skill));
            skillSelector.Should().BeOfType<ListSelector<Skill>>();
            ((ListSelector<Skill>)skillSelector).Items.Count.Should().Be(2);
            ((ListSelector<Skill>)skillSelector).Amount.Should().Be(1);

            selectors.Should().Contain(selector => selector.Name == nameof(Language));
            var langugageSelector = selectors.First(p => p.Name == nameof(Language));
            langugageSelector.Should().BeOfType<ListSelector<Language>>();
            ((ListSelector<Language>)langugageSelector).Items.Count.Should().Be(2);
            ((ListSelector<Language>)langugageSelector).Amount.Should().Be(2);

            selectors.Should().Contain(selector => selector.Name == nameof(WeaponSelector));
            var weaponSelector = selectors.First(p => p.Name == nameof(WeaponSelector));
            weaponSelector.Should().BeOfType<ListSelector<Weapon>>();
            ((ListSelector<Weapon>)weaponSelector).Items.Count.Should().Be(2);
            ((ListSelector<Weapon>)weaponSelector).Amount.Should().Be(1);

            selectors.Should().Contain(selector => selector.Name == nameof(Alignment));
            var alignmentSelector = selectors.First(p => p.Name == nameof(Alignment));
            alignmentSelector.Should().BeOfType<ItemSpecifier<AlignmentInputModel>>();

            selectors.Should().Contain(selector => selector.Name == nameof(Character.Weight));
            var weightSelector = selectors.First(p => p.Name == nameof(Character.Weight));
            weightSelector.Should().BeOfType<ItemSpecifier<int>>();

            selectors.Should().Contain(selector => selector.Name == nameof(Character.Height));
            var heightSelector = selectors.First(p => p.Name == nameof(Character.Height));
            heightSelector.Should().BeOfType<ItemSpecifier<int>>();

            selectors.Should().Contain(selector => selector.Name == nameof(Character.Age));
            var ageSelector = selectors.First(p => p.Name == nameof(Character.Age));
            ageSelector.Should().BeOfType<ItemSpecifier<int>>();

            selectors.Should().Contain(selector => selector.Name == nameof(Ability));
            var abilitySelector = selectors.First(p => p.Name == nameof(Ability));
            abilitySelector.Should().BeOfType<ItemSpecifier<List<int>>>();
            ((ItemSpecifier<List<int>>)abilitySelector).Item.First().Should()
                .Be((int)CharacterClass.PrimaryAbilityId);
        }

        [Fact]
        public void ValidateSelectionModel_CorrectSelectors_ShouldSelect()
        {
            SelectionInputModel selection = new()
            {
                ListSelectedItems = new()
                {
                    new()
                    {
                        Id = 0,
                        Selections = new()
                        {
                            1
                        }
                    },
                    new()
                    {
                        Id = 1,
                        Selections = new()
                        {
                            2
                        }
                    },
                    new()
                    {
                        Id = 2,
                        Selections = new()
                        {
                            1, 2
                        }
                    },
                    new()
                    {
                        Id = 3,
                        Selections = new()
                        {
                            2
                        }
                    }
                },
                SpecifiedItems = new()
                {
                    new()
                    {
                        Id = 4,
                        Selection = JsonSerializer.Serialize(new AlignmentInputModel
                        {
                            AlignmentSideId = AlignmentSideId.Good,
                            AlignmentStrengthId = AlignmentStrengthId.Lawful
                        })
                    },
                    new()
                    {
                        Id = 5,
                        Selection = JsonSerializer.Serialize(15)
                    },
                    new()
                    {
                        Id = 6,
                        Selection = JsonSerializer.Serialize(30)
                    },
                    new()
                    {
                        Id = 7,
                        Selection = JsonSerializer.Serialize(76)
                    },
                    new()
                    {
                        Id = 8,
                        Selection = JsonSerializer.Serialize(new List<int>()
                        {
                            1, 0, 2, 3, 4, 5
                        })
                    }
                }
            };
            Origin origin = new(Race, CharacterClass, Subrace);
            Constructor constructor = new(origin, Languages);
            var validatedSelectors = constructor.ValidateSelectionModel(selection);

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Name));
            var nameSelector = validatedSelectors.First(p => p.Name == nameof(Name));
            nameSelector.Should().BeOfType<ListSelector<Name>>();
            ((List<Name>)(((ListSelector<Name>)nameSelector).GetSelection())).Count.Should().Be(1);

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Skill));
            var skillSelector = validatedSelectors.First(p => p.Name == nameof(Skill));
            skillSelector.Should().BeOfType<ListSelector<Skill>>();
            ((List<Skill>)(((ListSelector<Skill>)skillSelector).GetSelection())).Count.Should().Be(1);

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Language));
            var langugageSelector = validatedSelectors.First(p => p.Name == nameof(Language));
            langugageSelector.Should().BeOfType<ListSelector<Language>>();
            ((List<Language>)(((ListSelector<Language>)langugageSelector).GetSelection())).Count.Should().Be(2);

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(WeaponSelector));
            var weaponSelector = validatedSelectors.First(p => p.Name == nameof(WeaponSelector));
            weaponSelector.Should().BeOfType<ListSelector<Weapon>>();
            ((List<Weapon>)(((ListSelector<Weapon>)weaponSelector).GetSelection())).Count.Should().Be(1);

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Alignment));
            var alignmentSelector = validatedSelectors.First(p => p.Name == nameof(Alignment));
            (((ItemSpecifier<AlignmentInputModel>)alignmentSelector).GetSelection())
                .Should().BeOfType<AlignmentInputModel>();

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Character.Weight));
            var weightSelector = validatedSelectors.First(p => p.Name == nameof(Character.Weight));
            (((ItemSpecifier<int>)weightSelector).GetSelection())
                .Should().BeOfType<int>();

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Character.Height));
            var heightSelector = validatedSelectors.First(p => p.Name == nameof(Character.Height));
            (((ItemSpecifier<int>)heightSelector).GetSelection())
                .Should().BeOfType<int>();

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Character.Age));
            var ageSelector = validatedSelectors.First(p => p.Name == nameof(Character.Age));
            (((ItemSpecifier<int>)ageSelector).GetSelection())
                .Should().BeOfType<int>();

            validatedSelectors.Should().Contain(selector => selector.Name == nameof(Ability));
            var abilitySelector = validatedSelectors.First(p => p.Name == nameof(Ability));
            (((ItemSpecifier<List<int>>)abilitySelector).GetSelection())
                .Should().BeOfType<List<int>>();
        }

        [Fact]
        public void ValidateSelectionModel_EmptySelection_ShouldThrowException()
        {
            SelectionInputModel selection = new();
            Origin origin = new(Race, CharacterClass, Subrace);
            Constructor constructor = new(origin, Languages);

            constructor.Invoking(p => p.ValidateSelectionModel(selection)).Should()
                .Throw<ArgumentException>();
        }
    }
}
