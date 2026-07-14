//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using Nivaes.App.Cross;

//public sealed class TestPresentationAttribute : BasePresentationAttribute
//{
//}

//public sealed class TestPressenterAction : PressenterAction<TestPresentationAttribute>
//{
//    public TestPressenterAction(ICrossViewsContainer viewsContainer, ILogger<TestPressenterAction> logger)
//        :base(viewsContainer, logger)
//    { }

//    protected override ValueTask<bool> ShowAction(Type viewType, TestPresentationAttribute attribute, CrossViewModelRequest request)
//    {
//        throw new NotImplementedException();
//    }
//    protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, TestPresentationAttribute attribute)
//    {
//        throw new NotImplementedException();
//    }

//    protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
//    {
//        throw new NotImplementedException();
//    }
//}