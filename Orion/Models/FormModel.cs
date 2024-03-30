using Newtonsoft.Json;

namespace Orion.Models
{
    public class FormModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Area { get; set; }
        public string Company { get; set; }
        public string Industry { get; set; }
        public string Size { get; set; }
        [JsonProperty("ValueProp_input")]
        public string ValuePropInput { get; set; }
        public string Product { get; set; }
        [JsonProperty("My_Name")]
        public string MyName { get; set; }
        [JsonProperty("My_Title")]
        public string MyTitle { get; set; }
        [JsonProperty("My_Company")]
        public string MyCompany { get; set; }

    }
}
