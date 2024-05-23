using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace tgbot
{

    public class Host
    {
        public Action<ITelegramBotClient, Update>? OnMessage;

        private TelegramBotClient kissabot;


        public Host(string token)
        {
            kissabot = new TelegramBotClient(token);

        }

        public void Start()
        {
            kissabot.StartReceiving(Update, Error);
            Console.WriteLine("Start!");
        }

        private async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {

            Console.WriteLine($"New message from {update.Message?.Chat.FirstName}, {update.Message?.Chat.Id}: {update.Message?.Text ?? "[не текст]"}");
            OnMessage?.Invoke(client, update);
            await Task.CompletedTask;
        }

        private async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine("Error:" + exception.Message);
            await Task.CompletedTask;
        }


    }
}