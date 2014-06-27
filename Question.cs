/* 
* @Author: CLS
* @Date:   24-06-2014 19:39:13
* @Last Modified by:   CLS
* @Last Modified time: 26-06-2014 14:18:17
*/
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace StackExchangeMonitor {
[DataContract]	
public class Question{
    [DataMember] public List<string> tags { get; set; }
    [DataMember] public Owner owner { get; set; }
    [DataMember] public bool is_answered { get; set; }
    [DataMember] public int view_count { get; set; }
    [DataMember] public int favorite_count { get; set; }
    [DataMember] public int down_vote_count { get; set; }
    [DataMember] public int up_vote_count { get; set; }
    [DataMember] public int answer_count { get; set; }
    [DataMember] public int score { get; set; }
    [DataMember] public int last_activity_date { get; set; }
    [DataMember] public int creation_date { get; set; }
    [DataMember] public int last_edit_date { get; set; }
    [DataMember] public int question_id { get; set; }
    [DataMember] public string link { get; set; }
    [DataMember] public string title { get; set; }
    [DataMember] public string body { get; set; }
}

[DataContract]
public class QuestionResponse:StackExchangeResponse{
	[DataMember(Name="items")]
    public List<Question> questions { get; set; }
}
}
