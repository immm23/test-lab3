using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class ItemsRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Item item = new()
            {
                Name = "test 2"
            };

            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                ItemsRepository repository = new(context);
                await repository.AddAsync(item);

                context.Items.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Item item = new();

            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                ItemsRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Item createdItem = new()
            {
                Name = "test 1",
                Id = 2
            };
            Item updateItem = new()
            {
                Name = "test updated 1",
                Id = 2
            };

            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Items.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                ItemsRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Items.Should().Contain(item => item.Name == updateItem.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Item item = new()
            {
                Name = "test updated 1",
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                ItemsRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Item item = new()
            {
                Name = "test delete",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Items.AddAsync(item);
                await context.SaveChangesAsync();

                ItemsRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Items.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                ItemsRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Item item = new()
            {
                Name = "test delete",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(ItemsRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Items.AddAsync(item);
                await context.SaveChangesAsync();

                ItemsRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
            }
        }
    }
}
