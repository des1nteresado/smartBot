using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace smartBot
{
    class Program
    {
        private static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("800246490:AAEHF0OEn3wva-7zfOZBHYpRXwWUu_NOrj0");

            Bot.OnMessage += BotOnMessageReceived;

            var me = Bot.GetMeAsync().Result;
            Console.Out.WriteLine("Name = {0}", me.FirstName);

            Bot.StartReceiving();
            Console.Read();
            Bot.StopReceiving();
        }

        private static void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            Console.Out.WriteLine("message = {0}", message.Text);
        }
    }
}
