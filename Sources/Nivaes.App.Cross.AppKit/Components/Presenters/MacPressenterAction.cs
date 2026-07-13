using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitOS
{
    public abstract class MacPressenterAction<TPressenterAttribute>
                : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : ICrossPresentationAttribute
    {
        #region Constructor
        protected MacPressenterAction(
                ICrossViewsContainer viewsContainer,
                ILogger logger)
            : base(viewsContainer, logger)
        {
        }
        #endregion
    }
}
