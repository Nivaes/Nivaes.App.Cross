// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    public class ListItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public override string ToString()
            => Title;
    }
}
