using IngosTelegramBot.Models.Abstract;
using System;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Web.Configuration;
using System.DirectoryServices;

namespace IngosTelegramBot.Models.Commands
{
    public class ResetPassword : Command
    {
        public override string Name => "ResetPassword";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            if (Users.db.FirstOrDefault(user => user.ChatID.Equals(chatId)).Name == null)
            {
                client.SendTextMessageAsync(chatId, $"Введите команду /login");
            }
            else
            {
                var newPassword = GetNewPassword();
                ResetUserPassword(chatId, newPassword);

                client.SendTextMessageAsync(chatId, $"Ваш новый пароль: {newPassword}");
            }
            
        }

        private string GetNewPassword()
        {
            string parametrs = "qwertyuiopasdfghjklzxcvbnm123456789QWERTYUIOPASDFGHJKLZXCVBNM";
            int length = 8;
            StringBuilder result = new StringBuilder();


            Random rnd = new Random();
            int lng = parametrs.Length;

            for (int i = 0; i <= length; i++)
            {
                result.Append(parametrs[rnd.Next(lng)]);
            }

            return result.ToString();
        }

        private void ResetUserPassword(long chatId, string password)
        {
            var userDn = Users.db.FirstOrDefault(user => user.ChatID.Equals(chatId)).Name;

            var directoryEntry = GetDirectoryEntryByUserName(userDn);
            directoryEntry.Invoke("SetPassword", new object[] { $"{password}" });
            directoryEntry.Properties["LockOutTime"].Value = 0;

            directoryEntry.Close();
        }

        private static DirectoryEntry GetDirectoryEntryByUserName(string userName)
        {
            var de = GetDirectoryObject(GetDomain());
            var deSearch = new DirectorySearcher(de) { SearchRoot = de, Filter = "(&(objectCategory=user)(cn=" + userName + "))" };

            var result = deSearch.FindOne();
            return result != null ? result.GetDirectoryEntry() : null;
        }

        private static string GetDomain()
        {
            string adDomain = WebConfigurationManager.AppSettings["adDomainFull"];

            var domain = new StringBuilder();
            string[] dcs = adDomain.Split('.');
            for (int i = 0; i < dcs.GetUpperBound(0) + 1; i++)
            {
                domain.Append("DC=" + dcs[i]);
                if (i < dcs.GetUpperBound(0))
                {
                    domain.Append(",");
                }
            }

            return domain.ToString();
        }

        private static DirectoryEntry GetDirectoryObject(string domainReference)
        {
            string adminUser = WebConfigurationManager.AppSettings["adAdminUser"];
            string adminPassword = WebConfigurationManager.AppSettings["adAdminPassword"];
            string fullPath = "LDAP://" + domainReference;

            var directoryEntry = new DirectoryEntry(fullPath, adminUser, adminPassword, AuthenticationTypes.Secure);
            return directoryEntry;
        }
    }
}