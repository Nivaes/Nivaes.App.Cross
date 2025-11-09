namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using ObjCRuntime;

    public class CrossStandardTableViewSource : CrossTableViewSource
    {
        private static readonly NSString DefaultCellIdentifier = new NSString("SimpleBindableTableViewCell");

        private static readonly CrossBindingDescription[] DefaultBindingDescription = new[]
            {
                new CrossBindingDescription
                    {
                        TargetName = "TitleText",
                        Source = new CrossPathSourceStepDescription()
                            {
                                SourcePropertyPath = string.Empty
                            }
                    },
            };

        private readonly IEnumerable<CrossBindingDescription> _bindingDescriptions;
        private readonly NSString _cellIdentifier;
        private readonly UITableViewCellStyle _cellStyle;
        private readonly UITableViewCellAccessory _tableViewCellAccessory = UITableViewCellAccessory.None;

        protected virtual NSString CellIdentifier => _cellIdentifier;

        public CrossStandardTableViewSource(UITableView tableView)
            : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, DefaultBindingDescription)
        {
        }

        public CrossStandardTableViewSource(UITableView tableView, NSString cellIdentifier)
            : this(tableView, UITableViewCellStyle.Default, cellIdentifier, DefaultBindingDescription)
        {
        }

        public CrossStandardTableViewSource(UITableView tableView, string bindingText)
            : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, bindingText)
        {
        }

        public CrossStandardTableViewSource(NativeHandle handle)
            : base(handle)
        {
            CrossLogHost.Default?.LogWarning("MvxStandardTableViewSource NativeHandle constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public CrossStandardTableViewSource(
            UITableView tableView,
            UITableViewCellStyle style,
            NSString cellIdentifier,
            string bindingText,
            UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : this(tableView, style, cellIdentifier, ParseBindingText(bindingText), tableViewCellAccessory)
        {
        }

        public CrossStandardTableViewSource(
            UITableView tableView,
            UITableViewCellStyle style,
            NSString cellIdentifier,
            IEnumerable<CrossBindingDescription> descriptions,
            UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(tableView)
        {
            _cellStyle = style;
            _cellIdentifier = cellIdentifier;
            _bindingDescriptions = descriptions;
            _tableViewCellAccessory = tableViewCellAccessory;
        }

        protected IEnumerable<CrossBindingDescription> BindingDescriptions => _bindingDescriptions;

        private static IEnumerable<CrossBindingDescription> ParseBindingText(string bindingText)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrEmpty(bindingText))
            //    return DefaultBindingDescription;

            //return Mvx.IoCProvider.Resolve<ICrossBindingDescriptionParser>().Parse(bindingText);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            return CreateDefaultBindableCell(tableView, indexPath, item);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        protected virtual CrossStandardTableViewCell CreateDefaultBindableCell(UITableView tableView,
                                                                             NSIndexPath indexPath, object item)
        {
            return new CrossStandardTableViewCell(_bindingDescriptions, _cellStyle, CellIdentifier,
                                                _tableViewCellAccessory);
        }
    }
}
