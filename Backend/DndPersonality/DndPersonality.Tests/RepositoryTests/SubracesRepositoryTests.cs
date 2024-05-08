using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class SubracesRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Subrace item = new()
            {
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

            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                await context.SaveChangesAsync();

                SubracesRepository repository = new(context);
                await repository.AddAsync(item);

                context.Subraces.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Subrace item = new();

            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                SubracesRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Subrace createdItem = new()
            {
                Name = "test 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Strength,
                    Amount = 100
                },
                SpecialAbility = "ability",
                Description = "description",
                ImagePath = "imagePath",
                RaceId = 0,
                Id = 1
            };
            Subrace updateItem = new()
            {
                Name = "test udpated 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Intelligence,
                    Amount = 34
                },
                SpecialAbility = "ability2",
                Description = "description2",
                ImagePath = "imagePath2",
                RaceId = 0, 
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Subraces.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                SubracesRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Subraces.Should().Contain(item => item.Name == updateItem.Name);
                context.Subraces.Should().Contain(item => item.RaceBonus.AbilityId == updateItem.RaceBonus.AbilityId);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Subrace item = new()
            {
                Name = "test udpated 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Intelligence,
                    Amount = 34
                },
                SpecialAbility = "ability2",
                Description = "description2",
                ImagePath = "imagePath2",
                RaceId = 0,
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                SubracesRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Subrace item = new()
            {
                Name = "test udpated 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Intelligence,
                    Amount = 34
                },
                SpecialAbility = "ability2",
                Description = "description2",
                ImagePath = "imagePath2",
                RaceId = 0,
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Subraces.AddAsync(item);
                await context.SaveChangesAsync();

                SubracesRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Subraces.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                SubracesRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Subrace item = new()
            {
                Name = "test udpated 2",
                RaceBonus = new()
                {
                    AbilityId = AbilityId.Intelligence,
                    Amount = 34
                },
                SpecialAbility = "ability2",
                Description = "description2",
                ImagePath = "imagePath2",
                RaceId = 0
            };
           
            using (var context = TestContext.CreateContext(nameof(SubracesRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Subraces.AddAsync(item);
                await context.SaveChangesAsync();

                SubracesRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
                all.First().RaceBonus.Should().NotBeNull();
            }
        }
    }
}
