namespace Turing.Tools.Singletons
{
    public class PureSingleton<T> where T : new()
    {
        private static T _instance;

        public static T Instance => _instance ??= new T();
    }
}