namespace Agronomia.Domain.SeedWork;

/// <summary>
/// Represents a domain error with a stable error <c>Code</c> and a human-readable <c>Message</c>.
/// </summary>
/// <remarks>
/// <para>
/// Use <c>Code</c> as a stable identifier across the system (e.g., for localization or API contracts),
/// and use <c>Message</c> as a user-facing description that can be localized or enriched at the boundaries.
/// </para>
/// <para>
/// This type is typically used together with a Result pattern to propagate failures without throwing exceptions in normal control flow.
/// The <c>Code</c> is commonly sourced from the <see cref="Errors.Movies.Errors"/> or
/// <see cref="Errors.Genres.Errors"/> catalogs.
/// </para>
/// <para>
/// Example:
/// <code>
/// // Create an error using a domain error code catalog entry
/// var error = new Error(TheMovie.Domain.Errors.Movies.Errors.Movie.NotFound, "Movie not found.");
///
/// // Optionally, propagate it via your Result type
/// // return Result.Failure(error);
/// </code>
/// </para>
/// </remarks>
/// <param name="Code">A stable, machine-friendly identifier for the error (e.g., <c>"Movie.NotFound"</c>).</param>
/// <param name="Message">A human-readable description suitable for display or logging.</param>
public record Error(string Code, string Message);
