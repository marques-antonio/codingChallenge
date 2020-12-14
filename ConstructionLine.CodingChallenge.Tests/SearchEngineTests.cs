using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void When_Null_Shirts_Expect_Argument_Null_Exception()
        {
            // Arrange
            Action action = () =>
            {
                var _ = new SearchEngine(null);
            };

            // Action / Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void When_No_Shirts_Expect_No_Results()
        {
            // Arrange
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions();

            // Action
            var results = searchEngine.Search(searchOptions);

            // Assert
            var expectedResults = GenerateSearchResults();
            results.Should().BeEquivalentTo(expectedResults);
        }

        [Test]
        public void When_Null_Search_Terms_Expect_Argument_Null_Exception()
        {
            // Arrange
            var shirts = _shirtList;
            var searchEngine = new SearchEngine(shirts);
            Action action = () => searchEngine.Search(null);

            // Action/Assert
            action.Should().Throw<ArgumentNullException>();
        }

        #region Privates
        private static SearchResults GenerateSearchResults(List<Shirt> shirts = null, List<ColorCount> colorCounts = null, List<SizeCount> sizeCounts = null)
        {
            var result = new SearchResults
            {
                Shirts = shirts ?? new List<Shirt>(),

                ColorCounts = Color.All.Select(color =>
                        colorCounts?.SingleOrDefault(colorCount => colorCount.Color == color)
                        ?? new ColorCount { Color = color, Count = 0 })
                    .ToList(),

                SizeCounts = Size.All.Select(size =>
                        sizeCounts?.SingleOrDefault(sizeCount => sizeCount.Size == size)
                        ?? new SizeCount() { Size = size, Count = 0 })
                    .ToList()
            };

            return result;
        }

        private static readonly List<Shirt> _shirtList = new List<Shirt>
        {
            new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),
            new Shirt(Guid.NewGuid(), "Yellow - Medium", Size.Medium, Color.Yellow),
            new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
            new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
            new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
            new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
            new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),
            new Shirt(Guid.NewGuid(), "White - Small", Size.Small, Color.White),
            new Shirt(Guid.NewGuid(), "White - Medium", Size.Medium, Color.White),
            new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
        };
        #endregion
    }
}
