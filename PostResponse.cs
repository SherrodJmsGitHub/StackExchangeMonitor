using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StackExchangeMonitor {
[DataContract]
public class Owner{
    [DataMember]
    public int reputation { get; set; }
    [DataMember]
    public int user_id { get; set; }
    [DataMember]
    public string user_type { get; set; }
    [DataMember]
    public int accept_rate { get; set; }
    [DataMember]
    public string profile_image { get; set; }
    [DataMember]
    public string display_name { get; set; }
    [DataMember]
    public string link { get; set; }
}

[DataContract]
    public class PostItem{
    [DataMember]
    public Owner owner { get; set; }
    [DataMember]
    public int score { get; set; }
    [DataMember]
    public int last_activity_date { get; set; }
    [DataMember]
    public int creation_date { get; set; }
    [DataMember]
    public string post_type { get; set; }
    [DataMember]
    public int post_id { get; set; }
    [DataMember]
    public string link { get; set; }
    [DataMember]
    public int? last_edit_date { get; set; }
}

[DataContract]
public class PostResponse{
    [DataMember(Name="items")]
    public List<PostItem> Posts { get; set; }
    [DataMember]
    public bool has_more { get; set; }
    [DataMember]
    public int backoff { get; set; }
    [DataMember]
    public int quota_max { get; set; }
    [DataMember]
    public int quota_remaining { get; set; }
}
}