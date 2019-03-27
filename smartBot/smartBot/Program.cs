using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace smartBot
{
    class Program
    {
        private static TelegramBotClient Bot;
        private const string KEY = "800246490:AAEHF0OEn3wva-7zfOZBHYpRXwWUu_NOrj0";
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("kek");//remove before push
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallBackQueryReceived;

            var me = Bot.GetMeAsync().Result;
            Console.Out.WriteLine("Name = {0}", me.FirstName);

            Bot.StartReceiving();
            Console.Read();
            Bot.StopReceiving();
        }

        private static void BotOnCallBackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }
            var name = $"{message.From.FirstName} {message.From.LastName}";
            Console.Out.WriteLine($"{name}: {message.Text}");

            switch (message.Text)
            {
                case "/start":
                    string text =
@"Приветствую странник! Этот бот умеет многое..
Вот список доступных комманд:
/start - Запуск бота
/menu - Вывод меню
/keyboard - Вывод клавиатуры";
                    await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;
                case "/keyboard":
                    break;
                case "/menu":
                    var menuKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/des1nteresado"),
                            InlineKeyboardButton.WithUrl("Github", "https://github.com/des1nteresado"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 2"),
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт меню", replyMarkup: menuKeyboard);
                    break;
                default:
                    break;
            }
        }
    }
}
