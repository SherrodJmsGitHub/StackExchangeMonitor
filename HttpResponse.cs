/* 
* @Author: CLS
* @Date:   26-06-2014 14:19:27
* @Last Modified by:   CLS
* @Last Modified time: 26-06-2014 16:38:02
*/

using System;
using System.Net;
using System.Windows;

namespace StackExchangeMonitor {


public partial class MainWindow : Window {

	public static HttpWebResponse PostRequest(string query){
        HttpWebRequest request = WebRequest.Create(query) as HttpWebRequest;
        request.AutomaticDecompression = DecompressionMethods.GZip;

    	HttpWebResponse response=null;

    	try{
    		response=request.GetResponse() as HttpWebResponse;
		}catch(Exception e){

			//For this type of app this will mostliky be a timeout from throttiling of the api on the server end.
			if(e is WebException){
				//need figureout a way to set a default response that is not null
				var wex=e as WebException;
				response=wex.Response as HttpWebResponse;
			}else{
				throw e;
			}

		}

	return response;}
}
}
