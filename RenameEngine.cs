using System.IO;
namespace OpenRename;
public sealed record RenameItem(string OriginalPath, string ProposedPath);
public static class RenameEngine
{
    public static IReadOnlyList<RenameItem> Preview(IEnumerable<string> paths, string find, string replace) => paths.Select(path => new RenameItem(path, Path.Combine(Path.GetDirectoryName(path)!, Path.GetFileName(path).Replace(find, replace, StringComparison.Ordinal)))).ToArray();
    public static IReadOnlyList<RenameItem> Validate(IEnumerable<RenameItem> items)
    {
        var list = items.ToArray(); var duplicates = list.GroupBy(x => x.ProposedPath, StringComparer.OrdinalIgnoreCase).Where(g => g.Count() > 1).SelectMany(g => g).ToHashSet();
        if (duplicates.Count > 0) throw new InvalidOperationException("Two or more files would receive the same name.");
        if (list.Any(x => !string.Equals(x.OriginalPath, x.ProposedPath, StringComparison.OrdinalIgnoreCase) && File.Exists(x.ProposedPath))) throw new IOException("A proposed filename already exists.");
        return list;
    }
    public static void Execute(IEnumerable<RenameItem> items) { var list = Validate(items); foreach (var item in list) if (!string.Equals(item.OriginalPath, item.ProposedPath, StringComparison.OrdinalIgnoreCase)) File.Move(item.OriginalPath, item.ProposedPath); }
}
