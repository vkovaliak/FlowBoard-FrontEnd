namespace FlowBoard.Frontend.Services.State;

public class UserState
{
    public event Action? OnChanged;

    public void NotifyChanged()
    {
        OnChanged?.Invoke();
    }
}