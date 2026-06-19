using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Theme;

public static class AppTheme
{
    public static MudTheme Build() => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2563EB",
            Secondary = "#6366F1",
            Tertiary = "#7C3AED",

            Success = "#22C55E",
            Error = "#EF4444",
            Warning = "#F59E0B",
            Info = "#3B82F6",

            Background = "#F8FAFC",
            Surface = "#FFFFFF",
            DrawerBackground = "#1E293B",
            DrawerText = "#CBD5E1",
            DrawerIcon = "#94A3B8",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#172B4D",

            TextPrimary = "#172B4D",
            TextSecondary = "#64748B",
            TextDisabled = "#94A3B8",

            LinesDefault = "#E2E8F0",
            LinesInputs = "#CBD5E1",
            Divider = "#E2E8F0",
            TableLines = "#E2E8F0",

            ActionDefault = "#64748B",
            HoverOpacity = 0.06,
        },

        PaletteDark = new PaletteDark
        {
            Primary = "#3B82F6",
            Secondary = "#818CF8",
            Tertiary = "#A78BFA",
            Success = "#22C55E",
            Error = "#EF4444",
            Warning = "#F59E0B",
            Info = "#60A5FA",
            Background = "#0F1729",
            Surface = "#1E293B",
            DrawerBackground = "#243044",
            DrawerText = "#CBD5E1",
            AppbarBackground = "#1E293B",
            AppbarText = "#F1F5F9",
            TextPrimary = "#F1F5F9",
            TextSecondary = "#94A3B8",
            LinesDefault = "#334155",
            LinesInputs = "#475569",
            Divider = "#334155",
        },

        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "Helvetica Neue", "Arial", "sans-serif"],
            },
            Button = new ButtonTypography
            {
                TextTransform = "none", 
                FontWeight = "600",
            },
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "10px",
            DrawerWidthLeft = "260px",
        },
    };
}