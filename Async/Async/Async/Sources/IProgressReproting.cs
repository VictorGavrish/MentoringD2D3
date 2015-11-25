using System;

namespace Async.Sources
{
    public interface IProgressReproting
    {
        double Progress { get; }
        event EventHandler<double> ProgressChanged;
    }
}