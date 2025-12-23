namespace Nivaes.App.Cross
{
    public class MvxCombinerSourceStepFactory 
        : MvxTypedSourceStepFactory<MvxCombinerSourceStepDescription>
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override IMvxSourceStep TypedCreate(MvxCombinerSourceStepDescription description)
        {
            var toReturn = new MvxCombinerSourceStep(description);
            return toReturn;
        }
    }
}
