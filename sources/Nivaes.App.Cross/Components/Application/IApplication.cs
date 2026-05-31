using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross;

public interface IApplication
{
    void Initialize();

    void Startup();

    void Reset();
}
