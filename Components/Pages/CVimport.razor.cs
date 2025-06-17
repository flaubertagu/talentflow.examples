using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;
using UglyToad.PdfPig;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class CVimport
    {
        /// <summary>
        /// Allows you to retrieve the configuration status of your OpenAI settings in TalentFlow.Csharp.AI 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        public bool SemanticIsConfigured { get; set; }
        public void AiConfigResult(bool config) => SemanticIsConfigured = config;
        #endregion


        #region Drag and drop
        private MudForm _form = new();
        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string _dragClass = DefaultDragClass;
        private IBrowserFile? _selectedFile;
        private MudFileUpload<IBrowserFile>? _fileUpload;
        private bool _isProcessing = false;

        public async Task ClearAsync()
        {
            await (_fileUpload?.ClearAsync() ?? Task.CompletedTask);
            _selectedFile = null;
            ClearDragClass();
        }

        private Task OpenFilePickerAsync()
            => _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();
            _selectedFile = e.File;
        }

        private void SetDragClass()
            => _dragClass = $"{DefaultDragClass} mud-border-primary";

        private void ClearDragClass()
            => _dragClass = DefaultDragClass;

        private string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }

            return $"{number:n1} {suffixes[counter]}";
        }

        public async Task Upload()
        {
            _isProcessing = true;
            CVSkills.Clear();
            try
            {
                var file = _selectedFile;

                if (file == null || !file.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    Snackbar.Add("Please only upload a PDF file.", Severity.Error);
                    return;
                }

                var tempPath = Path.Combine(Env.WebRootPath, "uploads");
                Directory.CreateDirectory(tempPath);
                var filePath = Path.Combine(tempPath, $"{Guid.NewGuid()}.pdf");

                await using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(fs);
                }

                await GetCVSkills(filePath);

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error while importing: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isProcessing = false;
            }
        } 
        #endregion

        #region Select file
        private List<string?> PdfFiles = [];
        protected override void OnInitialized()
        {
            PdfFiles = CvService.GetPdfFiles().ToList();
        }

        private bool _loading = false;
        private string? _fileName;
        private async Task LoadCv(string? file)
        {
            try
            {
                _loading = true;
                _fileName = file;
                if (file == null)
                {
                    Snacks.Warning("File name missing");
                    return;
                }

                var filePath = CvService.GetPdfPath(file);
                await GetCVSkills(filePath);
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            finally
            {
                _loading = false;
                _fileName = null;
            }
        } 
        #endregion

        #region Common
        private List<AICurriculumSkill> CVSkills { get; set; } = [];
        private async Task GetCVSkills(string filePath)
        {
            string cvText = await ExtractTextFromPdfAsync(filePath);

            if (string.IsNullOrWhiteSpace(cvText))
            {
                Snackbar.Add("Unable to read the text in the PDF. Use a file with accessible text.", Severity.Warning);
            }
            else
            {
                CVSkills = await AIManager.GetSkillsFromCV(cvText, SupportedLang.Fr);
            }
        }

        private async Task<string> ExtractTextFromPdfAsync(string filePath)
        {
            var sb = new StringBuilder();

            await Task.Run(() =>
            {
                using var document = PdfDocument.Open(filePath);
                foreach (var page in document.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            });

            return sb.ToString();
        }
        #endregion
    }
}
