namespace Nivaes.App.Cross.UIKit
{
    using System.Collections.Generic;
    using System.Linq;

    internal class ToggleExpandStateResponse
    {
        public ToggleExpandStateResponse(IEnumerable<int> expandedIndexes, IEnumerable<int> collapsedIndexes)
        {
            ExpandedIndexes = expandedIndexes;
            CollapsedIndexes = collapsedIndexes;
        }

        public IEnumerable<int> ExpandedIndexes { get; }

        public IEnumerable<int> CollapsedIndexes { get; }

        public IEnumerable<int> AllModifiedIndexes => ExpandedIndexes.Concat(CollapsedIndexes).ToList();
    }
}
