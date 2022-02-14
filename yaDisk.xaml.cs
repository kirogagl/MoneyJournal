using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace finWpf
{
    /// <summary>
    /// Логика взаимодействия для yaDisk.xaml
    /// </summary>
    public partial class yaDisk : Window
    {
        Regex regToken = new Regex("access_token=(?<token>[^&]+)", RegexOptions.Compiled);
        private const string name = "MoneyJournal";
        public object Token { get; private set; }
        public bool Success { get; private set; }
        public object ClientID { get; set; }

        private MainWindow mainWindow;
        public yaDisk(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// Getting a token
        /// </summary>
        private void wb_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith("https://oauth.yandex.ru/verification_code#"))
            {
                var m = regToken.Match(e.Uri.AbsoluteUri);
                if (m.Success)
                {
                    Token = m.Groups["token"].Value;
                    Success = true;
                }
            }
        }

        /// <summary>
        /// Link to get a yandex token
        /// </summary>
        private void wb_Loaded(object sender, RoutedEventArgs e)
        {
            wb.Navigate($@"https://oauth.yandex.ru/authorize?response_type=token&client_id={ClientID}");
        }

        /// <summary>
        /// Save to YaDisk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //https://oauth.yandex.ru/verification_code#access_token=AQAAAAAOsluvAAer9GZcix12-keLnzqwuO5RjTM&token_type=bearer&expires_in=31325931
            Task.Run(async () =>
            {
                try
                {
                    if (Success && !string.IsNullOrEmpty(Token.ToString()))
                    {
                        DiskHttpApi? api = new DiskHttpApi(Token.ToString());
                        Resource? rootFolderData;
                        rootFolderData = await api.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" });
                        if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(name)))
                        {
                            await api.Commands.CreateDictionaryAsync("/" + name);
                        }

                        string[]? files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "mj*.json");

                        foreach (var file in files)
                        {
                            Link? link = await api.Files.GetUploadLinkAsync("/" + name + "/" + Path.GetFileName(file), overwrite: true);
                            using (var fs = File.OpenRead(file))
                            {
                                await api.Files.UploadAsync(link, fs);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        /// <summary>
        /// Load from YaDisk
        /// </summary>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (Success && !string.IsNullOrEmpty(Token.ToString()))
                    {
                        DiskHttpApi? api = new DiskHttpApi(Token.ToString());
                        Resource? rootFolderData;
                        rootFolderData = await api.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" });
                        if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(name)))
                        {
                            throw new Exception("The folder is missing on the cloud storage");
                        }
                        else
                        {
                            var link = await api.Files.GetDownloadLinkAsync("/" + name + "/" + "mj.json");
                            var file = await api.Files.DownloadAsync(link);
                            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\mj.json", FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                byte[] buffer = new byte[file.Length];
                                file.Read(buffer, 0, (int)file.Length);
                                fs.Write(buffer, 0, buffer.Length);
                                file.Close();
                            }
                            link = await api.Files.GetDownloadLinkAsync("/" + name + "/" + "mj2.json");
                            file = await api.Files.DownloadAsync(link);
                            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\mj2.json", FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                byte[] buffer = new byte[file.Length];
                                file.Read(buffer, 0, (int)file.Length);
                                fs.Write(buffer, 0, buffer.Length);
                                file.Close();
                            }
                        }
                        Dispatcher.Invoke((Action)(() => mainWindow.JSONDeserializer()));
                        Dispatcher.Invoke((Action)(() => this.Close()));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}
