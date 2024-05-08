using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class ClassesRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            CharacterClass item = new()
            {
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

            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                ClassesRepository repository = new(context);
                await repository.AddAsync(item);

                context.Classes.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            CharacterClass item = new();

            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                ClassesRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Item item = new()
            {
                Id = 2,
                Name = "item2"
            };
            Tool tool = new()
            {
                Id = 2,
                Name = "tool2",
            };
            Skill skill = new()
            {
                Id = 2,
                Name = "skill2"
            };
            List<Weapon> weapons = new()
            {
                new()
                {
                    Id = 3,
                    Name = "weapon3",
                    WeaponTypeId = WeaponTypeId.SimpleRanged,
                    DamageTypeId = DamageTypeId.Piercing,
                    Damage = "damage",
                    Price = 12
                },
                new()
                {
                    Id = 4,
                    Name = "weapon4",
                    WeaponTypeId = WeaponTypeId.SimpleRanged,
                    DamageTypeId = DamageTypeId.Piercing,
                    Damage = "damage",
                    Price = 12
                }
            };
            CharacterClass createdItem = new()
            {
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
                Tools = new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "tool1"
                    }
                },
                Skills = new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "skill1"
                    }
                },
                StartItems = new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "item1"
                    }
                },
                WeaponSelectors = new()
                {
                    new()
                    {
                        Weapons = new()
                        {
                            new()
                            {
                                Id = 1,
                                Name = "weapon1",
                                WeaponTypeId = WeaponTypeId.SimpleRanged,
                                DamageTypeId = DamageTypeId.Piercing,
                                Damage = "damage",
                                Price = 12
                            },
                            new()
                            {
                                Id = 2,
                                Name = "weapon2",
                                WeaponTypeId = WeaponTypeId.SimpleRanged,
                                DamageTypeId = DamageTypeId.Piercing,
                                Damage = "damage",
                                Price = 12
                            }
                        }
                    }
                },
                Id = 1
            };
            CharacterClass updateItem = new()
            {
                Name = "test 2",
                Description = "description",
                PrimaryAbilityId = AbilityId.Dexterity,
                SavingThrowId = AbilityId.Strength,
                HitDice = "test",
                StartItems = new()
                {
                    new()
                    {
                        Id = 2,
                    }
                },
                ArmorTypes = new()
                {
                    new()
                    {
                        ArmorTypeId = ArmorTypeId.Shield
                    }
                },
                WeaponTypes = new()
                {
                    new()
                    {
                        WeaponTypeId = WeaponTypeId.SimpleRanged,
                    }
                },
                Tools = new()
                {
                    new()
                    {
                        Id = 2
                    }
                },
                Skills = new()
                {
                    new()
                    {
                        Id = 2
                    }
                },
                WeaponSelectors = new()
                {
                    new()
                    {
                        Weapons = new()
                        {
                            new()
                            {
                                Id = 3
                            },
                            new()
                            {
                                Id = 4
                            }
                        }
                    }
                },
                HitPoints = new()
                {
                    AbilityId = AbilityId.Dexterity,
                    Default = 5
                },
                ImagePath = "imagePath",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Classes.AddAsync(createdItem);
                await context.Skills.AddAsync(skill);
                await context.Tools.AddAsync(tool);
                await context.Items.AddAsync(item);
                await context.Weapons.AddRangeAsync(weapons);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;
                context.Entry(skill).State = EntityState.Detached;
                context.Entry(tool).State = EntityState.Detached;
                context.Entry(item).State = EntityState.Detached;
                foreach (var weapon in weapons)
                {
                    context.Entry(weapon).State = EntityState.Detached;
                }

                ClassesRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Classes.Should().Contain(item => item.Name == updateItem.Name);
                context.Classes.First().PrimaryAbilityId.Should().Be(updateItem.PrimaryAbilityId);
                context.Classes.First().SavingThrowId.Should().Be(updateItem.SavingThrowId);
                context.Classes.First().HitPoints.AbilityId.Should()
                    .Be(updateItem.HitPoints.AbilityId);
                context.Classes.First().Tools.Should().Contain(p => p.Id == 2);
                context.Classes.First().StartItems.Should().Contain(p => p.Id == 2);
                context.Classes.First().Skills.Should().Contain(p => p.Id == 2);
                context.Classes.First().WeaponSelectors.First().Weapons
                    .Should().Contain(p => p.Id == 4);
                context.Classes.First().WeaponSelectors.First().Weapons
                    .Should().Contain(p => p.Id == 3);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            CharacterClass item = new()
            {
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
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                ClassesRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            CharacterClass item = new()
            {
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
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Classes.AddAsync(item);
                await context.SaveChangesAsync();

                ClassesRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Classes.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                ClassesRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            CharacterClass item = new()
            {
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
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(ClassesRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Classes.AddAsync(item);
                await context.SaveChangesAsync();

                ClassesRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
            }
        }
    }
}
