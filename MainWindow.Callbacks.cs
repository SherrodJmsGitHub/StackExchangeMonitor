/* 
* @Author: CLS
* @Date:   25-06-2014 15:24:00
* @Last Modified by:   CLS
* @Last Modified time: 26-06-2014 19:25:12
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
	static void Swap(SiteTile[] sites, int a, int b){
		var temp=sites[b];
		sites[b]=sites[a];
		sites[a]=temp;
	return;}

	//sort from most to least questions
	static void QuickSort(SiteTile[] sites, int a, int p, int b){

		var piv_site=sites[p];

		for(int i=a; i<=p; i++){
			if( sites[i].num_questions < sites[p].num_questions ){
				int _p_=p;
				for(int j=p; j<=b;j++){
					if(sites[j].num_questions > sites[p].num_questions)
					_p_=j;
				}

				Swap(sites, i, _p_);
				//if the pot is swapped restart
				if(_p_==p)
				QuickSort(sites, a, p, b);
			}
		}

		for(int i=p; i<=b; i++){
			if( sites[i].num_questions > sites[p].num_questions ){
				int _p_=p;
				for(int j=a; j<=p;j++){
					if(sites[j].num_questions < sites[p].num_questions)
					_p_=j;
				}

				Swap(sites, i, _p_);
				//if the pot is swapped restart
				if(_p_==p)
				QuickSort(sites, a, p, b);
			}
		}


		if(b-p > 1) QuickSort(sites, p, p+(b-p)/2, b);

		if(p-a > 1) QuickSort(sites, a, a+(p-a)/2, p);
	return;}


	void Update(Dictionary<Key, object> hash){
		//sort sites
		var all_sites=hash[Key.AllSites] as SiteTile[];

		var top_sites=hash[Key.TopSites] as IList<SiteTile>;

		QuickSort(all_sites, 0, all_sites.Length/2, all_sites.Length-1);

		top_sites.Clear();

		for(int i=0; i<10; i++){
			top_sites.Add(all_sites[i]);
		}

		int backoff=(int)hash[Key.Backoff];
		if( backoff == 0){
			quota_txt.Text= hash[Key.QuotaRemaining] as string;
			quota_lbl.Content="Quota";			
		}else{
			quota_txt.Text=((int)hash[Key.Backoff]).ToString();
			quota_lbl.Content="Throttle Time";
		}

		(hash[Key.WaitUpdate] as AutoResetEvent ).Set();
	return;}
}
}
