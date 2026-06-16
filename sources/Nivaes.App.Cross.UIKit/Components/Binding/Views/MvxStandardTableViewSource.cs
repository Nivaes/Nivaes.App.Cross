using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;
using ObjCRuntime;
using UIKit;

namespace Nivaes.App.Cross.UIKitOS;
public class MvxStandardTableViewSource 
    : MvxTableViewSource
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

    public MvxStandardTableViewSource(UITableView tableView)
        : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, DefaultBindingDescription)
    {
    }

    public MvxStandardTableViewSource(UITableView tableView, NSString cellIdentifier)
        : this(tableView, UITableViewCellStyle.Default, cellIdentifier, DefaultBindingDescription)
    {
    }

    public MvxStandardTableViewSource(UITableView tableView, string bindingText)
        : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, bindingText)
    {
    }

    public MvxStandardTableViewSource(NativeHandle handle)
        : base(handle)
    {
        CrossLogHost.Default?.LogWarning("MvxStandardTableViewSource NativeHandle constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
    }

    public MvxStandardTableViewSource(
        UITableView tableView,
        UITableViewCellStyle style,
        NSString cellIdentifier,
        string bindingText,
        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
        : this(tableView, style, cellIdentifier, ParseBindingText(bindingText), tableViewCellAccessory)
    {
    }

    public MvxStandardTableViewSource(
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
        if (string.IsNullOrEmpty(bindingText))
            return DefaultBindingDescription;

        return IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingDescriptionParser>().Parse(bindingText);
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
    protected virtual MvxStandardTableViewCell CreateDefaultBindableCell(UITableView tableView,
                                                                         NSIndexPath indexPath, object item)
    {
        return new MvxStandardTableViewCell(_bindingDescriptions, _cellStyle, CellIdentifier,
                                            _tableViewCellAccessory);
    }
}
