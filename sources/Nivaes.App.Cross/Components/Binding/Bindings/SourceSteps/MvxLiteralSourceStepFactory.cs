namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public class MvxLiteralSourceStepFactory 
        : MvxTypedSourceStepFactory<MvxLiteralSourceStepDescription>
    {
        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected override IMvxSourceStep TypedCreate(MvxLiteralSourceStepDescription description)
        {
            var toReturn = new MvxLiteralSourceStep(description);
            return toReturn;
        }
    }
}
