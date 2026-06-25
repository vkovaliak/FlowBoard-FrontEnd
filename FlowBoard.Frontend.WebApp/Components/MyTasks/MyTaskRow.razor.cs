using System.Globalization;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.MyTasks;

public partial class MyTaskRow
{
    [Parameter] public MyCardDto Task { get; set; } = default!;
    [Parameter] public EventCallback<MyCardDto> OnClick { get; set; }

    private Color GetDateColor()
    {
        if (!Task.DueDate.HasValue) 
            return Color.Default;

        var due = Task.DueDate.Value.Date;
        var today = DateTime.Today;

        if (due < today) 
            return Color.Error;

        if (due == today) 
            return Color.Warning;

        return Color.Default;
    }

    private string FormatDate(DateTime date)
    {
        var d = date.Date;
        
        if (d == DateTime.Today) 
            return "Today";

        if (d == DateTime.Today.AddDays(1)) 
            return "Tomorrow";
            
        return date.ToString("MMM d", CultureInfo.InvariantCulture);
    }
}