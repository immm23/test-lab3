using DndPersonality.DAL.Repositories;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndPersonality.Tests.RepositoryTests
{
    public class LanguagesRepositoryTests
    {
        [Fact]
        public async Task AddAsync_CorrectEntity_ShouldAddToContext()
        {
            Language item = new()
            {
                Name = "test 2"
            };

            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(AddAsync_CorrectEntity_ShouldAddToContext)))
            {
                LanguagesRepository repository = new(context);
                await repository.AddAsync(item);

                context.Languages.Should().Contain(item);
            }
        }

        [Fact]
        public async Task AddAsync_IncorrectEntity_ShouldThrowException()
        {
            Language item = new();

            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(AddAsync_IncorrectEntity_ShouldThrowException)))
            {
                LanguagesRepository repository = new(context);

                await repository.Invoking(x => x.AddAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task UpdateAsync_CorrectEntity_ShouldUpdate()
        {
            Language createdItem = new()
            {
                Name = "test 1",
                Id = 2
            };
            Language updateItem = new()
            {
                Name = "test updated 1",
                Id = 2
            };

            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(UpdateAsync_CorrectEntity_ShouldUpdate)))
            {
                await context.Languages.AddAsync(createdItem);
                await context.SaveChangesAsync();
                context.Entry(createdItem).State = EntityState.Detached;

                LanguagesRepository repository = new(context);
                await repository.UpdateAsync(updateItem);

                context.Languages.Should().Contain(item => item.Name == updateItem.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_IncorrectEntity_ShouldThrowException()
        {
            Language item = new()
            {
                Name = "test updated 1",
                Id = -1
            };

            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(UpdateAsync_IncorrectEntity_ShouldThrowException)))
            {
                LanguagesRepository repository = new(context);

                await repository.Invoking(p => p.UpdateAsync(item))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task RemoveAsync_CorrectEntity_ShouldRemove()
        {
            Language item = new()
            {
                Name = "test delete",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(RemoveAsync_CorrectEntity_ShouldRemove)))
            {
                await context.Languages.AddAsync(item);
                await context.SaveChangesAsync();

                LanguagesRepository repository = new(context);
                await repository.RemoveAsync(1);

                context.Languages.Should().NotContain(p => p.Id == 1);
            }
        }

        [Fact]
        public async Task RemoveAsync_IncorrectEntity_ShouldThrowException()
        {
            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(RemoveAsync_IncorrectEntity_ShouldThrowException)))
            {
                LanguagesRepository repository = new(context);

                await repository.Invoking(p => p.RemoveAsync(-1))
                    .Should().ThrowAsync<Exception>();
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnAllWithIncluded()
        {
            Language item = new()
            {
                Name = "test delete",
                Id = 1
            };

            using (var context = TestContext.CreateContext(nameof(LanguagesRepositoryTests) +
                nameof(AllAsync_ShouldReturnAllWithIncluded)))
            {
                await context.Languages.AddAsync(item);
                await context.SaveChangesAsync();

                LanguagesRepository repository = new(context);
                var all = await repository.AllAsync();

                all.Should().HaveCount(1);
            }
        }
    }
}
