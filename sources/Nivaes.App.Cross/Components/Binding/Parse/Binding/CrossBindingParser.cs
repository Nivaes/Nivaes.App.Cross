namespace MvvmCross.Binding.Parse.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;

    public abstract class CrossBindingParser
        : CrossParser, ICrossBindingParser
    {
        protected abstract CrossSerializableBindingDescription ParseBindingDescription();

        public bool TryParseBindingDescription(string text, out CrossSerializableBindingDescription requestedDescription)
        {
            try
            {
                Reset(text);
                requestedDescription = ParseBindingDescription();
                return true;
            }
            catch (Exception exception)
            {
                CrossBindingLog.Instance?.LogError(exception, "Problem parsing binding");
                requestedDescription = null;
                return false;
            }
        }

        public bool TryParseBindingSpecification(string text, out CrossSerializableBindingSpecification requestedBindings)
        {
            try
            {
                Reset(text);

                var toReturn = new CrossSerializableBindingSpecification();
                while (!IsComplete)
                {
                    SkipWhitespaceAndDescriptionSeparators();
                    var result = ParseTargetPropertyNameAndDescription();
                    toReturn[result.Key] = result.Value;
                    SkipWhitespaceAndDescriptionSeparators();
                }

                requestedBindings = toReturn;
                return true;
            }
            catch (Exception exception)
            {
                CrossBindingLog.Instance?.LogError(exception, "Problem parsing binding");
                requestedBindings = null;
                return false;
            }
        }

        protected KeyValuePair<string, CrossSerializableBindingDescription> ParseTargetPropertyNameAndDescription()
        {
            var targetPropertyName = ReadTargetPropertyName();
            SkipWhitespace();
            var description = ParseBindingDescription();
            return new KeyValuePair<string, CrossSerializableBindingDescription>(targetPropertyName, description);
        }

        protected void ParseEquals(string block)
        {
            if (IsComplete)
                throw new CrossException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       FullText);
            if (CurrentChar != '=')
                throw new CrossException("Must follow binding option {0} with an '=' in {1}",
                                       block,
                                       FullText);

            MoveNext();
            if (IsComplete)
                throw new CrossException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       FullText);
        }

        protected CrossBindingMode ReadBindingMode()
        {
            return (CrossBindingMode)ReadEnumerationValue(typeof(CrossBindingMode));
        }

        protected string ReadTextUntilNonQuotedOccurrenceOfAnyOf(params char[] terminationCharacters)
        {
            var terminationLookup = terminationCharacters.ToDictionary(c => c, c => true);
            SkipWhitespace();
            var toReturn = new StringBuilder();

            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (currentChar == '\'' || currentChar == '\"')
                {
                    var subText = ReadQuotedString();
                    toReturn.Append(currentChar);
                    toReturn.Append(subText);
                    toReturn.Append(currentChar);
                    continue;
                }

                if (terminationLookup.ContainsKey(currentChar))
                {
                    break;
                }

                toReturn.Append(currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected string ReadTargetPropertyName()
        {
            return ReadValidCSharpName();
        }

        protected void SkipWhitespaceAndOptionSeparators()
        {
            SkipWhitespaceAndCharacters(',');
        }

        protected void SkipWhitespaceAndDescriptionSeparators()
        {
            SkipWhitespaceAndCharacters(';');
        }
    }
}
