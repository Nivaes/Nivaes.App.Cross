namespace Nivaes.App.Cross
{
    public class MvxPathSourceStepFactory 
        : MvxTypedSourceStepFactory<MvxPathSourceStepDescription>
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override IMvxSourceStep TypedCreate(MvxPathSourceStepDescription description)
        {
            return new MvxPathSourceStep(description);
        }
    }
}
