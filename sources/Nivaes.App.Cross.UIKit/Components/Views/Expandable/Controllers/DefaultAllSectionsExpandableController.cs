namespace Nivaes.App.Cross.UIKit
{
    using System.Collections.Generic;

    internal class DefaultAllSectionsExpandableController : SectionExpandableController
    {
        public override ToggleExpandStateResponse ToggleState(int atIndex)
        {
            List<int> collapsedIndexes = new List<int>();
            List<int> expandedIndexes = new List<int>();
            if (ExpandedIndexesSet.Contains(atIndex))
            {
                ExpandedIndexesSet.Remove(atIndex);
                collapsedIndexes.Add(atIndex);
            }
            else
            {
                ExpandedIndexesSet.Add(atIndex);
                expandedIndexes.Add(atIndex);
            }

            return new ToggleExpandStateResponse(expandedIndexes, collapsedIndexes);
        }
    }
}
