﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace WofHCalc.Models.jsonTemplates
{
    public class Unit
    {                
        //[J("capacity")] public long Capacity { get; set; }
        [J("cost")] public int[] ResCost { get; set; }                        
        [J("popcost")] public long PopCost { get; set; }                
        //[J("tags")] public long Tags { get; set; }
        [J("traintime")] public long TrainTime { get; set; }
        [JsonProperty("pay")]
        public int[][] Pay { get; set; }
        public static Unit[] FromJson(string json) => JsonConvert.DeserializeObject<Unit[]>(json, Converter.Settings);
    }    
}
