using System;
using System.Collections.Generic;
using System.Text;
using Generated;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Nivaes.App.Cross.Hosting;

public class CrossHostEnvironment : IHostEnvironment
{
    public string EnvironmentName
    {
        get => "Production";
        set => throw new System.NotImplementedException();
    }

    public string ApplicationName
    {
        //get => AppInfo.Current.Name;
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public string ContentRootPath
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public IFileProvider ContentRootFileProvider
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }
}
