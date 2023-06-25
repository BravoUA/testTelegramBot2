using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ConsoleApp1
{
     class dataConvert
    {
         private Message massage;
        public History History = new History();
        List<Client> Ollclients = new List<Client>();
        List<Client> client = new List<Client>();
        List<Products> products = new List<Products>();
        dbConnect dbconnect = new dbConnect();
        int state;
        public dataConvert(Message massage, int State) {
            this.massage = massage;
            Ollclients = dbconnect.Client.ToList();
            products = dbconnect.Products.ToList();
            state = State;
            ConvertToNormal();
           
        }

        public void ConvertToNormal() {
            string date = massage.Text;
            string[] dates = date.Split('\n');
            string NameProduct = "";
            string NameClient = dates[0];
            switch (state) {
                case 1:
                    NameProduct = "Картопля";
                    break;
                    case 2:
                    NameProduct = "Цибуля";
                    break;
                case 3:
                    NameProduct = "Морква";
                    break;
            }

          
            client = (from a in Ollclients where a.Name == NameClient select a).ToList();

            if (client.Count == 0)
            {
                Client newClient = new Client();
                newClient.Name = NameClient;
                Ollclients.Add(newClient);
                dbconnect.Client.Add(newClient);
                dbconnect.SaveChangesAsync();
                Ollclients = dbconnect.Client.ToList();
                client = (from a in Ollclients where a.Name == NameClient select a).ToList();
            }
            products =(from a in products where a.Name == NameProduct select a).ToList();
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            History.ClientId = client[0].id;
            History.ProductId = products[0].id;
            History.ProductPrice = double.Parse(dates[2], formatter);
            History.TotalAmoung = Convert.ToDecimal( dates[3], formatter);
            History.DateParches = massage.Date.ToShortDateString();
            History.Totalweight = int.Parse(dates[1]);
        }
    }
}
