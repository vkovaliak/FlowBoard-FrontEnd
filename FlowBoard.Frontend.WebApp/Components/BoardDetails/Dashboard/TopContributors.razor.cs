using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Dashboard;

public partial class TopContributors
{
    [Parameter] public BoardDetailsDto Board { get; set; } = default!;

    private List<ContributorModel> _contributors = [];
    private int _max = 1;

    protected override void OnParametersSet()
    {
        _contributors = Board.Lists
            .SelectMany(l => l.Cards ?? [])
            .SelectMany(c => c.Assignees)
            .GroupBy(a => new { a.UserName, a.AvatarUrl })
            .Select(g => new ContributorModel
            {
                UserName = g.Key.UserName, 
                AvatarUrl = g.Key.AvatarUrl, 
                Count = g.Count()
            })
            .OrderByDescending(c => c.Count)
            .Take(5)
            .ToList();

        _max = _contributors.Count > 0
            ? _contributors.Max(c => c.Count)
            : 1;
    }

    private double GetPercent(int count) 
        => (double)count / _max * 100;
}