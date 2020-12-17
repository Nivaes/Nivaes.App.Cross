namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;

    public class ConvertersViewModel
        : MvxViewModel
    {
        public string UppercaseConverterTest => "this text was lowercase";

        public string LowercaseConverterTest => "THIS TEXT WAS UPPERCASE";
    }
}
