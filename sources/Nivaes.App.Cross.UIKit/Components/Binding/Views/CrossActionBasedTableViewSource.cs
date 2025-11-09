namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using ObjCRuntime;

    public class CrossActionBasedTableViewSource : CrossStandardTableViewSource
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public CrossActionBasedTableViewSource(UITableView tableView)
            : base(tableView)
        {
            Initialize();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public CrossActionBasedTableViewSource(NativeHandle handle)
            : base(handle)
        {
            CrossLogHost.GetLog<CrossActionBasedTableViewSource>()?.Log(
                LogLevel.Warning, "MvxActionBasedTableViewSource NativeHandle constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
            Initialize();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public CrossActionBasedTableViewSource(
            UITableView tableView,
            UITableViewCellStyle style,
            NSString cellIdentifier,
            string bindingText,
            UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
        {
            Initialize();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public CrossActionBasedTableViewSource(
            UITableView tableView,
            UITableViewCellStyle style,
            NSString cellIdentifier,
            IEnumerable<CrossBindingDescription> descriptions,
            UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
            Initialize();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        private void Initialize()
        {
            CellCreator = CreateDefaultBindableCell;
            CellModifier = (ignored) => { };
        }

        public Func<UITableView, NSIndexPath, object, CrossStandardTableViewCell> CellCreator { get; set; }
        public Action<CrossStandardTableViewCell> CellModifier { get; set; }
        public Func<NSString> CellIdentifierOverride { get; set; }

        protected override NSString CellIdentifier
        {
            get
            {
                if (CellIdentifierOverride != null)
                    return CellIdentifierOverride();

                return base.CellIdentifier;
            }
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            var cell = CellCreator(tableView, indexPath, item);
            CellModifier?.Invoke(cell);
            return cell;
        }
    }
}
