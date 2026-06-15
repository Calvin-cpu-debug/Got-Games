using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace Got_Games
{
    internal class Program
    {
        public class Data1
        {
            public Data2 response { get; set; }
        }
        public class Data2
        {
            public int game_count { get; set; }
            public List<Data3> games { get; set; }
        }
        public class Data3
        {
            public string name { get; set; }
            public int appid { get; set; }
            public int playtime_forever { get; set; }
        }

        static async Task Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var jsonOnion = await httpClient.GetStringAsync("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=B00C0473160B519FDEE434DE69F2C7A5&steamid=76561198053123082&format=json&include_appinfo=true");


                var OnionData = JsonSerializer.Deserialize<Data1>(jsonOnion);

                Console.WriteLine();
                Console.WriteLine("Onions Data (Json Deserialization attempts)");
                Console.WriteLine();

                Console.WriteLine($"name : {OnionData.response.games[0].name}");
                Console.WriteLine($"appid : {OnionData.response.games[0].appid}");
                Console.WriteLine($"games : {string.Join(",", OnionData.response.games.Select(x => x.name).ToList())}");
                //Console.WriteLine($"playtime_forever : {OnionData.response.games[0].playtime_forever}");
                // var Data = JsonSerializer.Deserialize<Data1>(jsonHitbox);



                string jsonHitbox = await httpClient.GetStringAsync("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=B00C0473160B519FDEE434DE69F2C7A5&steamid=76561198833577564&format=json&include_appinfo=true");


                var HitboxData = JsonSerializer.Deserialize<Data1>(jsonHitbox);

                Console.WriteLine();
                Console.WriteLine("Hitboxes Data (Json Deserialization attempts)");
                Console.WriteLine();

                Console.WriteLine($"name : {HitboxData.response.games[0].name} Hello");
                Console.WriteLine($"appid : {HitboxData.response.games[0].appid}");
                Console.WriteLine($"games : {string.Join(",", HitboxData.response.games.Select(x => x.name).ToList())}");
                // Finding a count of all the games containing the words "the"

                Console.WriteLine();
                Console.WriteLine("Hitbox Games containing the Word 'The' (LINQ practice)");
                Console.WriteLine();

                IEnumerable<Data3> gamescontainingthe = HitboxData.response.games.Where(x => x.name.Contains("The"));

                int GamesCount = gamescontainingthe.Count();
                List<Data3> GamesContainingTheList = gamescontainingthe.ToList();
                Console.WriteLine($"games containing the : {string.Join(",", GamesContainingTheList.Select(x => x.name).ToList())}");

                Console.WriteLine();
                Console.WriteLine("Onion Game Time Manipulation (LINQ practice)");
                Console.WriteLine();

                IEnumerable<Data3> gametimeHundred = OnionData.response.games.Where(x => x.playtime_forever > 6000);

                int GamesHundred = gametimeHundred.Count();
                List<Data3> PlaytimeHundred = gametimeHundred.ToList();
                Console.WriteLine($"All Games over 100 hours : {string.Join("'", PlaytimeHundred.Select(x => x.playtime_forever).ToList())}");
                int PlaytimeGamesOverOneHundred = PlaytimeHundred.Sum(x => x.playtime_forever);
                Console.WriteLine(PlaytimeGamesOverOneHundred);

                Console.WriteLine();
                Console.WriteLine("Hitbox Game Time Manipulation (LINQ practice (orderby) and Linking multiple where statements  )");
                Console.WriteLine();

                IEnumerable<Data3> SpecifiedTimePlayed = HitboxData.response.games.Where(x => x.playtime_forever > 600).Where(x => x.playtime_forever < 6000);

                List<Data3> GametimeInParameters = SpecifiedTimePlayed.ToList();
                //Console.WriteLine($"Games For Hitbox Between 10 hours and 100 hours played : {string.Join(",", GametimeInParameters.Select(x => $"{x.name + " " + x.playtime_forever}" ).ToList())}");
                Console.WriteLine($"Games For Hitbox Between 10 hours and 100 hours played : {string.Join(",", GametimeInParameters.OrderBy(x => x.playtime_forever).Select(x => $"{x.name} ({x.playtime_forever})").ToList())}");

                Console.WriteLine();
                Console.WriteLine("Hitbox Data for Appid ordered ascending (LINQ practice)");
                Console.WriteLine();


                //List<Data3> AppIdHitbox = 

                IEnumerable<int> AppIdListingHitbox = HitboxData.response.games.Select(x => x.appid).ToList();
                Console.WriteLine($"Hitbox Games App Id's : {string.Join(",", AppIdListingHitbox.OrderBy(x => x))}");
                Console.WriteLine();
                IEnumerable<int> AppIdListingOnion = OnionData.response.games.Select(x => x.appid).ToList();
                Console.WriteLine($"Onion Games App Id's : {string.Join(",", AppIdListingOnion.OrderBy(x => x))}");


                // Golden Assisted Work for Comparison of Both Json File Information 

                Console.WriteLine();
                Console.WriteLine("Games Hitbox Owns In Common with Onion :");
                Console.WriteLine();

                List<Data3> SharedGamesBothOwn = new List<Data3>();

                foreach (Data3 HitboxOwnGame in HitboxData.response.games)
                {
                    if (AppIdListingOnion.Contains(HitboxOwnGame.appid))
                    {
                        SharedGamesBothOwn.Add(HitboxOwnGame);
                        Console.WriteLine(HitboxOwnGame.name);
                    }

                }


                //List<Data3> SharedGamesBothOwn = new List<Data3>();

                //foreach (Data3 HitboxOwnGame in HitboxData.response.games)
                //{
                //    if (AppIdListingOnion.Contains(HitboxOwnGame.appid))
                //    {
                //        SharedGamesBothOwn.Add(HitboxOwnGame);
                //        Console.WriteLine(HitboxOwnGame.name);
                //    }

                //}



                //List<int> SharedAppIDs = new List<int>();

                // foreach (int AppId in AppIdListingHitbox)
                // {
                //  if (AppIdListingOnion.Contains(AppId))
                //  {
                //       SharedAppIDs.Add(AppId);
                //Console.WriteLine($" {AppId} ");
                //  }
                // }

                //  Console.WriteLine();

                //IEnumerable<Data3> GamesHitboxOwns = HitboxData.response.games.Where(x => x.name);

                //IEnumerable<Data3> GameNamesShared = HitboxData.response.games.Select(x => x.name).Where(x => x.)

                //IEnumerable<int> AppIdResult = HitboxData.response.games.Select(x => x.appid.AppIdListingOnion.ToString).ToList();
            }
            // Get a list where you have played between 10 and 100 hours  For the HitboxData

            // IEnumerable<string> gamesQuery =
            //    from games in names
            //   where games == true
            //  select games;

            // foreach (var name in gamesQuery)
            // {
            //     Console.WriteLine(gamesQuery);
            // }
            // IEnumerable<List<Data3>> scoreQuery =
            // from score in Data
            // where score >  1
            // select score;


        }
    }
}

