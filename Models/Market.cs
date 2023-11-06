using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CryptoPulse.Areas.Identity.Data;
using Newtonsoft.Json;

namespace CryptoPulse.Models
{
    public class MarketsViewModel
    {
        public List<Coin> Coins { get; set; }
        public List<Market> Markets { get; set; }
    }

    public class Market
    {
        [Key]
        public int ID { get; set; }

        public int coinId { get; set; }
        public Coin Coin { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("quote")]
        public string Quote { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("price_usd")]
        public decimal PriceUSD { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("volume_usd")]
        public decimal VolumeUSD { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }
}
