using FastCache;

namespace PhlegmaticOne.LocalStorage.Implementation;

public class InMemoryLocalStorageService : ILocalStorageService
{
    public void SetValue<T>(string key, T value, TimeSpan time)
    {
        Cached<T>.Save(key, value, time);
    }

    public T GetValue<T>(string key)
    {
        return Cached<T>.TryGet(key, out var cached) ? cached : default;
    }
}