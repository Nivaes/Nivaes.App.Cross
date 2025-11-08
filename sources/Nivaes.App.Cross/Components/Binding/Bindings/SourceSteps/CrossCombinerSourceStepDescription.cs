namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class CrossCombinerSourceStepDescription : CrossSourceStepDescription
    {
        public ICrossValueCombiner? Combiner { get; set; }
        public List<CrossSourceStepDescription>? InnerSteps { get; set; }

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This is a diagnostic ToString method and the type name is not critical for functionality")]
        public override string ToString()
        {
            return Combiner == null ? "-null-" : Combiner.GetType().Name + " combiner-operation";
        }
    }
}
