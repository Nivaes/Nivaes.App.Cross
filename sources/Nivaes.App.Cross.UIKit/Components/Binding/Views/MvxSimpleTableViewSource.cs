namespace Nivaes.App.Cross.UIKitOS
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Foundation;
    using Microsoft.Extensions.Logging;
    using ObjCRuntime;
    using UIKit;

    public class MvxSimpleTableViewSource 
        : MvxTableViewSource
    {
        private readonly NSString? _cellIdentifier;
        private readonly MvxIosMajorVersionChecker _iosVersion6Checker = new MvxIosMajorVersionChecker(6);

        protected virtual NSString? CellIdentifier => _cellIdentifier;

        public MvxSimpleTableViewSource(NativeHandle handle)
            : base(handle)
        {
            CrossLogHost.Default?.LogWarning("MvxSimpleTableViewSource NativeHandle constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public MvxSimpleTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null,
                                        NSBundle bundle = null, bool registerNibForCellReuse = true)
            : base(tableView)
        {
            // if no cellIdentifier supplied, then use the nibName as cellId
            cellIdentifier = cellIdentifier ?? nibName;
            _cellIdentifier = new NSString(cellIdentifier);

            if (registerNibForCellReuse)
            {
                tableView.RegisterNibForCellReuse(UINib.FromName(nibName, bundle ?? NSBundle.MainBundle), cellIdentifier);
            }
        }

        public MvxSimpleTableViewSource(UITableView tableView, Type cellType, string cellIdentifier = null)
            : base(tableView)
        {
            // if no cellIdentifier supplied, then use the cell type name as cellId
            cellIdentifier = cellIdentifier ?? cellType.Name;
            _cellIdentifier = new NSString(cellIdentifier);
            tableView.RegisterClassForCellReuse(cellType, _cellIdentifier);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (_iosVersion6Checker.IsVersionOrHigher)
                return tableView.DequeueReusableCell(CellIdentifier, indexPath);

            return tableView.DequeueReusableCell(CellIdentifier);
        }
    }
}
