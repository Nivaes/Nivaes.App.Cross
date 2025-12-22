// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.


namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming")]
    public class CrossSimpleLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
        : CrossLeafPropertyInfoSourceBinding(source, propertyInfo)
    {
        protected override object[] PropertyIndexParameters() => [];
    }
}
