namespace Nivaes.App.Cross
{
    public class CrossCombinerSourceStepFactory 
        : CrossTypedSourceStepFactory<CrossCombinerSourceStepDescription>
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override ICrossSourceStep TypedCreate(CrossCombinerSourceStepDescription description)
        {
            var toReturn = new CrossCombinerSourceStep(description);
            return toReturn;
        }
    }
}
