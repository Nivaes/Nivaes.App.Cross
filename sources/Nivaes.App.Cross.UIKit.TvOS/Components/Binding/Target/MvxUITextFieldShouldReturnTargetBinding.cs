namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUITextFieldShouldReturnTargetBinding
        : CrossTargetBinding
    {
        private ICommand _command;

        protected UITextField View => Target as UITextField;

        public MvxUITextFieldShouldReturnTargetBinding(UITextField target)
            : base(target)
        {
            target.ShouldReturn = HandleShouldReturn;
        }

        private bool HandleShouldReturn(UITextField textField)
        {
            if (_command == null)
                return false;

            var text = textField.Text;
            if (!_command.CanExecute(text))
                return false;

            textField.ResignFirstResponder();
            _command.Execute(text);
            return true;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _command = command;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            var editText = View;
            if (editText == null) return;

            editText.ShouldReturn = null;
        }
    }
}
