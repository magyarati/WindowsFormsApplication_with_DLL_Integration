using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    class SaveHandler : ISaveHandler
    {
        public async Task SaveLogAsync(
            Func<string> getContent,
            Action<string> log,
            bool silent = false,
            string silentFilePath = null)
        {
            while (true)
            {
                string targetPath;
                if (silent)
                {
                    if (string.IsNullOrWhiteSpace(silentFilePath))
                    {
                        silentFilePath = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            $"log_auto_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                    }
                    targetPath = silentFilePath;
                }
                else
                {
                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Text fájl (*.txt)|*.txt";
                        sfd.Title = "Mentés fájlba";
                        sfd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        sfd.FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                        if (sfd.ShowDialog() != DialogResult.OK)
                        {
                            log(">> Mentés megszakítva, nem lett fájl kiválasztva.");
                            return;
                        }

                        targetPath = sfd.FileName;
                    }
                }

                log($">> Log mentése elkezdődött: {targetPath}");
                var content = getContent();
                var data = Encoding.UTF8.GetBytes(content);

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

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
                    return;
                }
                catch (IOException ioex)
                {
                    log($"[HIBA] Mentés sikertelen, fájl használatban lehet: {ioex.Message}");

                    if (!silent)
                    {
                        var retry = MessageBox.Show(
                            $"Nem sikerült menteni, a fájl lehet, hogy használatban van:\n\n{ioex.Message}\n\nPróbálsz másik fájlt?",
                            "Hiba mentéskor",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning);

                        if (retry == DialogResult.Cancel)
                        {
                            log(">> Felhasználó megszakította a mentést.");
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log($"[HIBA] Ismeretlen mentési hiba: {ex.Message}");
                    return;
                }
            }
        }
    }
}
