using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class ArmorsRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Armor item = new()
            {
                Name = "test 2",
                Price = 5,
                BasePoints = 10,
                ArmorTypeId = ArmorTypeId.HeavyArmor,
                BonusAbilityId = AbilityId.Constitution
            };

            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) + 
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                ArmorsRepository repository = new(context);
                await repository.AddAsync(item);

                context.Armors.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Armor item = new();

            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                ArmorsRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Armor createdItem = new()
            {
                ArmorTypeId = ArmorTypeId.Shield,
                Name = "test 1",
                BonusAbilityId = AbilityId.Constitution,
                BasePoints = 6,
                Price = 1,
                Id = 2
            };
            Armor updateItem = new()
            {
                ArmorTypeId = ArmorTypeId.HeavyArmor,
                Name = "test updated 1",
                Id = 2,
                BonusAbilityId = AbilityId.Constitution,
                BasePoints = 11,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Armors.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                ArmorsRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Armors.Should().Contain(item => item.Name == updateItem.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Armor item = new()
            {
                ArmorTypeId = ArmorTypeId.HeavyArmor,
                Name = "test updated 1",
                Id = -1,
                BonusAbilityId = AbilityId.Constitution,
                BasePoints = 11,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                ArmorsRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Armor item = new()
            {
                ArmorTypeId = ArmorTypeId.HeavyArmor,
                Name = "test delete",
                Id = 1,
                BonusAbilityId = AbilityId.Constitution,
                BasePoints = 10,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Armors.AddAsync(item);
                await context.SaveChangesAsync();

                ArmorsRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Armors.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                ArmorsRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Armor item = new()
            {
                ArmorTypeId = ArmorTypeId.HeavyArmor,
                Name = "test delete",
                Id = 1,
                BonusAbilityId = AbilityId.Constitution,
                BasePoints = 10,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ArmorsRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Armors.AddAsync(item);
                await context.SaveChangesAsync();

                ArmorsRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
                all.First().ArmorType.Should().NotBeNull();
                all.First().BonusAbility.Should().NotBeNull();
            }
        }
    }
}
