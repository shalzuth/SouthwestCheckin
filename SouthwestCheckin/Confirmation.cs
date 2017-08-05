using System;
using System.Linq;

namespace SouthwestCheckin
{
    class Confirmation
    {
        public String ConfirmationNumber;
        public String FirstName;
        public String LastName;
        String Group = "";
        String Position = "";
        public String BoardingGroupPosition { get { return Group + Position; } }
        public DateTime FlightTime;
        public String FlightNumber;
        public String Departure;
        public String Arrival;

        String CheckinUrl = "https://www.southwest.com/flight/retrieveCheckinDoc.html";
        String BoardingPassUrl = "https://www.southwest.com/flight/selectPrintDocument.html";
        String ReservationUrl = "https://www.southwest.com/flight/view-air-reservation.html";
        Networking Networking = new Networking();
        public DateTime GetFlightTime()
        {
            if (FlightTime != null)
                return FlightTime;
            var reservation = Networking.PostData(ReservationUrl, $"confirmationNumberFirstName={FirstName}&confirmationNumberLastName={LastName}&confirmationNumber={ConfirmationNumber}&submitButton=");
            var flightInfo = reservation.Substring(reservation.IndexOf("/flight/flight-notification-subscribe.html?fn=") + 46);
            FlightNumber = flightInfo.Substring(0, flightInfo.IndexOf("&dac="));
            Departure = flightInfo.Substring(flightInfo.IndexOf("&dac=") + 5, 3);
            Arrival = flightInfo.Substring(flightInfo.IndexOf("&aac=") + 5, 3);
            var mmddYYYY = flightInfo.Substring(flightInfo.IndexOf("&dd=") + 4, 10);
            var time = reservation.Substring(reservation.IndexOf("<span class=\"nowrap\">") + 21);
            time = time.Substring(0, time.IndexOf("</span>"));
            FlightTime = DateTime.Parse(mmddYYYY + " " + time);
            return FlightTime;
        }
        public Boolean CheckIn()
        {
            var tryCheckIn = Networking.GetData(CheckinUrl + $"?confirmationNumber={ConfirmationNumber}&firstName={FirstName}&lastName={LastName}");
            if (tryCheckIn.Contains("We were unable to retrieve your reservation from our database"))
                return false;
            var boardingPass = Networking.PostData(BoardingPassUrl, "checkinPassengers%5B0%5D.selected=true&printDocuments=Check+In");
            if (!boardingPass.Contains("boarding_group"))
                return false;
            Group = boardingPass.Substring(boardingPass.IndexOf("<td class=\"boarding_group\"><span class=\"boardingInfo\">") + 54, 1);
            Position = boardingPass.Substring(boardingPass.IndexOf("<td class=\"boarding_position\"><span class=\"boardingInfo\">") + 57, 2);
            return true;
        }
    }
}
