namespace MvvmCross.Binding.Parse.Binding.Tibet
{
    using MvvmCross.Binding.Parse.Binding.Swiss;
    using Nivaes.App.Cross;

    public class CrossTibetBindingParser
        : CrossSwissBindingParser
    {
        public static readonly object LiteralNull = new object();

        public static readonly char[] OperatorCharacters = { '>', '<', '+', '-', '*', '/', '|', '&', '!', '=', '%', '^' };

        private static Dictionary<string, string> TwoCharacterOperatorCombinerNames => new Dictionary<string, string>
        {
            { "!=", "NotEqualTo" },
            { ">=", "GreaterThanOrEqualTo" },
            { "<=", "LessThanOrEqualTo" },
            { "==", "EqualTo" },
            { "&&", "And" },
            { "||", "Or" }
        };

        private static Dictionary<char, string> SingleCharacterOperatorCombinerNames => new Dictionary<char, string>
        {
            { '>', "GreaterThan" },
            { '<', "LessThan" },
            { '+', "Add" },
            { '-', "Subtract" },
            { '*', "Multiply" },
            { '/', "Divide" },
            { '%', "Modulus" },
            { '!', "Inverted" },
            { '^', "XOr" }
        };

        private char[] _terminatingCharacters;

        protected override IEnumerable<char> TerminatingCharacters() =>
            _terminatingCharacters ?? (_terminatingCharacters = base.TerminatingCharacters().Union(OperatorCharacters).ToArray());

        protected override void ParseNextBindingDescriptionOptionInto(CrossSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            object literal;
            if (TryReadValue(AllowNonQuotedText.DoNotAllow, out literal))
            {
                // for null, replace with LiteralNull
                literal = literal ?? LiteralNull;
                ThrowExceptionIfPathAlreadyDefined(description);
                description.Literal = literal;
                return;
            }

            base.ParseNextBindingDescriptionOptionInto(description);
        }

        protected override void ParseFunctionStyleBlockInto(CrossSerializableBindingDescription description, string block)
        {
            description.Function = block;
            MoveNext();
            if (IsComplete)
                throw new CrossException("Unterminated () pair for combiner {0}", block);

            var terminationFound = false;
            var sources = new List<CrossSerializableBindingDescription>();
            while (!terminationFound)
            {
                SkipWhitespace();
                sources.Add(ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma));
                SkipWhitespace();
                if (IsComplete)
                    throw new CrossException("Unterminated () while parsing combiner {0}", block);

                switch (CurrentChar)
                {
                    case ')':
                        MoveNext();
                        terminationFound = true;
                        break;

                    case ',':
                        MoveNext();
                        break;

                    default:
                        throw new CrossException("Unexpected character {0} while parsing () combiner contents for {1}", CurrentChar, block);
                }
            }

            description.Sources = sources.ToArray();
        }

        private Tuple<uint, string> ParseTwoCharacterOperator()
        {
            uint moveFowards = 0;
            var twoCharacterOperatorString = SafePeekString(2);
            var gotCombinerName = TwoCharacterOperatorCombinerNames.TryGetValue(twoCharacterOperatorString, out string combinerName);
            if (gotCombinerName)
                moveFowards = 2;

            return Tuple.Create(moveFowards, combinerName);
        }

        protected override CrossSerializableBindingDescription ParseOperatorWithLeftHand(CrossSerializableBindingDescription description)
        {
            // Parse the operator
            var parsed = ParseTwoCharacterOperator();
            var moveForwards = parsed.Item1;
            var combinerName = parsed.Item2;

            if (combinerName == null)
            {
                var gotCombinerName = SingleCharacterOperatorCombinerNames.TryGetValue(CurrentChar, out combinerName);
                if (gotCombinerName)
                    moveForwards = 1;
            }

            if (combinerName == null)
                throw new CrossException("Unexpected operator starting with {0}", CurrentChar);

            MoveNext(moveForwards);

            // now create the operator Combiner
            var child = new CrossSerializableBindingDescription
            {
                Path = description.Path,
                Literal = description.Literal,
                Sources = description.Sources,
                Function = description.Function,
                Converter = description.Converter,
                FallbackValue = description.FallbackValue,
                ConverterParameter = description.ConverterParameter,
                Mode = description.Mode
            };

            description.Converter = null;
            description.ConverterParameter = null;
            description.FallbackValue = null;
            description.Path = null;
            description.Mode = CrossBindingMode.Default;
            description.Literal = null;
            description.Function = combinerName;
            description.Sources = new List<CrossSerializableBindingDescription>
            {
                child,
                ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma)
            };

            return description;
        }

        protected override bool DetectOperator() => OperatorCharacters.Contains(CurrentChar);

        protected override void HandleEmptyBlock(CrossSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            if (CurrentChar == '(')
            {
                MoveNext();
                ParseChildBindingDescriptionInto(description, ParentIsLookingForComma.ParentIsNotLookingForComma);

                SkipWhitespace();
                if (IsComplete || CurrentChar != ')')
                    throw new CrossException("Unterminated () pair");
                MoveNext();
                SkipWhitespace();
            }
        }
    }
}
