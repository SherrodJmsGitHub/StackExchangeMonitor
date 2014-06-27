using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace StackExchangeMonitor {

[Serializable]
public abstract class Response{}



public enum StackExchange{
	Question,
	Sites
}

[DataContract]
public abstract class StackExchangeResponse:Response{

	//nullable fields that may be absent
	[DataMember] public int backoff;
	[DataMember] public int error_id;
	[DataMember] public string error_message;
	[DataMember] public string error_name;

	[DataMember] public bool has_more;
	[DataMember] public int page;
	[DataMember] public int page_size;
	[DataMember] public int quota_max;
	[DataMember] public int quota_remaining;
	[DataMember] public int total;
	[DataMember] public string type;


}


public static class ResponseDecoder{

	static Dictionary<Enum, Type> ResponseSelect; 
	
	static ResponseDecoder(){
		ResponseSelect= new Dictionary<Enum, Type>();
		ResponseSelect[StackExchange.Question]=typeof(QuestionResponse);
		ResponseSelect[StackExchange.Sites]=typeof(SiteResponse);
	return;}

	public static Response Decode(WebResponse web_response, Enum api_enum){

		DataContractJsonSerializer serializer=new DataContractJsonSerializer(ResponseSelect[api_enum]);
        
        Response response=serializer.ReadObject(web_response.GetResponseStream()) as Response;
	return response;}

}
}
