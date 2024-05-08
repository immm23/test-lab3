using DndPersonality.CharacterGenerator.Selectors;
using DndPersonality.Models.DALModels.Components;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace DndPersonality.Tests
{
    public class SelectorsTests
    {
        [Fact]
        public void ItemSpecifier_ValidSelection_ShouldSelect()
        {
            ItemSpecifier<int> selector = new(1, 0, "testName");
            string selection = JsonSerializer.Serialize(5);
            
            selector.Select(selection);

            selector.Item.Should().Be(5);
        }

        [Fact]
        public void ItemSpecifier_InvalidSelection_ShouldThrowException()
        {
            ItemSpecifier<string> selector = new(1, "34", "testName");
            string selection = JsonSerializer.Serialize(1);

            selector.Invoking(p => p.Select(selection)).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ItemSpecifier_GetSelection_NotSelected_ShouldThrowException()
        {
            ItemSpecifier<string> selector = new(1, "34", "testName");
            selector.Invoking(p => p.GetSelection()).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ItemSpecifier_GetSelection_Selected_ShouldReturnSelection()
        {
            ItemSpecifier<int> selector = new(1, 0, "testName");
            string selection = JsonSerializer.Serialize(5);

            selector.Select(selection);

            selector.GetSelection().Should().Be(5);
        }

        [Fact]
        public void ListSelector_ValidSelection_ShouldSelect()
        {
            List<Name> names = new()
            {
                new()
                {
                    Id = 1,
                    RaceName = "test"
                },
                new()
                {
                    Id = 2,
                    RaceName = "test2"
                }
            };
            ListSelector<Name> selector = new(1, names, 1, "name" );

            selector.Select(new List<int> { 1 });

            selector.Items.Should().HaveCount(1);
            selector.Items.First().Id.Should().Be(1);
        }

        [Fact]
        public void ListSelector_InvalidSelection_ShouldThrowException()
        {
            List<Name> names = new()
            {
                new()
                {
                    Id = 1,
                    RaceName = "test"
                },
                new()
                {
                    Id = 2,
                    RaceName = "test2"
                }
            };
            ListSelector<Name> selector = new(1, names, 1, "name");

            selector.Invoking(p => p.Select(new List<string> { "test" }))
                .Should().Throw<ArgumentException>();
        }
    }
}
