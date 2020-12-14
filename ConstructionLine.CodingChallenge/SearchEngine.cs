using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts ?? throw new ArgumentNullException(nameof(shirts));
        }

        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var searchResults = _shirts.Where(shirt =>
                    (!options.Colors.Any() || options.Colors.Contains(shirt.Color))
                     && (!options.Sizes.Any() || options.Sizes.Contains(shirt.Size)))
               .ToList();

            var result = new SearchResults
            {
                Shirts = searchResults,
                ColorCounts = Color.All.Select(color => new ColorCount
                {
                    Color = color,
                    Count = searchResults.Count(shirt => shirt.Color == color)
                }).ToList(),
                SizeCounts = Size.All.Select(size => new SizeCount
                {
                    Size = size,
                    Count = searchResults.Count(shirt => shirt.Size == size)
                }).ToList()
            };

            return result;
        }
    }
}