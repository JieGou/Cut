<%@ WebService Language="C#" Class="MapGuideWebService" %>
using System;
using System.Web.Services;
[WebService(Namespace = "http://localhost/Mapguide/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class MapGuideWebService  : System.Web.Services.WebService {
    [WebMethod(Description = " MapGuide ·â×°ÍøÂç·þÎñ")]
    public ParcelProperty[] GetParcelList(String address, String parcelType, double bufferDistance)
    {
        UtilityClass utility = new UtilityClass();
        utility.InitializeWebTier(@"C:\Program Files\Autodesk\MapGuideEnterprise2007\WebServerExtensions\www\webconfig.ini");
        utility.ConnectToServer("Administrator", "admin");
        Geocoding geocoding = new Geocoding();
        GeocodeAddress geocodeAddress = geocoding.RequestGeocodeAddress(address);
        ParcelProperty[] props = utility.GetNearParcels(parcelType, geocodeAddress, 0.1);
      
        return props;
    }        
}



