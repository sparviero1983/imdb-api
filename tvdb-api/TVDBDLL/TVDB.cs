/*
 * This file is part of TVDBDLL.
 *
 *  TVDBDLL is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  TVDBDLL is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with TVDBDLL.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Diagnostics;
using TVDBDLL.ICSharpCode.SharpZipLib.Zip;
using TVDBDLL.SharpZipLib.Zip;

namespace TVDBDLL
{
    /// <summary>
    /// Main Class of the API
    /// </summary>
    public class TVDB
    {
        /// <summary>
        /// Delegate that calls the parent error handler.
        /// </summary>
        public Delegate parentErrorCaller;

        /// <summary>
        /// Delegate that calls the parent progress update handler.
        /// </summary>
        public Delegate parentProgressUpdaterCaller;

        /// <summary>
        /// Delegate that calls the parent data receiver handler.
        /// </summary>
        public Delegate parentDataReceiverCaller;

        /// <summary>
        /// Api Key to acces tvdb.
        /// </summary>
        private const String apiKey = "57B9F10354E657BE";

        /// <summary>
        /// Only active mirror.
        /// </summary>
        private const String mirror = "http://thetvdb.com/";

        /// <summary>
        /// Preferred language of series.
        /// </summary>
        private String language = "";

        /// <summary>
        /// Wich info to fetch.
        /// </summary>
        private bool ser = false, acts = false, banns = false;

        /// <summary>
        /// Calls the error handler of the caller form.
        /// </summary>
        /// <param name="exc">The exception.</param>
        private void errorHandler(Exception exc)
        {
            parentErrorCaller.DynamicInvoke(new Object[] { exc });
        }

        /// <summary>
        /// Calls the progress handler of the caller form.
        /// </summary>
        /// <param name="value">The value to set the progress.</param>
        private void progressHandler(int value)
        {
            parentProgressUpdaterCaller.DynamicInvoke(new Object[] { value });
        }

        /// <summary>
        /// Calls the data receiver of the caller form.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="type">The type of data.</param>
        private void dataHandler(object data, int type)
        {
            parentDataReceiverCaller.DynamicInvoke(new Object[] { data, type });
        }

        /// <summary>
        /// Searches a serie by name in the site
        /// </summary>
        /// <param name="language">Preferred language</param>
        /// <param name="name">Name of the serie</param>
        /// <returns>A list of ResultItem</returns>
        public List<ResultItem> searchSerie(String language, String name)
        {
            this.language = language;
            String xml = "";
            String link = mirror + "api/GetSeries.php?seriesname=" + name + "&language=" + language;
            WebClient webClient = new WebClient();
            xml = webClient.DownloadString(link);
            return parseSearchResultXML(xml);
        }

        /// <summary>
        /// Parses the XML of the search result from the site into ResultItem
        /// </summary>
        /// <param name="xml">The search result in xml format</param>
        /// <returns>A list of ResultItem</returns>
        private List<ResultItem> parseSearchResultXML(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(xml));
            List<ResultItem> results = new List<ResultItem>();

            if (doc.DocumentElement.HasChildNodes)
            {
                XmlNodeList series = doc.DocumentElement.ChildNodes;
               
                foreach (XmlNode serie in series)
                {
                    String seriesid="";
                    String language="";
                    String SeriesName="";
                    String banner="";
                    String Overview="";
                    String IMDB_ID="";

                    XmlNodeList infos = serie.ChildNodes;
                    for (int i = 0; i < infos.Count; i++)
                    {
                        if (infos[i].Name == "seriesid")
                        {
                            seriesid = infos[i].InnerText;
                        }
                        else if (infos[i].Name == "language")
                        {
                            language = infos[i].InnerText;
                        }
                        else if (infos[i].Name == "SeriesName")
                        {
                            SeriesName = infos[i].InnerText;
                        }
                        else if (infos[i].Name == "banner")
                        {
                            banner = infos[i].InnerText;
                        }
                        else if (infos[i].Name == "Overview")
                        {
                            Overview = infos[i].InnerText;
                        }
                        else if (infos[i].Name == "IMDB_ID")
                        {
                            IMDB_ID = infos[i].InnerText;
                        }
                    }
                    ResultItem res = new ResultItem(seriesid, SeriesName, language, Overview, banner, IMDB_ID);
                    results.Add(res);
                }
                return results;
            }
            return null;

        }

        /// <summary>
        /// Handler of the download progress
        /// </summary>
        /// <param name="sender">default argument.</param>
        /// <param name="e">default argument.</param>
        private void webClient_downloadDataProgressChange(object sender, DownloadProgressChangedEventArgs e)
        {
            progressHandler(e.ProgressPercentage);
        }

        /// <summary>
        /// Handler of the download complete
        /// </summary>
        /// <param name="sender">default argument.</param>
        /// <param name="e">default argument.</param>
        private void webClient_downloadDataComplete(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && e.Error == null)
                {
                    byte[] xml = e.Result;
                    ZipInputStream zip = new ZipInputStream(new MemoryStream(xml));

                    ZipEntry entry = zip.GetNextEntry();
                    String seriesString = null;
                    String actorsString = null;
                    String bannersString = null;
                    while (entry != null)
                    {
                        byte[] buffer = new byte[zip.Length];
                        int count = zip.Read(buffer, 0, (int)zip.Length);
                        if (ser && (entry.Name.Equals(language + ".xml") || entry.Name.Equals("en.xml")))
                        {
                            seriesString = Encoding.UTF8.GetString(buffer);
                        }
                        else if (banns && entry.Name.Equals("banners.xml"))
                        {
                            bannersString = Encoding.UTF8.GetString(buffer);
                        }
                        else if (acts && entry.Name.Equals("actors.xml"))
                        {
                            actorsString = Encoding.UTF8.GetString(buffer);
                        }
                        entry = zip.GetNextEntry();
                    }
                    zip.Close();

                    List<String> results = new List<string>();

                    results.Add(seriesString);
                    results.Add(actorsString);
                    results.Add(bannersString);
                    if (ser && acts && banns)
                        dataHandler(results, 0);
                    else if(ser && acts && !banns)
                        dataHandler(results, 1);
                    else if (ser && !acts && banns)
                        dataHandler(results, 2);
                    else if (ser && !acts && !banns)
                        dataHandler(results, 3);
                    else if (!ser && acts && !banns)
                        dataHandler(results, 4);
                    else if (!ser && !acts && banns)
                        dataHandler(results, 5);
                    else if (!ser && acts && banns)
                        dataHandler(results, 6);
                }
                else if (e.Error != null)
                {
                    errorHandler(e.Error);
                }
            }
            catch (Exception ex)
            {
                errorHandler(ex);
            }
        }

        /// <summary>
        /// Downloads infos of a serie from the site, in zip format
        /// </summary>
        /// <param name="id">id of the serie</param>
        /// <param name="ser">if its to parse the serie info</param>
        /// <param name="acts">if its to parse the actors info</param>
        /// <param name="banns">if its to parse the banners info</param>
        public void DownloadSerieZipped(String id, bool ser, bool acts, bool banns)
        {
            String link = mirror + "/api/" + apiKey + "/series/" + id + "/all/" + language + ".zip";
            this.ser = ser;
            this.acts = acts;
            this.banns = banns;
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_downloadDataProgressChange);
                webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(webClient_downloadDataComplete);
                Uri uri = new Uri(link);
                webClient.DownloadDataAsync(uri);
                
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            
        }
    }
}
