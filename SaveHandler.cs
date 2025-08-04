using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    class SaveHandler : ISaveHandler
    {
        public async Task SaveLogAsync(
            string content,
            Action<string> log,
            bool silent = false,
            string silentFilePath = null)
        {
            string targetPath = silent
                ? silentFilePath
                  ?? Path.Combine(
                      AppDomain.CurrentDomain.BaseDirectory,
                      $"log_auto_{DateTime.Now:yyyyMMdd_HHmmss}.txt")
                : null;

            if (!silent)
            {
                using (var sfd = new SaveFileDialog
                {
                    Filter = "Text fájl (*.txt)|*.txt",
                    Title = "Mentés fájlba",
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                })
                {
                    if (sfd.ShowDialog() != DialogResult.OK)
                    {
                        log(">> Mentés megszakítva, nem lett fájl kiválasztva.");
                        return;
                    }

                    targetPath = sfd.FileName;
                }
            }

            log($">> Log mentése elkezdődött: {targetPath}");
            var data = Encoding.UTF8.GetBytes(content);

            try
            {
                var dir = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrWhiteSpace(dir))
                    Directory.CreateDirectory(dir);

                using (var fs = new FileStream(
                    targetPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,
                    useAsync: true))
                {
                    await fs.WriteAsync(data, 0, data.Length);
                    await fs.FlushAsync();
                }

                log($">> Log sikeresen elmentve: {targetPath}");
            }
            catch (IOException ioex)
            {
                log($"[HIBA] Mentés sikertelen, fájl használatban lehet: {ioex.Message}");
            }
            catch (Exception ex)
            {
                log($"[HIBA] Ismeretlen mentési hiba: {ex.Message}");
            }
        }
    }
}
