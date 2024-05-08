using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class RacesRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Race item = new()
            {
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
                AgeDescription= "test"
            };

            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                RacesRepository repository = new(context);
                await repository.AddAsync(item);

                context.Races.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Race item = new();

            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                RacesRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Language language = new Language()
            {
                    Name = "test2",
                    Id = 2
            };

            Race createdItem = new()
            {
                NativeLanguages = new()
                {
                    new()
                    {
                        Name = "test1",
                        Id = 1
                    }
                },
                Names = new()
                {
                    new()
                    {
                        RaceName = "test1",
                    }
                },
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
                AgeDescription = "test",
                Id = 2
            };
            Race updateItem = new()
            {
                NativeLanguages = new()
                {
                    new()
                    {
                        Name = "test2",
                        Id = 2
                    }
                },
                Names = new()
                {
                    new()
                    {
                        RaceName = "test2",
                    }
                },
                Name = "test udapted 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Constitution,
                    Amount = 100,
                },
                Alignment = new()
                {
                    AlignmentSideId = AlignmentSideId.Neutral,
                    AlignmentStrengthId = AlignmentStrengthId.Chaotic
                },
                SizeDescription = "test2",
                Description = "test2",
                ImagePath = "test2",
                AgeDescription = "test2",
                Id = 2
            };

            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Languages.AddAsync(language);
                await context.SaveChangesAsync();
                context.Entry(language).State = EntityState.Detached;

                await context.Races.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                RacesRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Races.Should().Contain(item => item.Name == updateItem.Name);
                context.Races.Should().Contain(item => item.RaceBonus.AbilityId == 
                    updateItem.RaceBonus.AbilityId);
                context.Races.Should().Contain(item => item.Alignment.AlignmentSideId ==
                    updateItem.Alignment.AlignmentSideId);
                context.Races.Should().Contain(item => item.Alignment.AlignmentStrengthId == 
                    updateItem.Alignment.AlignmentStrengthId);
                context.Races.First().NativeLanguages.Should().Contain(item => item.Id == 2);
                context.Races.First().Names.Should().Contain(item => item.RaceName == "test2");
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Race item = new()
            {
                Name = "test udapted 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Constitution,
                    Amount = 100,
                },
                Alignment = new()
                {
                    AlignmentSideId = AlignmentSideId.Neutral,
                    AlignmentStrengthId = AlignmentStrengthId.Chaotic
                },
                SizeDescription = "test2",
                Description = "test2",
                ImagePath = "test2",
                AgeDescription = "test2",
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                RacesRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Race item = new()
            {
                Name = "test udapted 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Constitution,
                    Amount = 100,
                },
                Alignment = new()
                {
                    AlignmentSideId = AlignmentSideId.Neutral,
                    AlignmentStrengthId = AlignmentStrengthId.Chaotic
                },
                SizeDescription = "test2",
                Description = "test2",
                ImagePath = "test2",
                AgeDescription = "test2",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Races.AddAsync(item);
                await context.SaveChangesAsync();

                RacesRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Races.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                RacesRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Race item = new()
            {
                Name = "test udapted 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Constitution,
                    Amount = 100,
                },
                Alignment = new()
                {
                    AlignmentSideId = AlignmentSideId.Neutral,
                    AlignmentStrengthId = AlignmentStrengthId.Chaotic
                },
                SizeDescription = "test2",
                Description = "test2",
                ImagePath = "test2",
                AgeDescription = "test2",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(RacesRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Races.AddAsync(item);
                await context.SaveChangesAsync();

                RacesRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
                all.First().Names.Should().NotBeNull();
                all.First().RaceBonus.Should().NotBeNull();
                all.First().Alignment.Should().NotBeNull();
                all.First().NativeLanguages.Should().NotBeNull();
            }
        }
    }
}
