using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace SouthwestCheckin
{
    class RapidRewards
    {
        public String PreferredName { get; set; }
        String RapidRewardsUrl = "https://www.southwest.com/flight/login?loginEntryPoint=GLOBAL_NAV_LOGIN";
        String UpcomingTripsUrl = "https://www.southwest.com/flight/apiSecure/upcoming-trips/account-view/summary";
        Networking Network = new Networking();
        String RapidRewardsNumber;
        String Password;
        public List<Confirmation> Flights;
        public RapidRewards(String rapidRewardsNumber, String password)
        {
            RapidRewardsNumber = rapidRewardsNumber;
            Password = password;
        }
        public void Login()
        {
            Network.GetData(RapidRewardsUrl);
            var login = Network.PostData(RapidRewardsUrl, $"credential={RapidRewardsNumber}&password={Password}&returnUrl=%2F");
            var preferredName = login.Substring(login.IndexOf("<!-- mp_trans_disable_start -->") + 31);
            PreferredName = preferredName.Substring(0, preferredName.IndexOf("<!-- mp_trans_disable_end -->"));
        }
        public List<Confirmation> GetFlights()
        {
            Flights = new List<Confirmation>();
            var flights = Network.GetData(UpcomingTripsUrl);
            var flightsJson = new JavaScriptSerializer().Deserialize<dynamic>(flights);
            var trips = flightsJson["trips"];
            var numtrips = trips.Length;
            for (int i = 0; i < numtrips; i++)
            {
                var trip = trips[i];
                var data = trip["products"][0];
                Flights.Add(new Confirmation
                {
                    ConfirmationNumber = data["confirmationNumbers"][0],
                    FirstName = data["firstName"],
                    LastName = data["lastName"],
                    Departure = data["originAirportCode"],
                    Arrival = data["destinationAirportCode"],
                    FlightTime = DateTime.Parse(data["departureDateTime"])
                });
            }
            return Flights;
        }
    }
}
