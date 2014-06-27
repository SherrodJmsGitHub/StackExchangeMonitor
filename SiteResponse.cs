/* 
* @Author: CLS
* @Date:   07-06-2014 04:47:43
* @Last Modified by:   CLS
* @Last Modified time: 26-06-2014 18:28:02
*/

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StackExchangeMonitor {
[DataContract]
public class Styling{

    [DataMember]
    public string tag_background_color { get; set; }
    [DataMember]
    public string tag_foreground_color { get; set; }
    [DataMember]
    public string link_color { get; set; }
}


[DataContract]
public class RelatedSite
{
    [DataMember]
    public string relation { get; set; }
    [DataMember]
    public string api_site_parameter { get; set; }
    [DataMember]
    public string site_url { get; set; }
    [DataMember]
    public string name { get; set; }
}


[DataContract]
public class Site
{
    [DataMember]
    public List<string> aliases { get; set; }
    [DataMember]
    public Styling styling { get; set; }
    [DataMember]
    public List<RelatedSite> related_sites { get; set; }
    [DataMember]
    public List<string> markdown_extensions { get; set; }
    [DataMember]
    public int launch_date { get; set; }
    [DataMember]
    public string site_state { get; set; }
    [DataMember]
    public string high_resolution_icon_url { get; set; }
    [DataMember]
    public string favicon_url { get; set; }
    [DataMember]
    public string icon_url { get; set; }
    [DataMember]
    public string audience { get; set; }
    [DataMember]
    public string site_url { get; set; }
    [DataMember]
    public string api_site_parameter { get; set; }
    [DataMember]
    public string logo_url { get; set; }
    [DataMember]
    public string name { get; set; }
    [DataMember]
    public string site_type { get; set; }
    [DataMember]
    public string twitter_account { get; set; }
    [DataMember]
    public int? open_beta_date { get; set; }
    [DataMember]
    public int? closed_beta_date { get; set; }
}

[DataContract]
public class SiteResponse:StackExchangeResponse{
    [DataMember(Name="items")]
    public List<Site> sites { get; set; }
}
    
}
