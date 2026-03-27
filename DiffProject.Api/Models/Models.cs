namespace DiffProject.Api.Models;

using System.Text.Json.Serialization;

//Avoiding strings in code
public enum DiffSide { Left, Right }
public record DataInput(string Data);
public record DiffObject(int Offset, int Length);
public record DiffResponse(string DiffResultType,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    List<DiffObject>? Diffs = null
);

//categorizing errors to differ between Business logic custom exceptions and "bugs"
public class DiffDomainException(string msg) : Exception(msg);
public class InvalidBase64Exception() : DiffDomainException("Expecting base64 encoded payload in body");
//...some other custom exceptions if needed...

