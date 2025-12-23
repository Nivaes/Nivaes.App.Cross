namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossLiteralSourceStepFactory 
        : CrossTypedSourceStepFactory<CrossLiteralSourceStepDescription>
    {
        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override ICrossSourceStep TypedCreate(CrossLiteralSourceStepDescription description)
        {
            var toReturn = new CrossLiteralSourceStep(description);
            return toReturn;
        }
    }
}
