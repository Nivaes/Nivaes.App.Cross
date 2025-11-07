namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    public class CrossSwissBindingParser
        : CrossBindingParser
    {
        protected virtual IEnumerable<char> TerminatingCharacters()
        {
            return new[] { '=', ',', ';', '(', ')' };
        }

        private void ParsePath(string block, CrossSerializableBindingDescription description)
        {
            ParseEquals(block);
            ThrowExceptionIfPathAlreadyDefined(description);
            description.Path = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
        }

        private void ParseConverter(string block, CrossSerializableBindingDescription description)
        {
            ParseEquals(block);
            var converter = ReadTargetPropertyName();
            if (!string.IsNullOrEmpty(description.Converter))
                CrossBindingLog.Instance?.LogWarning("Overwriting existing Converter with {ConverterName}", converter);
            description.Converter = converter;
        }

        private void ParseConverterParameter(string block, CrossSerializableBindingDescription description)
        {
            ParseEquals(block);
            if (description.ConverterParameter != null)
                CrossBindingLog.Instance?.LogWarning("Overwriting existing ConverterParameter");
            description.ConverterParameter = ReadValue();
        }

        private void ParseCommandParameter(string block, CrossSerializableBindingDescription description)
        {
            if (!IsComplete &&
               CurrentChar == '(')
            {
                // following https://github.com/MvvmCross/MvvmCross/issues/704, if the next character is "(" then
                // we can treat CommandParameter as a normal non-keyword block
                ParseNonKeywordBlockInto(description, block);
            }
            else
            {
                ParseEquals(block);
                if (!string.IsNullOrEmpty(description.Converter))
                    CrossBindingLog.Instance?.LogWarning("Overwriting existing Converter with CommandParameter");
                description.Converter = "CommandParameter";
                description.ConverterParameter = ReadValue();
            }
        }

        private void ParseFallbackValue(string block, CrossSerializableBindingDescription description)
        {
            ParseEquals(block);
            if (description.FallbackValue != null)
                CrossBindingLog.Instance?.LogWarning("Overwriting existing FallbackValue");
            description.FallbackValue = ReadValue();
        }

        private void ParseMode(string block, CrossSerializableBindingDescription description)
        {
            ParseEquals(block);
            description.Mode = ReadBindingMode();
        }

        protected virtual void ParseNextBindingDescriptionOptionInto(CrossSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            var block = ReadTextUntilNonQuotedOccurrenceOfAnyOf(TerminatingCharacters().ToArray());
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                HandleEmptyBlock(description);
                return;
            }

            switch (block)
            {
                case "Path":
                    ParsePath(block, description);
                    break;
                case "Converter":
                    ParseConverter(block, description);
                    break;
                case "ConverterParameter":
                    ParseConverterParameter(block, description);
                    break;
                case "CommandParameter":
                    ParseCommandParameter(block, description);
                    break;
                case "FallbackValue":
                    ParseFallbackValue(block, description);
                    break;
                case "Mode":
                    ParseMode(block, description);
                    break;
                default:
                    ParseNonKeywordBlockInto(description, block);
                    break;
            }
        }

        protected virtual void HandleEmptyBlock(CrossSerializableBindingDescription description)
        {
            // default implementation doesn't do any special handling on an empty block
        }

        protected virtual void ParseNonKeywordBlockInto(CrossSerializableBindingDescription description, string block)
        {
            if (!IsComplete && CurrentChar == '(')
            {
                ParseFunctionStyleBlockInto(description, block);
            }
            else
            {
                ThrowExceptionIfPathAlreadyDefined(description);
                description.Path = block;
            }
        }

        protected virtual void ParseFunctionStyleBlockInto(CrossSerializableBindingDescription description, string block)
        {
            description.Converter = block;
            MoveNext();
            if (IsComplete)
                throw new CrossException("Unterminated () pair for converter {0}", block);

            ParseChildBindingDescriptionInto(description);
            SkipWhitespace();
            switch (CurrentChar)
            {
                case ')':
                    MoveNext();
                    break;

                case ',':
                    MoveNext();
                    ReadConverterParameterAndClosingBracket(description);
                    break;

                default:
                    throw new CrossException("Unexpected character {0} while parsing () contents", CurrentChar);
            }
        }

        protected void ReadConverterParameterAndClosingBracket(CrossSerializableBindingDescription description)
        {
            SkipWhitespace();
            description.ConverterParameter = ReadValue();
            SkipWhitespace();
            if (CurrentChar != ')')
                throw new CrossException("Unterminated () pair for converter {0}");
            MoveNext();
        }

        protected void ParseChildBindingDescriptionInto(CrossSerializableBindingDescription description,
            ParentIsLookingForComma parentIsLookingForComma = ParentIsLookingForComma.ParentIsLookingForComma)
        {
            SkipWhitespace();
            description.Function = "Single";
            description.Sources = new[]
                {
                    ParseBindingDescription(parentIsLookingForComma)
                };
        }

        protected void ThrowExceptionIfPathAlreadyDefined(CrossSerializableBindingDescription description)
        {
            if (description.Path != null &&
                description.Literal != null &&
                description.Function != null)
            {
                throw new CrossException(
                    "Make sure you are using ';' to separate multiple bindings. You cannot specify Path/Literal/Combiner more than once - position {0} in {1}",
                    CurrentIndex, FullText);
            }
        }

        protected enum ParentIsLookingForComma
        {
            ParentIsLookingForComma,
            ParentIsNotLookingForComma
        }

        protected override CrossSerializableBindingDescription ParseBindingDescription() =>
            ParseBindingDescription(ParentIsLookingForComma.ParentIsNotLookingForComma);

        protected virtual CrossSerializableBindingDescription ParseBindingDescription(
            ParentIsLookingForComma parentIsLookingForComma)
        {
            var description = new CrossSerializableBindingDescription();
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
                        if (parentIsLookingForComma == ParentIsLookingForComma.ParentIsLookingForComma)
                            return description;

                        MoveNext();
                        break;

                    case ';':
                    case ')':
                        return description;

                    default:
                        if (DetectOperator())
                            ParseOperatorWithLeftHand(description);
                        else
                            throw new CrossException(
                                "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                                CurrentChar,
                                CurrentIndex,
                                FullText);
                        break;
                }
            }
        }

        protected virtual CrossSerializableBindingDescription ParseOperatorWithLeftHand(
            CrossSerializableBindingDescription description)
        {
            throw new CrossException("Operators not expected in base SwissBinding");
        }

        protected virtual bool DetectOperator() => false;
    }
}
