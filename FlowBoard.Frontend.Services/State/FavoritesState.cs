namespace FlowBoard.Frontend.Services.State;

public class FavoritesState
{
    public event Action? OnChanged;

    public void NotifyChanged()
    {
        OnChanged?.Invoke();
    }
}