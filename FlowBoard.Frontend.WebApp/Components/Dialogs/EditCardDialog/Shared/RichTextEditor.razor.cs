using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Shared;

public partial class RichTextEditor
{
    private const string ApiKey = "ril1s7tjuhstzun5vtt9z4mxgpe0fpoxjohfi3vumix7zf2v";

    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    private string? _internalValue;

    protected override void OnParametersSet()
    {
        _internalValue = Value;
    }

    private async Task OnInternalChanged(string? value)
    {
        _internalValue = value;
        await ValueChanged.InvokeAsync(value);
    }

    private readonly Dictionary<string, object> _config = new()
    {
        ["height"] = 100,
        ["menubar"] = false,
        ["statusbar"] = false,
        ["plugins"] = "lists link autoresize image",
        ["toolbar"] =
            "blocks | bold italic underline | " +
            "bullist numlist | alignleft aligncenter alignright | link"
    };
}