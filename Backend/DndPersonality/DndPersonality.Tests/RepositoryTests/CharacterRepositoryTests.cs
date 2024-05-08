using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using DndPersonality.Models.InputModels.CharaterGenerator;
using FluentAssertions;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class CharacterRepositoryTests
    {
        [Fact]
        public async Task ComposeOriginAsync_IncorrectOrigin_ShouldThrowException()
        {
            OriginInputModel input = new();

            using (var context = TestContext.CreateContext(nameof(CharacterRepositoryTests)+
                nameof(ComposeOriginAsync_IncorrectOrigin_ShouldThrowException)))
            {
                CharacterRepository repository = new(context);
                await repository.Invoking(p => p.ComposeOriginAsync(input))
                    .Should().ThrowAsync<KeyNotFoundException>();
            }
        }

        [Fact]
        public async Task ComposeOriginAsync_CorrectOrigin_ShouldReturnOrigin()
        {
            CharacterClass characterClass = new()
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
                ImagePath = "imagePath"
            };
            Subrace subrace = new()
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
            Race race = new()
            {
                Id = 1,
                Name = "test 2",
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
                AgeDescription = "test"
            };
            OriginInputModel originInput = new()
            {
                RaceId = 1,
                ClassId = 1,
                SubraceId = 1
            };

            using (var context = TestContext.CreateContext(nameof(CharacterRepositoryTests) + 
                nameof(ComposeOriginAsync_CorrectOrigin_ShouldReturnOrigin)))
            {
                await context.Races.AddAsync(race);
                await context.Subraces.AddAsync(subrace);
                await context.Classes.AddAsync(characterClass);
                await context.SaveChangesAsync();

                CharacterRepository repository = new(context);
                var origin = await repository.ComposeOriginAsync(originInput);

                origin.Subrace.Should().NotBeNull();
                origin.Race.Names.Should().NotBeNull();
                origin.Race.RaceBonus.Should().NotBeNull();
                origin.Race.Alignment.Should().NotBeNull();
                origin.Race.NativeLanguages.Should().NotBeNull();
                origin.CharacterClass.PrimaryAbility.Should().NotBeNull();
                origin.CharacterClass.SavingThrow.Should().NotBeNull();
                origin.CharacterClass.HitPoints.Should().NotBeNull();
                origin.CharacterClass.ArmorTypes.Should().NotBeNull();
                origin.CharacterClass.WeaponTypes.Should().NotBeNull();
                origin.CharacterClass.Tools.Should().NotBeNull();
                origin.CharacterClass.Skills.Should().NotBeNull();
                origin.CharacterClass.WeaponSelectors.Should().NotBeNull();
                origin.CharacterClass.StartItems.Should().NotBeNull();
            }
        }
    }
}
