namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;

    public class CrossCommandCollection
        : ICrossCommandCollection
    {
        private readonly object _owner;
        private readonly Dictionary<string, ICrossCommand> _commandLookup = new();
        private readonly Dictionary<string, List<ICrossCommand>> _canExecuteLookup = new();

        public CrossCommandCollection(object owner)
        {
            _owner = owner;
            SubscribeToNotifyPropertyChanged();
        }

        private void SubscribeToNotifyPropertyChanged()
        {
            var inpc = _owner as INotifyPropertyChanged;
            if (inpc == null)
                return;

            inpc.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            // if args.PropertyName is empty then it means all properties have changed.
            if (string.IsNullOrEmpty(args.PropertyName))
            {
                RaiseAllCanExecuteChanged();
                return;
            }

            if (_canExecuteLookup.TryGetValue(args.PropertyName, out List<ICrossCommand>? commands))
            {
                foreach (var command in commands)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        private void RaiseAllCanExecuteChanged()
        {
            foreach (var command in _commandLookup)
            {
                command.Value.RaiseCanExecuteChanged();
            }
        }

        public ICrossCommand? this[string name]
        {
            get
            {
                if (_commandLookup.Count == 0)
                {
                    CrossLogHost.Default?.Log(LogLevel.Trace, "MvxCommandCollection is empty - did you forget to add your commands?");
                    return null;
                }

                _commandLookup.TryGetValue(name, out var toReturn);
                return toReturn;
            }
        }

        public void Add(ICrossCommand command, string name, string? canExecuteName)
        {
            AddToLookup(_commandLookup, command, name);
            AddToLookup(_canExecuteLookup, command, canExecuteName);
        }

        private static void AddToLookup(IDictionary<string, ICrossCommand> lookup, ICrossCommand command, string? name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (lookup.ContainsKey(name))
            {
                CrossLogHost.Default?.Log(LogLevel.Warning,
                    "Ignoring Commmand - it would overwrite the existing Command, name {Name}", name);
                return;
            }

            lookup[name] = command;
        }

        private static void AddToLookup(IDictionary<string, List<ICrossCommand>> lookup, ICrossCommand command, string? name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            // If no collection exists, create a new one
            if (!lookup.TryGetValue(name, out var commands))
            {
                commands = new List<ICrossCommand>();
                lookup[name] = commands;
            }

            // Protect against adding command twice
            if (!commands.Contains(command))
            {
                commands.Add(command);
            }
        }
    }
}