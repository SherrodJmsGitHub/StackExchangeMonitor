/* 
* @Author: CLS
* @Date:   24-06-2014 21:56:11
* @Last Modified by:   CLS
* @Last Modified time: 26-06-2014 19:35:06
*/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace StackExchangeMonitor {

	public partial class MainWindow : Window {

	//reference epoc time because api calls referece unix time
	static System.DateTime unix_epoc = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);


	static void QuestionsQuery( object obj ) {
		Dictionary<Key, object> hash=(Dictionary<Key, object>)obj;

		string api=hash[Key.Api] as string;
		
		//post query for recent questions
		//count views of questions from last hour
    	SiteTile[] all_sites=hash[Key.AllSites] as SiteTile[];

		//sort the sites based on view count
		string query;
		int seconds_to_mills=1000;
		int pause_mills=2000;

		do{
		//time from 1 hour ago
			int t0=(int)(DateTime.Now.ToUniversalTime()-unix_epoc).TotalSeconds;
			t0-=3600;
			QuestionResponse question_response=new QuestionResponse();
			
			foreach(var site in all_sites){
				query=String.Format("{0}questions?order=desc&sort=activity&site={1}", api, site.api_site_parameter);
				if(run==false || question_response.backoff>0) break;
				
				SpinWait(new TimeSpan(0,0,pause_mills/seconds_to_mills));
	        	
	        	HttpWebResponse response=PostRequest(query);
	        	if(response != null){
		        	using (response){
		            	if (response.StatusCode != HttpStatusCode.OK)
		                throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
		                response.StatusCode,
		                response.StatusDescription));

						question_response=
						ResponseDecoder.Decode(response, StackExchange.Question) as QuestionResponse;

			            site.top_title=question_response.questions[0].title;
			            site.num_questions=0;
			            
			            foreach(var question in question_response.questions){
			            	if(question.creation_date > t0) site.num_questions++;
			            } 

						hash[Key.QuotaRemaining]=question_response.quota_remaining.ToString();

						hash[Key.Backoff]=question_response.backoff; 


		        	}

						

	        	}else{
	        		hash[Key.QuotaRemaining]="Server error";
	        	}
			}

			if(question_response.backoff > 0)
				pause_mills=question_response.backoff*seconds_to_mills;
			else
				pause_mills=2000;  

			(hash[Key.View] as MainWindow).Dispatcher.Invoke(update_action, hash);

			(hash[Key.WaitUpdate] as AutoResetEvent ).WaitOne();

			SpinWait(new TimeSpan(0,1,0));
			
		}while(run);

		Thread.Sleep(seconds_to_mills/2);
		(hash[Key.WaitClose] as AutoResetEvent).Set();
	}

	static void SpinWait(TimeSpan tspan){
			DateTime now=DateTime.Now;
			while(DateTime.Now-now<tspan && run)Thread.Sleep(250);
	}



/*class*/}
/*namespace*/}
