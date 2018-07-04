using IngosTelegramBot.Models.Abstract;
using IngosTelegramBot.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace IngosTelegramBot.Models
{
    public class Bot
    {
        private static TelegramBotClient client;
        private static List<Command> commandsList;
        public string phoneNumber;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
        {
            if (client != null)
                return client;

            commandsList = new List<Command>();
            commandsList.Add(new Login());
            commandsList.Add(new ResetPassword());


            client = new TelegramBotClient(AppSettings.Key);
            var hook = string.Format(AppSettings.Url, "api/message/update");
            await client.SetWebhookAsync(hook);

            return client;
        }
    }
}
