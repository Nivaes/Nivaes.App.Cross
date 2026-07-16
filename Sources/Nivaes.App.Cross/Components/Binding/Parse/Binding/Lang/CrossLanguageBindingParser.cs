namespace Nivaes.App.Cross
{
    public class CrossLanguageBindingParser
        : CrossBindingParser, ICrossLanguageBindingParser
    {
        public CrossBindingMode DefaultBindingMode { get; set; }

        public string DefaultConverterName { get; set; }

        public string DefaultTextSourceName { get; set; }

        public CrossLanguageBindingParser()
        {
            DefaultConverterName = "Language";
            DefaultTextSourceName = "TextSource";
            DefaultBindingMode = CrossBindingMode.OneTime;
        }

        protected void ParseNextBindingDescriptionOptionInto(CrossSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            var block = ReadTextUntilNonQuotedOccurrenceOfAnyOf('=', ',', ';');
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                return;
            }

            switch (block)
            {
                case "Source":
                    ParseEquals(block);
                    var sourceName = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    description.Path = sourceName;
                    break;

                case "Converter":
                    ParseEquals(block);
                    description.Converter = ReadValidCSharpName();
                    break;

                case "Key":
                    ParseEquals(block);
                    description.ConverterParameter = ReadValue();
                    break;

                case "FallbackValue":
                    ParseEquals(block);
                    description.FallbackValue = ReadValue();
                    break;

                default:
                    if (description.ConverterParameter != null)
                    {
                        throw new AppException(
                            "Problem parsing Language Binding near '{0}', Key set to '{1}', position {2} in {3}",
                            block, description.ConverterParameter, CurrentIndex, FullText);
                    }

                    block = UnquoteBlockIfNecessary(block);

                    description.ConverterParameter = block;
                    break;
            }
        }

        private static string UnquoteBlockIfNecessary(string block)
        {
            if (string.IsNullOrEmpty(block))
                return block;

            if (block.Length < 2)
                return block;

            if ((block.StartsWith('\'') && block.EndsWith('\''))
                || (block.StartsWith('"') && block.EndsWith('"')))
                return block.Substring(1, block.Length - 2);

            return block;
        }

        protected override CrossSerializableBindingDescription ParseBindingDescription()
        {
            var description = new CrossSerializableBindingDescription
            {
                Converter = DefaultConverterName,
                Path = DefaultTextSourceName,
                Mode = DefaultBindingMode
            };

            SkipWhitespace();

            while (true)
            {
                ParseNextBindingDescriptionOptionInto(description);

                SkipWhitespace();
                if (IsComplete)
                    return description;

                switch (CurrentChar)
                {
                    case ',':
                        MoveNext();
                        break;

                    case ';':
                        return description;

                    default:
                        throw new AppException(
                            "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                            CurrentChar,
                            CurrentIndex,
                            FullText);
                }
            }
        }
    }
}
