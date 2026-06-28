using Refit;

namespace FlowBoard.Frontend.Services.Helpers;

public static class ApiResponseExtensions
{
    public static string GetErrorMessage<T>(this ApiResponse<T> response)
    {
        if (response.Error is ApiException apiException
            && !string.IsNullOrWhiteSpace(apiException.Content))
        {
            return apiException.Content.Trim();
        }

        return "An unexpected error occurred.";
    }
}