/* 
* @Author: CLS
* @Date:   07-06-2014 16:56:30
* @Last Modified by:   CLS
* @Last Modified time: 26-06-2014 18:34:05
*/

using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using CodeLibrary.Visuals;

namespace StackExchangeMonitor {

public class SiteTile{
     
    public string icon_url { get; set; }
     
    public string audience { get; set; }
     
    public string site_url { get; set; }
     
    public string api_site_parameter { get; set; }
     
    public string logo_url { get; set; }
     
    public string name { get; set; }
     
    public string site_type { get; set; }

    public string favi_img {get; set;}
	
	public string top_title{get; set;}

	public uint num_questions=0;

	public Styling styling{get; set;}

	public Brush fore{get; set;}
	public Brush back{get; set;}
    
    private SiteTile(){return;}

	public static Brush StringToBrush( string str ) {
		uint hex=0;

		if(str.Length<7){
			StringBuilder sb=new StringBuilder();

			foreach( char c in str.Substring(1)){
				sb.Append(c);
				sb.Append(c);
			};

			str=sb.ToString();
		}else{
			str=str.Substring(1);
		}
		hex=(uint)Convert.ToInt32(str, 16);
	return Palette.BrushFromRgb(hex);}

    public static SiteTile CreateFrom(Site siteItem){

    	SiteTile tile=new SiteTile();
    	tile.name=siteItem.name;
    	tile.audience=siteItem.audience;
		tile.api_site_parameter=siteItem.api_site_parameter;

    	tile.icon_url=siteItem.icon_url;    	
    	tile.site_url=siteItem.site_url;
		tile.logo_url=siteItem.logo_url;

    	tile.site_type=siteItem.site_type;
    	tile.styling=siteItem.styling;

		tile.fore= StringToBrush(siteItem.styling.tag_foreground_color);

		tile.back= StringToBrush(siteItem.styling.tag_background_color);

		if( tile.name.StartsWith("Geographic Information Systems") ){
			tile.name=tile.name.Replace("Geographic Information Systems", "GIS");
		}
    return tile;}

    public SiteTile WithFaviconImage(string n_favi_img){
    	this.favi_img=n_favi_img;
    return this;}

    public override string ToString(){return name;}
}
}
