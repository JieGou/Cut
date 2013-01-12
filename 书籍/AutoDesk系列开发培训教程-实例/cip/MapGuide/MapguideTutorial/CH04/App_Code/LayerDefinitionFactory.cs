using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using OSGeo.MapGuide;


public class LayerDefinitionFactory
{

    String rootDirectoryPath="";

    public String RootDirectoryPath
    {
        get { return rootDirectoryPath; }
        set { rootDirectoryPath = value; }
    }


    public String LoadTemplateFile(String filePath)
    {
        StreamReader sr = File.OpenText(rootDirectoryPath + filePath);
        return sr.ReadToEnd();
    }


    public String Substitute(String templ, String[] vals)
    {
        StringBuilder res = new StringBuilder();
        int index = 0, val = 0;
        bool found;
        do
        {
            found = false;
            int i = templ.IndexOf('%', index);
            if (i != -1)
            {
                found = true;
                res.Append(templ.Substring(index, i - index));
                if (i < templ.Length - 1)
                {
                    if (templ[i + 1] == '%')
                        res.Append('%');
                    else if (templ[i + 1] == 's')
                        res.Append(vals[val++]);
                    else
                        res.Append('@');    //add a character illegal in jscript so we know the template was incorrect
                    index = i + 2;
                }
            }
        } while (found);
        res.Append(templ.Substring(index));
        return res.ToString();
    }

        //Creates Area Rule
    //Parameters:
    //foreGroundColor - color code for the foreground color
    //legendLabel - string for the legend label
    //filterText - filter string
    //textSymbol - use textsymbol.templ to create it
    public String CreateAreaRule(string legendLabel, string filterText, string foreGroundColor)
    {
        String areaRule = LoadTemplateFile("/viewerfiles/arearule.templ");
      
        String[] vals = {
            legendLabel,
            filterText,
            foreGroundColor
        };
        areaRule = Substitute(areaRule, vals);
        return areaRule;
    }
     
    //Creates AreaTypeStyle.
    //Parameters:
    //areaRules - call CreateAreaRule to create area rules
    public string  CreateAreaTypeStyle(string areaRules)
    {
        string style = LoadTemplateFile("/viewerfiles/areatypestyle.templ");
        string[] strCollection = {areaRules};
        style = Substitute(style, strCollection);
        return style;

    }


    //Creates line rule
    //Parameters:
    //color - color code for the line
    //legendLabel - string for the legend label
    //filter - filter string
    public string CreateLineRule(string legendLabel, string filter, string color)
    {
        string lineRule = LoadTemplateFile("/viewerfiles/linerule.templ");
        string[] strColletion = { legendLabel, filter, color };
        lineRule = Substitute(lineRule, strColletion);
        //lineRule = sprintf(lineRule, legendLabel, filter, color);
        return lineRule;
    }

    //Creates LineTypeStyle
    //Parameters:
    //lineRules - call CreateLineRule to create line rules
    public string CreateLineTypeStyle(string lineRules)
    {
        string lineStyle = LoadTemplateFile("/viewerfiles/linetypestyle.templ");
        string[] strCollection = { lineRules };
        lineStyle = Substitute(lineStyle, strCollection);
        return lineStyle;
    }

     //Creates mark symbol
    //Parameters:
    //resourceId - resource identifier for the resource to be used
    //symbolName - the name of the symbol
    //width - the width of the symbol
    //height - the height of the symbol
    //color - color code for the symbol color
    public string CreateMarkSymbol(string  resourceId, string  symbolName, string  width, string  height, string  color)
    {
        string markSymbol = LoadTemplateFile("/viewerfiles/marksymbol.templ");
        string[] strCollection = { width, height, resourceId, symbolName, color };
        markSymbol = Substitute(markSymbol, strCollection);
        return markSymbol;
    }

    //Creates text symbol
    //Parameters:
    //text - string for the text
    //fontHeight - the height for the font
    //TODO:Can we pass it as a integer (ex. 10) or string (ex"10")
    //foregroundColor - color code for the foreground color
    public string CreateTextSymbol(string text, string fontHeight, string foregroundColor)
    {
        string textSymbol = LoadTemplateFile("/viewerfiles/textsymbol.templ");
        string[] strCollection = { fontHeight, fontHeight, text, foregroundColor};
        textSymbol = Substitute (textSymbol,strCollection );
        return textSymbol;
    }
    //
    
    //Creates a point rule
    //Parameters:
    //pointSym - point symbolization. Use CreateMarkSymbol to create it
    //legendlabel - string for the legend label
    //filter - string for the filter
    //label - use CreateTextSymbol to create it
    public string CreatePointRule(string legendLabel, string filter, string label, string pointSym)
    {
        string pointRule = LoadTemplateFile("/viewerfiles/pointrule.templ");
        string[] strCollection = { legendLabel, filter, label, pointSym };
        pointRule =Substitute(pointRule , strCollection);
        return pointRule;
    }


        //Creates PointTypeStyle
    //Parameters:
    //pointRule - use CreatePointRule to define rules
    public string CreatePointTypeStyle(string pointRule)
    {
        string pointTypeStyle = LoadTemplateFile("/viewerfiles/pointtypestyle.templ");
        string[] strCollection = { pointRule };
        pointTypeStyle = Substitute(pointTypeStyle, strCollection);
        return pointTypeStyle;
    }

     //Creates ScaleRange
    //Parameterss
    //minScale - minimum scale
    //maxScale - maximum scale
    //typeStyle - use one CreateAreaTypeStyle, CreateLineTypeStyle, or CreatePointTypeStyle
    public string  CreateScaleRange(string minScale, string maxScale, string typeStyle)
    {
        string scaleRange =LoadTemplateFile("/viewerfiles/scalerange.templ");
        string[] strCollection = { minScale, maxScale, typeStyle };
        scaleRange = Substitute(scaleRange, strCollection);
        return scaleRange;
    }


        //Creates a layer definition
    //resourceId - resource identifier for the new layer
    //featureClass - the name of the feature class
    //geometry - the name of the geometry
    //featureClassRange - use CreateScaleRange to define it.
    public string CreateLayerDefinition(string  resourceId, string featureClass, string geometry, string featureClassRange)
    {
        string layerDef = LoadTemplateFile("/viewerfiles/layerdefinition.templ");
        string[] strCollection = { resourceId, featureClass, geometry, featureClassRange };
        //layerDef = sprintf(layerDef, resourceId, featureClass, geometry, featureClassRange);
        layerDef = Substitute(layerDef, strCollection);

        return layerDef;
    }

	public LayerDefinitionFactory()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}
