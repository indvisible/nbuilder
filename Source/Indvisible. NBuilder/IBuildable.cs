namespace Indvisible.NBuilder
{
    using System;

    public interface IBuildable
    {
        dynamic Build(Type type);
    }
}