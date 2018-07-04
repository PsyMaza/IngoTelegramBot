using IngosTelegramBot.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace IngosTelegramBot.Models.Commands
{
    public class Login : Command
    {
        public override string Name => "login";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var user = Users.db.FirstOrDefault(m => m.ChatID == message.Chat.Id);

            if (user == null)
            {
                var keyboard = new ReplyKeyboardMarkup(new[] { new KeyboardButton("Отправить номер телефона") { RequestContact = true } }, true);
                client.SendTextMessageAsync(chatId, "отправьте нам номер телефона", ParseMode.Default, false, false, 0, keyboard);
            }

            client.SendTextMessageAsync(chatId, $"Здравствуйте {user.Name}");
        }
    }
}