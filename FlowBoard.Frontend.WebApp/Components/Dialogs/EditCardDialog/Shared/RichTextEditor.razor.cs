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

    private readonly Dictionary<string, object> _config = new()
    {
        ["height"] = 100,
        [ "auto_focus"] = true,
        ["menubar"] = false,
        ["statusbar"] = false,
        ["plugins"] = "lists link autoresize",
        ["toolbar"] = 
            "blocks | bold italic underline" +
            "| bullist numlist " +
            "| alignleft aligncenter alignright "+
            "| link",
        ["content_style"] =
            "body { " +
                "font-family: Inter, sans-serif; " +
                "font-size: 14px; " +
                "word-wrap: break-word; " + 
                "overflow-wrap: break-word; " + 
                "white-space: normal; " +  
                "overflow-x: hidden; " + 
                "max-width: 100%; " +
            "}",
    };
}