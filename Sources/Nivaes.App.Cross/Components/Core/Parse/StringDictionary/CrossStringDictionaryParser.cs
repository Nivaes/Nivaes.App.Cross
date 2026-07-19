namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public class CrossStringDictionaryParser
        : CrossParser, ICrossStringDictionaryParser
    {
        protected Dictionary<string, string?>? CurrentEntries { get; private set; }

        public IDictionary<string, string> Parse(string textToParse)
        {
            Reset(textToParse);

            while (!IsComplete)
            {
                ParseNextKeyValuePair();
                SkipWhitespaceAndCharacters(';');
            }

            return CurrentEntries!;
        }

        protected override void Reset(string? textToParse)
        {
            CurrentEntries = new Dictionary<string, string?>();
            base.Reset(textToParse);
        }

        private void ParseNextKeyValuePair()
        {
            SkipWhitespace();

            if (IsComplete)
            {
                return;
            }

            var key = ReadValue();
            if (key is not string keyString)
            {
                throw new AppException($"Unexpected object in key for key/value pair {key?.GetType().Name} at position {CurrentIndex}");
            }

            SkipWhitespace();

            if (CurrentChar != '=')
            {
                throw new AppException($"Unexpected character in key/value pair {CurrentChar} at position {CurrentIndex}");
            }

            MoveNext();
            SkipWhitespace();

            var value = ReadValue();
            if (value == null)
            {
                CurrentEntries![keyString] = null;
            }
            else if (value is string stringValue)
            {
                CurrentEntries![keyString] = stringValue;
            }
            else
            {
                throw new AppException($"Unexpected object in value for key/value pair {value.GetType().Name} for key {key} at position {CurrentIndex}");
            }
        }
    }
}