namespace AirTicketSearcher.TransportLayer
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    public class Transport
    {
        //string ADDRESS = "https://api.skypicker.com/flights?fly_from=CZ&fly_to=TYO&date_from=10/10/2019&date_to=27/10/2019&nights_in_dst_from=2&nights_in_dst_to=14&return_from=10/10/2019&return_to=30/10/2019&flight_type=round&adults=1&children=0&infants=0&fly_days=[0,1,2,3,4,5,6]&fly_days_type=departure&ret_fly_days=[0,1,2,3,4,5,6]&ret_fly_days_type=departure&one_for_city=0&only_working_days=0&only_weekends=0&one_per_date=0&direct_flights=0&locale=en&partner=picky&partner_market=us&v=3&curr=czk&price_from=1&price_to=20000&max_stopovers=1&sort=price&asc=1";
        string ADDRESS = "https://api.skypicker.com/flights?flyFrom=PRG&to=TYO&dateFrom=20/4/2019&dateTo=4/5/2019&partner=picky&curr=czk";
        public Transport()
        {
        }

        public async  void GetDataFromWeb(string url)
        {
            
            // HttpClient client = new HttpClient();
            // var task =  client.GetStringAsync("https://api.skypicker.com/flights?fly_from=CZ&fly_to=TYO&date_from=10/10/2019&date_to=27/10/2019&nights_in_dst_from=2&nights_in_dst_to=14&return_from=10/10/2019&return_to=30/10/2019&flight_type=round&adults=1&children=0&infants=0&fly_days=[0,1,2,3,4,5,6]&fly_days_type=departure&ret_fly_days=[0,1,2,3,4,5,6]&ret_fly_days_type=departure&one_for_city=0&only_working_days=0&only_weekends=0&one_per_date=0&direct_flights=0&locale=en&partner=picky&partner_market=us&v=3&curr=czk&price_from=1&price_to=20000&max_stopovers=1&sort=price&asc=1");
            // Console.WriteLine( task );
            
            // WORKS ------
            // var client = new HttpClient();
            // Task<string> task = client.GetStringAsync(ADDRESS);
            // Console.WriteLine(await task);



            // WORKS ---------
            WebClient client = new WebClient();
            string result = client.DownloadString(ADDRESS);
            //result = result.Replace("\\\"","");
            Console.WriteLine(result);

            
        }

    }
}