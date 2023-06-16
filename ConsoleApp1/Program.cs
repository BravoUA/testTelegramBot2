using Telegram.Bot;
using Telegram.Bot.Types;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using ConsoleApp1.Models;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        public static dbConnect dbconnect;
        public static List<Client> clients;
        static void Main(string[] args)
        {
            dbconnect = new dbConnect();
            var client = new TelegramBotClient("5727159643:AAGOihRgjykiZWVyCnuOtiNpgo77oGGuH_A");
             clients= new List<Client>();   
             clients = dbconnect.Client.ToList();  
            client.StartReceiving(Update2, Error2);
       
            Console.ReadLine();
            
        }

        async static  Task Error2(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            
            return;
                }

        async static  Task Update2(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var massage = update.Message;
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{new KeyboardButton[] { "Записати", "Показати записи" },})
            {
                ResizeKeyboard = true
            };
            ReplyKeyboardMarkup replyKeyboardMarkup2 = new(new[] { new KeyboardButton[] { "Картопля", "Цибуля","Морква" }, })
            {
                ResizeKeyboard = true
            };
            ReplyKeyboardMarkup replyKeyboardMarkup3 = new(new[] { new KeyboardButton[] { "Записи по картоплі", "Записи по цибулі", "Записи по моркві" }, })
            {
                ResizeKeyboard = true
            };


            if (massage.Text != null)
            {
                Console.WriteLine($"{massage.Chat.FirstName} | {massage.Text}");
                if (massage.Text.Contains("Записати"))
                {
                    await botClient.SendTextMessageAsync(massage.Chat.Id, "Записати", replyMarkup: replyKeyboardMarkup2) ;
                    return;
                }
                if (massage.Text.Contains("Картопля") || massage.Text.Contains("Цибуля") || massage.Text.Contains("Морква"))
                {

                    await botClient.SendTextMessageAsync(massage.Chat.Id, "Форма запису\nІмя:\nКількість:\nЦіна за КГ:\nЗагальна сума:\n+", replyMarkup: replyKeyboardMarkup2);
                    return;
                }
                if (massage.Text.Contains("+"))
                {
                    await botClient.SendTextMessageAsync(massage.Chat.Id, "Готово", replyMarkup: replyKeyboardMarkup);
                    return;
                }
                if (massage.Text.Contains("Показати записи")) {
                    await botClient.SendTextMessageAsync(massage.Chat.Id, "Записати", replyMarkup: replyKeyboardMarkup3);
                    return;
                }
                    string ask = "покажи всіх";
                if (massage.Text.ToLower().Contains(ask))
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        await botClient.SendTextMessageAsync(massage.Chat.Id, i+" "+clients[i].Name, replyMarkup: replyKeyboardMarkup);
                    }
                   
                    return;
                }
            }
        }
    }
}