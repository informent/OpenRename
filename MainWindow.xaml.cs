using System.Windows;
using System.IO;
using Forms = System.Windows.Forms;
namespace OpenRename;
public partial class MainWindow : Window
{
    private string? folder; private IReadOnlyList<RenameItem> plan = Array.Empty<RenameItem>(); private IReadOnlyList<RenameItem> previous = Array.Empty<RenameItem>();
    public MainWindow() => InitializeComponent();
    private void ChooseFolder_Click(object sender, RoutedEventArgs e) { using var dialog = new Forms.FolderBrowserDialog { Description = "Choose a folder whose files you want to rename" }; if (dialog.ShowDialog() == Forms.DialogResult.OK) { folder = dialog.SelectedPath; RebuildPreview(); } }
    private void RuleChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => RebuildPreview();
    private void RebuildPreview() { if (folder is null) return; try { plan = RenameEngine.Validate(RenameEngine.Preview(Directory.EnumerateFiles(folder), FindBox.Text, ReplaceBox.Text)); PreviewList.ItemsSource = plan.Select(x => $"{Path.GetFileName(x.OriginalPath)}  →  {Path.GetFileName(x.ProposedPath)}").ToArray(); CountText.Text = $"{plan.Count:N0} files"; StatusText.Text = "Preview ready"; } catch (Exception ex) { StatusText.Text = ex.Message; } }
    private void Apply_Click(object sender, RoutedEventArgs e) { if (plan.Count == 0) return; try { previous = plan; RenameEngine.Execute(plan); StatusText.Text = $"Renamed {plan.Count:N0} files"; RebuildPreview(); } catch (Exception ex) { StatusText.Text = ex.Message; } }
    private void Undo_Click(object sender, RoutedEventArgs e) { if (previous.Count == 0) { StatusText.Text = "No rename plan to undo"; return; } try { RenameEngine.Execute(previous.Select(x => new RenameItem(x.ProposedPath, x.OriginalPath))); previous = Array.Empty<RenameItem>(); StatusText.Text = "Last rename undone"; RebuildPreview(); } catch (Exception ex) { StatusText.Text = ex.Message; } }
}
