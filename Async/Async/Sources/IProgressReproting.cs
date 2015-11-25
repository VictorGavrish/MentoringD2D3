using System;

namespace Sources
{
    public interface IProgressReproting
    {
        double Progress { get; }
        event EventHandler<double> ProgressChanged;
    }
}