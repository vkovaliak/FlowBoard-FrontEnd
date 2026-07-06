using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.State;

public class UserState
{
    private readonly IUserApi _userApi;

    private UserDto? _currentUser;

    public event Action? OnChanged;

    public UserState(IUserApi userApi)
    {
        _userApi = userApi;
    }

    public SubscriptionPlan Plan =>
        _currentUser?.SubscriptionPlan ?? SubscriptionPlan.None;

    public bool IsPro => Plan == SubscriptionPlan.Pro;

    public UserDto? CurrentUser => _currentUser;

    public void NotifyChanged()
    {
        OnChanged?.Invoke();
    }

    public async Task EnsureLoadedAsync()
    {
        if (_currentUser is not null)
        {
            return;
        }

        await LoadAsync();
    }

    public async Task LoadAsync()
    {
        var response = await _userApi.GetMeAsync();

        if (response.IsSuccessStatusCode)
        {
            _currentUser = response.Content;
        }

        NotifyChanged();
    }

    public void Clear()
    {
        _currentUser = null;
        NotifyChanged();
    }
}