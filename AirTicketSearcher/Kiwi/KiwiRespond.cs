namespace AirTicketSearcher.Kiwi
{
    using System.Collections;
    using System.Collections.Generic;

    public class KiwiRespond
    {
        public List<object> ref_tasks { get; set; }
        public SearchParams search_params { get; set; }
        public double currency_rate { get; set; }
        public List<object> refresh { get; set; }
        public List<object> connections { get; set; }
        public string currency { get; set; }
        public double del { get; set; }
        public List<object> all_airlines { get; set; }
        public int time { get; set; }
        public List<object> all_stopover_airports { get; set; }
        public List<Data> data { get; set; }
        public string search_id { get; set; }
    }   
    public class Seats
        {
        public int infants { get; set; }
        public int passengers { get; set; }
        public int adults { get; set; }
        public int children { get; set; }
    }

    public class SearchParams
    {
        public string to_type { get; set; }
        public string flyFrom_type { get; set; }
        public Seats seats { get; set; }
    }

    public class Baglimit
    {
        public int hand_width { get; set; }
        public int hold_weight { get; set; }
        public int hand_weight { get; set; }
        public int hand_height { get; set; }
        public int hand_length { get; set; }
    }

    public class BagsPrice
    {
        public double __invalid_name__1 { get; set; }
    }

    public class Duration
    {
        public int total { get; set; }
        public int @return { get; set; }
        public int departure { get; set; }
    }

    public class Conversion
    {
        public int CZK { get; set; }
        public int EUR { get; set; }
    }

    public class CountryTo
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Route
    {
        public int aTimeUTC { get; set; }
        public int refresh_timestamp { get; set; }
        public bool bags_recheck_required { get; set; }
        public int @return { get; set; }
        public double latTo { get; set; }
        public int flight_no { get; set; }
        public int price { get; set; }
        public int original_return { get; set; }
        public string operating_carrier { get; set; }
        public string cityTo { get; set; }
        public string mapIdfrom { get; set; }
        public double lngFrom { get; set; }
        public string vehicle_type { get; set; }
        public string flyFrom { get; set; }
        public string id { get; set; }
        public int dTimeUTC { get; set; }
        public string equipment { get; set; }
        public string mapIdto { get; set; }
        public string combination_id { get; set; }
        public int dTime { get; set; }
        public string fare_family { get; set; }
        public string found_on { get; set; }
        public string flyTo { get; set; }
        public string source { get; set; }
        public double latFrom { get; set; }
        public string airline { get; set; }
        public string fare_classes { get; set; }
        public double lngTo { get; set; }
        public string cityFrom { get; set; }
        public int last_seen { get; set; }
        public int aTime { get; set; }
        public bool guarantee { get; set; }
        public string fare_basis { get; set; }
    }

    public class Hold
    {
        public double weight { get; set; }
        public double price { get; set; }
        public int dimensions_sum { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public string tier { get; set; }
    }

    public class Hand
    {
        public int width { get; set; }
        public double price { get; set; }
        public int length { get; set; }
        public double weight { get; set; }
        public int height { get; set; }
    }

    public class PersonalItem
{
    public int width { get; set; }
    public double price { get; set; }
    public int length { get; set; }
    public double weight { get; set; }
    public int height { get; set; }
}

    public class Baggage
    {
 public List<Hold> hold { get; set; }
    public Hand hand { get; set; }
    public PersonalItem personal_item { get; set; }
    }

    public class CountryFrom
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Data
    {
        public string return_duration { get; set; }
        public double quality { get; set; }
        public string flyTo { get; set; }
        public string deep_link { get; set; }
        public string mapIdto { get; set; }
        public int nightsInDest { get; set; }
        public List<string> airlines { get; set; }
        public int pnr_count { get; set; }
        public string fly_duration { get; set; }
        public Baglimit baglimit { get; set; }
        public bool has_airport_change { get; set; }
        public double distance { get; set; }
        public List<string> type_flights { get; set; }
        public BagsPrice bags_price { get; set; }
        public string flyFrom { get; set; }
        public int dTimeUTC { get; set; }
        public int p2 { get; set; }
        public int p3 { get; set; }
        public int p1 { get; set; }
        public int dTime { get; set; }
        public List<string> found_on { get; set; }
        public string booking_token { get; set; }
        public string cityFrom { get; set; }
        public string mapIdfrom { get; set; }
        public Duration duration { get; set; }
        public string id { get; set; }
        public Conversion conversion { get; set; }
        public CountryTo countryTo { get; set; }
        public int aTimeUTC { get; set; }
        public int price { get; set; }
        public List<List<string>> routes { get; set; }
        public string cityTo { get; set; }
        public List<object> transfers { get; set; }
        public List<Route> route { get; set; }
        public Baggage baggage { get; set; }
        public bool facilitated_booking_available { get; set; }
        public int aTime { get; set; }
        public CountryFrom countryFrom { get; set; }
    }


}