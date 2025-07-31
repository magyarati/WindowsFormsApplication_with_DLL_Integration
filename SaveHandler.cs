using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    class SaveHandler
    {
        public async Task SaveLogAsync(Func<string> getContent, Action<string> log)
        {
            while (true)
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

                    string content = getContent();

                    try
                    {
                        await Task.Run(() => File.WriteAllText(sfd.FileName, content));

                        log($">> Log sikeresen elmentve: {sfd.FileName}");
                        return;
                    }
                    catch (IOException ioex)
                    {
                        log($"[HIBA] Mentés sikertelen, fájl használatban lehet: {ioex.Message}");
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
                    catch (Exception ex)
                    {
                        log($"[HIBA] Ismeretlen mentési hiba: {ex.Message}");
                        return;
                    }
                }
            }
        }
    }
}
