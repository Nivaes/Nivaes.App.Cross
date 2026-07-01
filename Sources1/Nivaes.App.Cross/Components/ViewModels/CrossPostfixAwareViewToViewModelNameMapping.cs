//namespace Nivaes.App.Cross
//{
//    [Obsolete("No asociar la vista y el modelo por el nombre de la clase")]
//    public class CrossPostfixAwareViewToViewModelNameMapping
//        : CrossViewToViewModelNameMapping
//    {
//        private readonly string[] _postfixes;

//        public CrossPostfixAwareViewToViewModelNameMapping(params string[] postfixes)
//        {
//            _postfixes = postfixes;
//        }

//        public override string Map(string inputName)
//        {
//            foreach (var postfix in _postfixes)
//            {
//                if (inputName.EndsWith(postfix) && inputName.Length > postfix.Length)
//                {
//                    inputName = inputName.Substring(0, inputName.Length - postfix.Length);
//                    break;
//                }
//            }
//            return base.Map(inputName);
//        }
//    }
//}
