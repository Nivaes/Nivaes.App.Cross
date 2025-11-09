namespace Nivaes.App.Cross.UIKit
{
    using System.Collections.Generic;
    using System.Linq;

    internal abstract class SectionExpandableController
    {
        protected readonly HashSet<int> ExpandedIndexesSet = new HashSet<int>();

        /// <summary>
        /// Toggles expandable state and returns what indexes had changed.
        /// </summary>
        /// <param name="atIndex"></param>
        public abstract ToggleExpandStateResponse ToggleState(int atIndex);

        public bool IsExpanded(int atIndex) => ExpandedIndexesSet.Contains(atIndex);

        public void ResetState() => ExpandedIndexesSet.Clear();

        public IEnumerable<int> ExpandedIndexes => ExpandedIndexesSet.ToList();
    }
}
