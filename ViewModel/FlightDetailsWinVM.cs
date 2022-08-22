﻿using BL;
using BO;
using BO.Flights;
using FlightsMap.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsMap.ViewModel
{
    internal class FlightDetailsWinVM 
    {
        public WinFlightDetails WFD { get; set; }
        BLImp bl = new BLImp();
        public HelperClass helper; 

        public FlightInfoPartial FlightPartial { get; set; }
        public FlightDetail Flight { get; set; }

        public FlightDetailsWinVM(FlightInfoPartial fip)
        {
            FlightPartial = fip;
            Flight = bl.GetFlightDetail(FlightPartial.SourceId);
            helper = new HelperClass();
        }
        public string FlightNumber
        {
            get
            {

                var flightnum = Flight.identification.number.Default;
                if (Flight.identification.number.alternative != null) 
                    flightnum += " / " + Flight.identification.number.alternative;
                return flightnum.ToString();
            }
        }
        public string AirlineCompany
        {
            get
            {
                var airline = Flight.airline.name;
                return airline;
            }
        }
        public string Source
        {
            get
            {
                return FlightPartial.Source;
            }
        }
        public string Destination
        {
            get
            {
                return FlightPartial.Destination;
            }
        }
        public string SourceName
        {
            get
            {
                return Flight.airport.origin.name;
            }
        }
        public string DestinationName
        {
            get
            {
                return Flight.airport.destination.name;
            }
        }
        public string SDest
        {
            get
            {
                try
                {
                    int utctime = Flight.time.scheduled.arrival;
                    int offset = Flight.airport.destination.timezone.offset;
                    int total = utctime + offset;
                    return helper.GetDateTimeFromEpoch(total).ToString("HH:mm");

                }
                catch(Exception ex)
                {
                    return "N/A";
                }
                   
            }
        }
        public string SSource
        {
            get
            {
                try
                {
                    int utctime = Flight.time.scheduled.departure;
                    int offset = Flight.airport.origin.timezone.offset;
                    int total = utctime + offset;
                    return helper.GetDateTimeFromEpoch(total).ToString("HH:mm");

                }
                catch (Exception)
                {
                    return "N/A";
                }
            }
        }
        public string Act
        {
            get
            {
                if (Flight.time.real.departure != null) { 
                    long utctime = (long)Flight.time.real.departure;
                    long offset = Flight.airport.origin.timezone.offset;
                    long total = utctime + offset;
                    return helper.GetDateTimeFromEpoch(total).ToString("HH:mm");
                }
                return SDest;
            }
        }
        public string Est
        {
            get
            {
                if (Flight.time.estimated.arrival != null)
                {
                    long utctime = (long)Flight.time.estimated.arrival;
                    long offset = Flight.airport.destination.timezone.offset;
                    long total = utctime + offset;
                    return helper.GetDateTimeFromEpoch(total).ToString("HH:mm");

                }
                return helper.GetDateTimeFromEpoch(Flight.time.scheduled.arrival).ToString("HH:mm");
            }
        }
        public string StatusAirplane
        {
            get
            {
                switch(Flight.status.generic.status.text)
                {
                    case "scheduled":
                        return "scheduled.png";
                    case "landed":
                        return "land.png";
                    case "estimated":
                        return "estimated.png";
                    case "delayed":
                        return "delayed.png";

                }
                return "takeoff.png";
            }
        }
        public string FlightStatus
        {
            get
            {
                return Flight.status.text;
            }
        }

        public string Stime
        {
            get
            {
                try
                {
                    return helper.GetDateTimeFromEpoch(Flight.status.generic.eventTime.utc).ToString("HH:mm") + " UTC";
                }
                catch(Exception e)
                {
                    return "N/A";
                }
            }
        }
        public string Dtime
        {
            get
            {
                try
                {
                    return helper.GetDateTimeFromEpoch(Flight.status.generic.eventTime.local).ToString("HH:mm") + " UTC";
                }
                catch (Exception e)
                {
                    return "N/A";
                }
            }
        }
        public int PBValue
        {
            get
            {
                return bl.GetProp(Flight, FlightPartial);
            }
        }
        public string PBStatus
        {
            get
            {
                return bl.GetRemainingDistance(Flight,FlightPartial).ToString("F1")+" km in "+bl.GetStringRemainingTime(Flight);
            }
        }

        public string WeatherOrigin
        {
            get
            {
                var result = bl.GetWeather(Flight, FlightPartial);
                //This function returns a Dictionary of dictionaries.

                // there are 3 locations: current, origin and destination
                // for each location there are : temperature, main and shortDesc

                // For example, to have the temperature of the destination 
                // it is :
                return result["origin"]["shortDesc"].ToUpper() + " "+result["origin"]["temperature"] + " °C";
            }
        }
        public string WeatherDest
        {
            get
            {
                var result = bl.GetWeather(Flight, FlightPartial);
                return result["destination"]["shortDesc"].ToUpper() + " "+result["destination"]["temperature"] + " C°";
            }
        }
        public string WeatherCurrent
        {
            get
            {
                var result = bl.GetWeather(Flight, FlightPartial);
                return result["current"]["shortDesc"].ToUpper() + " "+result["current"]["temperature"]+ " C°";
            }
        }

    }
}
