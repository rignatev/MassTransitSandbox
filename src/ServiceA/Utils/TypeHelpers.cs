using System.Reflection;

namespace ServiceA.Utils;

public static class TypeHelpers
{
    public static IEnumerable<Type> GetAllImplementations(this Assembly assembly, Type type)
    {
        return assembly.DefinedTypes
            .Where(x => x.ImplementedInterfaces.Any(y => y.Name == type.Name))
            .Select(x => x.AsType());
    }
}
