namespace Nivaes.App.Cross
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CrossCommandAttribute : Attribute
    {
        public CrossCommandAttribute(string commandName, string? canExecutePropertyName = null)
        {
            CanExecutePropertyName = canExecutePropertyName;
            CommandName = commandName;
        }

        public string CommandName { get; set; }
        public string? CanExecutePropertyName { get; set; }
    }
}
