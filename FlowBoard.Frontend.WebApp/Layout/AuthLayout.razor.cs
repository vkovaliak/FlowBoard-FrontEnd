using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class AuthLayout
{
    [Inject] 
    public IAuthService AuthService { get; set; } = default!;
}