namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [RequiresUnreferencedCode("This class uses GetType() for type inspection which may not be preserved by trimming")]
    public class CrossLiteralSourceStep
        : MvxSourceStep<CrossLiteralSourceStepDescription>
    {
        public CrossLiteralSourceStep(CrossLiteralSourceStepDescription description)
            : base(description)
        {
        }

        public override Type SourceType
        {
            get
            {
                if (Description.Literal == null)
                    return typeof(object);

                return Description.Literal.GetType();
            }
        }

        protected override void SetSourceValue(object sourceValue)
        {
            // ignored - there is no way to set the source value
        }

        protected override object GetSourceValue()
        {
            return Description.Literal;
        }
    }
}
