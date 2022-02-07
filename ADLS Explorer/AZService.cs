using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ADLS_Explorer
{
    internal static class AZService
    {
        const string AZ_PATH = @"Microsoft SDKs\Azure\CLI2\wbin\az.cmd";

        private static string GetAZPath()
        {
            var roots = new string[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            };

            foreach(string root in roots)
            {
                string fullPath = Path.Combine(root, AZ_PATH);
                if (File.Exists(fullPath))
                    return fullPath;
            }

            throw new Exception("Cannot find az.cmd");
        }


        private static async Task LoginAsync()
        {
            var cmd = new EasyProc.Command();

            var sbErr = new StringBuilder();
            cmd.OnStdErr += (sender, e) =>
            {
                sbErr.AppendLine(e.Text);
            };

            var sbOutput = new StringBuilder();
            cmd.OnStdOut += (sender, e) =>
            {
                sbOutput.AppendLine(e.Text);
            };

            var code = await cmd.RunAsync(GetAZPath(), "login", default);
            if (code != 0)
            {
                string err = sbErr.ToString();
                if (string.IsNullOrWhiteSpace(err))
                    throw new Exception($"Login failed: code {code}");

                throw new Exception(err);
            }
        }

        private static async Task<T> TryFunctionAsync<T>(Func<object, Task<T>> func, object args)
        {
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    var ret = await func(args);
                    return ret;
                }
                catch (Exception ex)
                {
                    if (i == 0 && ex.Message.Contains("ERROR: Please run 'az login' to setup account."))
                        await LoginAsync();
                    else
                        throw;
                }
            }

            throw new Exception("Impossible failure inside AZService.TryFunction. Tell Jason");
        }

        private static async Task TryActionAsync(Func<object, Task> func, object args)
        {
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    await func(args);
                    return;
                }
                catch (Exception ex)
                {
                    if (i == 0 && ex.Message.Contains("ERROR: Please run 'az login' to setup account."))
                        await LoginAsync();
                    else
                        throw;
                }
            }

            throw new Exception("Impossible failure inside AZService.TryAction. Tell Jason");
        }


        
        private static async Task<List<AZFSO>> ListAsync(object args)
        {
            var cmd = new EasyProc.Command();

            var sbErr = new StringBuilder();
            cmd.OnStdErr += (sender, e) =>
            {
                sbErr.AppendLine(e.Text);
            };

            var sbOutput = new StringBuilder();
            cmd.OnStdOut += (sender, e) =>
            {
                sbOutput.AppendLine(e.Text);
            };

            var code = await cmd.RunAsync(GetAZPath(), args as string, default);
            if (code != 0)
            {
                string err = sbErr.ToString();
                if (string.IsNullOrWhiteSpace(err))
                    throw new Exception($"List failed: code {code}");

                throw new Exception(err);
            }

            var ret = JsonSerializer.Deserialize<List<AZFSO>>(sbOutput.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web));
            ret.Sort();

            return ret;
        }

        private static async Task CommandWithNoResponseAsync(object args)
        {
            var cmd = new EasyProc.Command();

            var sbErr = new StringBuilder();
            cmd.OnStdErr += (sender, e) =>
            {
                sbErr.AppendLine(e.Text);
            };

            var code = await cmd.RunAsync(GetAZPath(), args as string, default);
            if (code != 0)
            {
                string err = sbErr.ToString();
                if (string.IsNullOrWhiteSpace(err))
                    throw new Exception($"Command failed: code {code}");

                throw new Exception(err);
            }
        }

        private static async Task TransferWithProgressAsync(object args)
        {
            var argsArray = args as object[];
            var sArgs = (string)argsArray[0];
            var progress = (IProgress<double>)argsArray[1];
            var cancellationToken = (CancellationToken)argsArray[2];

            var cmd = new EasyProc.Command();

            var sbErr = new StringBuilder();
            cmd.OnStdErr += (sender, e) =>
            {
                string s = (e.Text + string.Empty).Trim();
                if (s.StartsWith("Alive[") || s.StartsWith("Finished["))
                {
                    try
                    {
                        s = s.Substring(s.IndexOf("]")).Trim(new char[] { ']', ' ', '%' });
                        progress.Report(double.Parse(s));
                    }
                    catch { }
                }
                else
                {
                    sbErr.AppendLine(e.Text);
                }
            };            

            var code = await cmd.RunAsync(GetAZPath(), sArgs, cancellationToken);
            if (code != 0)
            {
                string err = sbErr.ToString();
                if (string.IsNullOrWhiteSpace(err))
                    throw new Exception($"Transfer failed: code {code}");

                throw new Exception(err);
            }
        }




        public static Task<List<AZFSO>> GetRootObjectsAsync(AZContainer azContainer, bool recursive = false)
        {
            string args = $"storage fs file list --auth-mode login --account-name \"{azContainer.Account}\" --file-system \"{azContainer.Container}\" --recursive {recursive}";
            return TryFunctionAsync(ListAsync, args);
        }

        public static Task<List<AZFSO>> GetChildObjectsAsync(AZContainer azContainer, AZFSO azFSO, bool recursive = false)
        {
            if (!azFSO.IsDirectory)
                throw new Exception("The specified item is not a directory");
            string args = $"storage fs file list --auth-mode login --account-name \"{azContainer.Account}\" --file-system \"{azContainer.Container}\" --path \"{azFSO.Name}\" --recursive {recursive}";
            return TryFunctionAsync(ListAsync, args);
        }

        public static Task DeleteAsync(AZContainer azContainer, AZFSO azFSO)
        {
            string s1 = azFSO.IsDirectory ? "directory" : "file";
            string s2 = azFSO.IsDirectory ? "name" : "path";
            string args = $"storage fs {s1} delete --auth-mode login --account-name \"{azContainer.Account}\" --file-system \"{azContainer.Container}\" --{s2} \"{azFSO.Name}\" --yes";
           
            return TryActionAsync(CommandWithNoResponseAsync, args);
        }


        public static Task MoveAsync(AZContainer azContainer, AZFSO azFSO, string newPath)
        {
            string s1 = azFSO.IsDirectory ? "directory" : "file";
            string s2 = azFSO.IsDirectory ? "name" : "path";
            string s3 = azFSO.IsDirectory ? "new-directory" : "new-path";
            string args = $"storage fs {s1} move --auth-mode login --account-name \"{azContainer.Account}\" --file-system \"{azContainer.Container}\" --{s2} \"{azFSO.Name}\" --{s3} \"{azContainer.Container}/{newPath}\"";

            return TryActionAsync(CommandWithNoResponseAsync, args);
        }

        public static Task CreateDirectoryAsync(AZContainer azContainer, AZFSO parent, string name)
        {
            string args = $"storage fs directory create --auth-mode login --account-name \"{azContainer.Account}\" --file-system \"{azContainer.Container}\" --name \"{parent.Name}/{name}\"";
            return TryActionAsync(CommandWithNoResponseAsync, args);
        }

        public static Task DownloadFileAsync(AZContainer azContainer, AZFSO file, string destFile, IProgress<double> progress, CancellationToken cancellationToken = default)
        {            
            Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            string args = $"storage blob download --auth-mode login --account-name \"{azContainer.Account}\" --container-name \"{azContainer.Container}\" --name \"{file.Name}\" --file \"{destFile}\"";
            return TryActionAsync(TransferWithProgressAsync, new object[] { args, progress, cancellationToken });
        }

        public static Task UploadFileAsync(AZContainer azContainer, AZFSO destFile, string srcFile, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            string args = $"storage blob upload --auth-mode login --account-name \"{azContainer.Account}\" --container-name \"{azContainer.Container}\" --name \"{destFile.Name}\" --file \"{srcFile}\"";
            return TryActionAsync(TransferWithProgressAsync, new object[] { args, progress, cancellationToken });
        }

        public static async Task<bool> FileExistsAsync(AZContainer azContainer, string path, CancellationToken cancellationToken = default)
        {
            try
            {
                string args = $"storage fs file show --auth-mode login --account-name \"{azContainer.Account}\" --file-system \"{azContainer.Container}\" --path \"{path}\"";
                await TryActionAsync(CommandWithNoResponseAsync, args);
                return true;
            }
            catch { return false; }
        }
    }
}
