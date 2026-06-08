using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class BoardDetails
{
    [Parameter] 
    public Guid Id { get; set; }

    [Inject] 
    public IBoardService BoardService { get; set; } = default!;

    [Inject] 
    public ISnackbar Snackbar { get; set; } = default!;

    private BoardDetailsDto? _board;
    private bool _isNotFound = false;

    protected override async Task OnInitializedAsync()
    {
        _board = await BoardService.GetDetailsAsync(Id);

        if (_board is null)
        {
            _isNotFound = true;
            Snackbar.Add("Board not found or access denied.", Severity.Error);
        }
    }
}