using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class SkillsRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Skill item = new()
            {
                Name = "test 2"
            };

            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                SkillsRepository repository = new(context);
                await repository.AddAsync(item);

                context.Skills.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Skill item = new();

            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                SkillsRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Skill createdItem = new()
            {
                Name = "test 1",
                Id = 2
            };
            Skill updateItem = new()
            {
                Name = "test updated 1",
                Id = 2
            };

            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Skills.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                SkillsRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Skills.Should().Contain(item => item.Name == updateItem.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Skill item = new()
            {
                Name = "test updated 1",
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                SkillsRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Skill item = new()
            {
                Name = "test delete",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Skills.AddAsync(item);
                await context.SaveChangesAsync();

                SkillsRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Skills.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                SkillsRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Skill item = new()
            {
                Name = "test delete",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(SkillsRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Skills.AddAsync(item);
                await context.SaveChangesAsync();

                SkillsRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
            }
        }
    }
}
