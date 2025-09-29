namespace API.Models
{
    public class LOLServerStatusDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> locales { get; set; }
        public List<object> maintenances { get; set; }
        public List<Incident> incidents { get; set; }
    }
    public class Incident
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public object archive_at { get; set; }
        public List<Title> titles { get; set; }
        public List<Update> updates { get; set; }
        public List<string> platforms { get; set; }
        public object maintenance_status { get; set; }
        public string incident_severity { get; set; }
    }
    public class Title
    {
        public string locale { get; set; }
        public string content { get; set; }
    }
    public class Translation
    {
        public string locale { get; set; }
        public string content { get; set; }
    }
    public class Update
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool publish { get; set; }
        public string author { get; set; }
        public List<Translation> translations { get; set; }
        public List<string> publish_locations { get; set; }
    }
}
