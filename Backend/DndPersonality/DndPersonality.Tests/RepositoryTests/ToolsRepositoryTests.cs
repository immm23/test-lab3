using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class ToolsRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Tool item = new()
            {
                Name = "test 2",
                Price = 5
            };

            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                ToolsRepository repository = new(context);
                await repository.AddAsync(item);

                context.Tools.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Tool item = new();

            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                ToolsRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Tool createdItem = new()
            {
                Name = "test 1",
                Price = 1,
                Id = 2
            };
            Tool updateItem = new()
            {
                Name = "test updated 1",
                Id = 2,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Tools.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                ToolsRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Tools.Should().Contain(item => item.Name == updateItem.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Tool item = new()
            {
                Name = "test updated 1",
                Id = -1,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                ToolsRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Tool item = new()
            {
                Name = "test delete",
                Id = 1,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Tools.AddAsync(item);
                await context.SaveChangesAsync();

                ToolsRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Tools.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                ToolsRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Tool item = new()
            {
                Name = "test delete",
                Id = 1,
                Price = 6
            };

            using (var context = TestContext.CreateContext(nameof(ToolsRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Tools.AddAsync(item);
                await context.SaveChangesAsync();

                ToolsRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
            }
        }
    }
}
