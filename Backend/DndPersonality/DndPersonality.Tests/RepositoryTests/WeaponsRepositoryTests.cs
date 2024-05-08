using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class WeaponsRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Weapon item = new()
            {
                Name = "test 2",
                Damage = "damage",
                Price = 40,
                DamageTypeId = DamageTypeId.Bludgeoning,
                WeaponTypeId = WeaponTypeId.SimpleRanged
            };

            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                WeaponsRepository repository = new(context);
                await repository.AddAsync(item);

                context.Weapons.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Weapon item = new();

            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                WeaponsRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Weapon createdItem = new()
            {
                Name = "test 2",
                Damage = "damage",
                Price = 40,
                DamageTypeId = DamageTypeId.Bludgeoning,
                WeaponTypeId = WeaponTypeId.SimpleRanged,
                Id = 2
            };
            Weapon updateItem = new()
            {
                Name = "test 2 updated",
                Damage = "damage updated",
                Price = 45,
                DamageTypeId = DamageTypeId.Slashing,
                WeaponTypeId = WeaponTypeId.MartialRanged,
                Id = 2
            };

            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Weapons.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                WeaponsRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Weapons.Should().Contain(item => item.Name == updateItem.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Weapon item = new()
            {
                Name = "test 2",
                Damage = "damage",
                Price = 40,
                DamageTypeId = DamageTypeId.Bludgeoning,
                WeaponTypeId = WeaponTypeId.SimpleRanged,
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                WeaponsRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Weapon item = new()
            {
                Name = "test 2",
                Damage = "damage",
                Price = 40,
                DamageTypeId = DamageTypeId.Bludgeoning,
                WeaponTypeId = WeaponTypeId.SimpleRanged,
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Weapons.AddAsync(item);
                await context.SaveChangesAsync();

                WeaponsRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Weapons.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                WeaponsRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Weapon item = new()
            {
                Name = "test 2",
                Damage = "damage",
                Price = 40,
                DamageTypeId = DamageTypeId.Bludgeoning,
                WeaponTypeId = WeaponTypeId.SimpleRanged,
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(WeaponsRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Weapons.AddAsync(item);
                await context.SaveChangesAsync();

                WeaponsRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
                all.First().WeaponType.Should().NotBeNull();
                all.First().DamageType.Should().NotBeNull();
            }
        }
    }
}
