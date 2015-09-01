using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace XmlExplorer
{
    public interface IMainWindowPresentationModel
    {
        DelegateCommand<object> Close { get; }
        DelegateCommand<object> Exit { get; }
        DelegateCommand<object> Open { get; }
    }
}
