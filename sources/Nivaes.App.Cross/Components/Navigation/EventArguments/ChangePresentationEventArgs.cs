using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross
{
    public class ChangePresentationEventArgs 
        : CrossCancelEventArgs
    {
        public ChangePresentationEventArgs(CancellationToken cancellationToken = default) : base(cancellationToken)
        {
        }

        public ChangePresentationEventArgs(PresentationHint hint, CancellationToken cancellationToken = default) : this(cancellationToken)
        {
            Hint = hint;
        }

        public PresentationHint Hint { get; set; }

        public bool? Result { get; set; }
    }
}
