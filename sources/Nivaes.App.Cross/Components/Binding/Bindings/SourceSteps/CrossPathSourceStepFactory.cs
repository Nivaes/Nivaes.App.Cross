namespace Nivaes.App.Cross
{
    public class CrossPathSourceStepFactory : CrossTypedSourceStepFactory<CrossPathSourceStepDescription>
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override ICrossSourceStep TypedCreate(CrossPathSourceStepDescription description)
        {
            return new CrossPathSourceStep(description);
        }
    }
}
