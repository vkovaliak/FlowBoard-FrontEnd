namespace FlowBoard.Frontend.Services.State;

public class TasksState
{
    public event Action? OnChanged;

    public void NotifyChanged()
    {
        OnChanged?.Invoke();
    }
}