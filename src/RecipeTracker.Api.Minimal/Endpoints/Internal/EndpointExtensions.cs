namespace RecipeTracker.Api.Minimal.Endpoints.Internal;

public static class EndpointExtensions
{
    public static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        var endpointTypes = typeof(TMarker).Assembly.DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && typeof(IEndpoints).IsAssignableFrom(type));

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoints.DefineEndpoints))
                ?.Invoke(null, new object[] { app });
        }
    }
}