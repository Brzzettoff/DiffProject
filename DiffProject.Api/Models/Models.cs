namespace DiffProject.Api.Models;

using System.Text.Json.Serialization;

//Avoiding strings in code
public enum DiffSide { Left, Right }
public record DataInput(string Data);
public record DiffOffset(int Offset, int Length);
public record DiffResponse(string DiffResultType,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    List<DiffOffset>? Diffs = null
);

//categorizing errors to differ between Business logic custom exceptions and "bugs"
public class DiffDomainException(string msg) : Exception(msg);
public class InvalidBase64Exception() : DiffDomainException("Not a B64 data");
//...some other custom exceptions if needed...

