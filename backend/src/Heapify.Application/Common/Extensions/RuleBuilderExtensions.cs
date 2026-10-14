using FluentValidation;
using Heapify.Application.Common.Constants;

namespace Heapify.Application.Common.Extensions;

internal static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, int> ValidatePageIndex<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(PageConstants.MinimumIndex);
    }

    public static IRuleBuilderOptions<T, int> ValidatePageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(PageConstants.MinimumSize)
            .LessThanOrEqualTo(PageConstants.MaximumSize);
    }
}