using DiffProject.Api.Dao;
using DiffProject.Api.Models;

namespace DiffProject.Api.Services;

public class DiffService(IDiffRepository repo)
{
    public void StoreData(string id, string data, DiffSide side)
    {
        try
        {
            Convert.FromBase64String(data);
        }
        catch
        {
            throw new InvalidBase64Exception();
        }

        var entity = repo.GetDiffEntity(id);
        if (side == DiffSide.Left)
            entity.Left = data;
        else
            entity.Right = data;

        repo.UpdateDiffEntity(id, entity);
    }

    public DiffResponse? GetDiff(string id)
    {
        var entity = repo.GetDiffEntity(id);
        if (entity.Left == null || entity.Right == null)
        {
            return null;
        }

        if (entity.Left == entity.Right)
        {
            return new("Equals");
        }

        if (entity.Left.Length != entity.Right.Length)
        {
            return new("SizeDoNotMatch");
        }

        return new("ContentDoNotMatch", CalculateOffsets(entity.Left, entity.Right));
    }

    //Some basic comparrison logic.
    private static List<DiffObject> CalculateOffsets(string left, string right)
    {
        var result = new List<DiffObject>();
        //The sizes of left/right were checked before. Both are equal.
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                int start = i;
                while (i < left.Length && left[i] != right[i]) i++;
                result.Add(new(start, i - start));
            }
        }
        return result;
    }
}
