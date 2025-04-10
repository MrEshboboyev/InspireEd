namespace InspireEd.Domain.Shared;

public static class ResultExtensions
{
    #region Option 4

    // Standard SelectMany implementation for Result<T>
    public static async Task<Result<TResult>> SelectMany<T, TIntermediate, TResult>(
        this Result<T> result,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TResult> resultSelector)
    {
        if (!result.IsSuccess)
            return Result.Failure<TResult>(result.Error);

        var intermediate = await intermediateSelector(result.Value);
        if (!intermediate.IsSuccess)
            return Result.Failure<TResult>(intermediate.Error);

        return Result.Success(resultSelector(result.Value, intermediate.Value));
    }

    // Overload for when the intermediateSelector returns a non-Task Result
    public static Result<TResult> SelectMany<T, TIntermediate, TResult>(
        this Result<T> result,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TResult> resultSelector)
    {
        if (!result.IsSuccess)
            return Result.Failure<TResult>(result.Error);

        var intermediate = intermediateSelector(result.Value);
        if (!intermediate.IsSuccess)
            return Result.Failure<TResult>(intermediate.Error);

        return Result.Success(resultSelector(result.Value, intermediate.Value));
    }

    // Overload for when intermediateSelector returns a Task<Result<T>>
    public static async Task<Result<TResult>> SelectMany<T, TResult>(
        this Result<T> result,
        Func<T, Task<Result<TResult>>> selector)
    {
        return !result.IsSuccess
            ? Result.Failure<TResult>(result.Error)
            : await selector(result.Value);
    }

    // Overload for when result is a Task<Result<T>>
    public static async Task<Result<TResult>> SelectMany<T, TIntermediate, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TResult> resultSelector)
    {
        var result = await resultTask;
        return await result.SelectMany(intermediateSelector, resultSelector);
    }

    // Select implementation for Result<T>
    public static Result<TResult> Select<T, TResult>(
        this Result<T> result,
        Func<T, TResult> selector)
    {
        return result.IsSuccess
            ? Result.Success(selector(result.Value))
            : Result.Failure<TResult>(result.Error);
    }

    // Task-based Select implementation
    public static async Task<Result<TResult>> Select<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, TResult> selector)
    {
        var result = await resultTask;
        return result.Select(selector);
    }

    #endregion
}
