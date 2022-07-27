using TNArch.UnitTestSpecification.Core.Astactions;

namespace TNArch.UnitTestSpecification.Core
{
    public static class Specification
    {
        public static IGivenSpecification<T> ForService<T>() where T : class
        {
            return new ServiceSpecification<T>();
        }
    }
}
