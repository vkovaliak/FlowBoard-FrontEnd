namespace FlowBoard.Frontend.Services.State;

public class PresenceState
{
    private readonly HashSet<Guid> _online = new();
    public event Action? OnChanged;

    public bool IsOnline(Guid userId) 
        => _online.Contains(userId);

    public void SetOnline(IReadOnlyCollection<Guid> users)
    {
        _online.Clear();
        foreach (var id in users) _online.Add(id);
        OnChanged?.Invoke();
    }

    public void Add(Guid id)
    {
        if (_online.Add(id)) OnChanged?.Invoke();
    }

    public void Remove(Guid id)
    {
        if (_online.Remove(id)) OnChanged?.Invoke();
    }

    public void Clear()
    {
        _online.Clear();
        OnChanged?.Invoke();
    }
}