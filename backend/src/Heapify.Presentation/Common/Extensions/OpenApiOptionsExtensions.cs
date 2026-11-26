using Microsoft.AspNetCore.OpenApi;

namespace Heapify.Presentation.Common.Extensions;

internal static class OpenApiOptionsExtensions
{
    private static readonly string[] ExcludedNames = ["Endpoint"];

    extension(OpenApiOptions options)
    {
        public void ConfigureSchemaReferenceId()
        {
            var defaultSchemaReferenceId = options.CreateSchemaReferenceId;

            options.CreateSchemaReferenceId = jsonTypeInfo =>
            {
                var type = jsonTypeInfo.Type;

                if (type.FullName == null || !type.FullName.StartsWith("Heapify"))
                {
                    return defaultSchemaReferenceId(jsonTypeInfo);
                }

                return ExcludedNames.Aggregate(GetSchemaName(type),
                    (current, excludedName) => current.Replace(excludedName, string.Empty));
            };
        }
    }

    private static string GetSchemaName(Type type)
    {
        var name = type.Name;
        
        if (type.IsGenericType)
        {
            var genericTypeName = type.Name.Split('`')[0];

            name = $"{genericTypeName}Of{string.Join("And", type.GetGenericArguments()
                .Select(GetSchemaName))}";
        }

        if (type.DeclaringType is { } declaringType)
        {
            name = $"{GetSchemaName(declaringType)}{name}";
        }

        return name;
    }
}