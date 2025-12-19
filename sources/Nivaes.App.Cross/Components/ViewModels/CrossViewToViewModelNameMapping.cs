namespace Nivaes.App.Cross
{

    [Obsolete]
    public class CrossViewToViewModelNameMapping
        : ICrossNameMapping
    {
        public string ViewModelPostfix { get; set; }

        public CrossViewToViewModelNameMapping()
        {
            ViewModelPostfix = "ViewModel";
        }

        public virtual string Map(string inputName)
        {
            return inputName + ViewModelPostfix;
        }
    }
}
