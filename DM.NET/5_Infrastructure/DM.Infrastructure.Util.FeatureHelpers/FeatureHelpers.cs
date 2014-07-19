using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DM.Infrastructure.Cache;

namespace DM.Infrastructure.Util.FeatureHelpers
{
    public class FeatureHelpers
    {
        public static string GetFeature(string featureID)
        {
            return GetFeature(featureID, "Features");
        }

        /// <summary>
        /// Get the feature value
        /// </summary>
        /// <param name="featureID">featureID</param>
        /// <param name="featureSetID">file name</param>
        /// <returns></returns>
        public static string GetFeature(string featureID, string featureSetID)
        {
            XElement featureXml = FeatureItem.Get(featureSetID).Value;
            IEnumerable<XElement> featureList = featureXml.XPathSelectElements("Feature[@ID='" + featureID + "']");
            if (featureList.Any())
            {
                if (featureList.Count() > 1)
                {
                    return featureList.Where(
                        x => x.Attribute("Environment") != null
                            && x.Attribute("Environment").Value == ConfigurationManager.AppSettings["Environment"]
                            ).First().Value;
                }
                else
                {
                    return featureList.First().Value;
                }
            }
            return string.Empty;
        }
    }
}
