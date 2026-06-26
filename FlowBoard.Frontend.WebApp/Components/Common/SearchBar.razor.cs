using FlowBoard.Frontend.Domain.DTOs.Search;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Common;

public partial class SearchBar
{
    [Inject] private ISearchService SearchService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string _query = string.Empty;

    private SearchResultDto _results = new([], []);
    private bool _showDropdown;
    private bool _hasSearched;

    private const int MinQueryLength = 3;

    private void OnFocus()
    {
        if (_results.Boards.Count > 0 
            || _results.Cards.Count > 0)
        {
            _showDropdown = true;
        }
    }

    private async Task OnSearchAsync()
    {
        var trimmed = _query.Trim();

        if (trimmed.Length < MinQueryLength)
        {
            _results = new([], []);
            _showDropdown = false;
            _hasSearched = false;
            return;
        }

        _results = await SearchService.SearchAsync(
            trimmed);
        _hasSearched = true;
        _showDropdown = true;
    }

    private void GoToBoard(Guid boardId)
    {
        CloseDropdown();
        _query = string.Empty;
        _results = new([], []);
        NavigationManager.NavigateTo($"/boards/{boardId}"); 
    }

    private void CloseDropdown()
    {
        _showDropdown = false;
    }
}