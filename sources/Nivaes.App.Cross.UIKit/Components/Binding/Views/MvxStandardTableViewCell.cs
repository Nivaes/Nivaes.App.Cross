namespace Nivaes.App.Cross.UIKit
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Foundation;
    using Nivaes.App.Cross;
    using ObjCRuntime;

    public class MvxStandardTableViewCell
        : MvxTableViewCell
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxStandardTableViewCell(NativeHandle handle)
            : this("TitleText" /* default binding is ToString() on the passed in item */, handle)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxStandardTableViewCell(string bindingText, NativeHandle handle)
            : base(bindingText, handle)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxStandardTableViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions, NativeHandle handle)
            : base(bindingDescriptions, handle)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxStandardTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxStandardTableViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions,
                                        UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
        }

        public string TitleText
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public string DetailText
        {
            get { return DetailTextLabel.Text; }
            set { DetailTextLabel.Text = value; }
        }

        public ICommand SelectedCommand { get; set; }

        private bool _isSelected;

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);

            if (_isSelected == selected)
                return;

            _isSelected = selected;
            if (_isSelected)
            {
                SelectedCommand?.Execute(null);
            }
        }
    }
}
