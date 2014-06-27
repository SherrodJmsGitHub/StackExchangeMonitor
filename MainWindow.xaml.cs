using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

public enum Key{
    Api,
    TopSites,
    AllSites,
    WaitClose,
    WaitUpdate,
    View,
    QuotaRemaining,
    Backoff
}


public partial class MainWindow : Window {
    List<string> names=new List<string>();
    SiteTile[] all_sites=null;

    ObservableCollection<SiteTile> top_sites= new ObservableCollection<SiteTile>();

    string api="http://api.stackexchange.com/2.2/";
    string sitesurl="sites";
    string image_archive=@"C:\Users\CLS\Documents\CodeLibrary\openapps\StackExchangeMonitor\StackExchangeMonitor";

    static bool run=true;

    Dictionary<Key, object> arg_hash = new Dictionary<Key, object>();
	static Action<Dictionary<Key, object>> update_action;

	public MainWindow() {InitializeComponent();

        Title="StackExchangeMonitor";

	//during intilization find the current stackexchange sites.
        HttpWebResponse response=PostRequest(api+sitesurl);

        if( response != null ){
            using (response){


                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

               SiteResponse sites_response=ResponseDecoder.Decode(response, StackExchange.Sites) as SiteResponse;

                all_sites = new SiteTile[sites_response.sites.Count];
                
                for(int i=0; i<all_sites.Length; i++){
                    Site site = sites_response.sites[i];

                    string image=String.Format("{0}\\img\\{1}.ico", image_archive, site.name.ToLower());
                    
                    all_sites[i]=SiteTile.CreateFrom(site)
                                    .WithFaviconImage(image);

                    DownloadFavicon(site.favicon_url, image);

                    if(i<10)
                    top_sites.Add(all_sites[i]);
                }

                //add the top ten sites to view
                SiteTiles.ItemsSource=top_sites;

                number_sites_txt.Text=all_sites.Length.ToString();
                quota_txt.Text=sites_response.quota_remaining.ToString();
            }
            
            arg_hash[Key.Api]=api;
    		arg_hash[Key.AllSites]=all_sites;
    		arg_hash[Key.TopSites]=top_sites;

    		arg_hash[Key.WaitClose]=new AutoResetEvent(false);
            arg_hash[Key.WaitUpdate]=new AutoResetEvent(false);
            
            arg_hash[Key.View]=this;

            arg_hash[Key.QuotaRemaining]=quota_txt.Text;
            arg_hash[Key.Backoff]=0;

            update_action=this.Update;
    		
            Closed+=MainWindow_Closed;

    		Loaded+=MainWindow_Loaded;
        }else{
        
        number_sites_txt.Text="Api Throtteled";
        }
    return;}

    private static void DownloadFavicon(string uri, string fileName){ 

        if(File.Exists(fileName)==false){
        using(HttpWebResponse response = PostRequest(uri)){

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK || 
                response.StatusCode == HttpStatusCode.Moved || 
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image",StringComparison.OrdinalIgnoreCase))
            {

                //if icon already downloaded skip it
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.Open(fileName, FileMode.OpenOrCreate))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do{
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    }while(bytesRead != 0);
                }
            }
        }
        }
    } 


    void MainWindow_Loaded(object sender, EventArgs args){

        run=true;

        Thread query_thread=new Thread(QuestionsQuery);

		query_thread.Start(arg_hash);
    return;}

    void MainWindow_Closed(object sender, EventArgs args){
        
        lock(all_sites){ run=false;}

        (arg_hash[Key.WaitClose] as AutoResetEvent).WaitOne();
    }
/*class*/}
/*namespace*/}
