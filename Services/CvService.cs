namespace TalentFlow.Demo.Services
{
    public class CvService
    {
        private readonly IWebHostEnvironment _env;

        public CvService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IEnumerable<string?> GetPdfFiles()
        {
            var cvDir = Path.Combine(_env.WebRootPath, "cv");
            if (!Directory.Exists(cvDir)) return [];

            return Directory.GetFiles(cvDir, "*.pdf")
                            .Select(Path.GetFileName);
        }

        public string GetPdfPath(string filename) =>
            Path.Combine(_env.WebRootPath, "cv", filename);
    }
}
