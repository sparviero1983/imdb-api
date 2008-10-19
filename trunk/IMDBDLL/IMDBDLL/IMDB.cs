/*
 * This file is part of IMDBDLL.
 *
 *  IMDBDLL is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IMDBDLL is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IMDBDLL.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;

namespace IMDBDLL
{
    /// <summary>
    /// Main class of the API.
    /// </summary>
    public class IMDB
    {
        /// <summary>
        /// String that holds the downloaded page from IMDb.
        /// </summary>
        String page = "";
        
        /// <summary>
        /// String that holds the query.
        /// </summary> 
        String query = "";
        
        /// <summary>
        /// StringBuilder for working on html String
        /// </summary>
        StringBuilder sB;

        /// <summary>
        /// Function that will download the search result from IMDb.
        /// </summary>
        /// <param name="url">The URL of the search result page.</param>
        /// <returns>The result of the page download. OK if there was no problems; The message error if there was any problem.</returns>
        public String getPage(String url)
        {
            page = "";
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                myRequest.Timeout = 6000;
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                page = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return "OK";
        }

        /// <summary>
        /// Function that detects if the download page is a result page or if its the page for a specific title.
        /// </summary>
        /// <param name="media">Defines if it's to search a movie(0) or a TV serie(1).</param>
        /// <param name="q">The query inputed by the user.</param>
        /// <returns>An integer that tells the user if it's a result page or the actual page of the title.</returns>
        public int getPageType(int media, String q)
        {
            query = q;
            String type = "";
            String startPat = "<title>", endPat = "</title>";
            Regex startReg = new Regex(startPat);
            Match startMatch = startReg.Match(page);

            if (startMatch.Success)
            {
                int startInd = startMatch.Index + 7;
                Regex endReg = new Regex(endPat);
                Match endMatch = endReg.Match(page, startInd);
                if (endMatch.Success)
                {
                    type = page.Substring(startInd, endMatch.Index - startInd);
                    String seriePat = "TV series";
                    Regex serieReg = new Regex(seriePat);
                    Match serieMatch = serieReg.Match(page, endMatch.Index + 8);

                    sB = new StringBuilder(page);
                    sB = new StringBuilder(sB.ToString(endMatch.Index + 8, sB.Length - (endMatch.Index + 9)));
                    
                    if (type.StartsWith("IMDb"))
                    {
                        return 0;
                    }
                    else
                    {
                        if (media == 0 && !serieMatch.Success && type.ToLower().Contains(query.ToLower()))
                            return 1;
                        else if (media == 1 && serieMatch.Success && type.ToLower().Contains(query.ToLower()))
                            return 1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// This method gets the titles's links in the result page, from the section "Popular Titles" and "Titles (Exact Match)".
        /// </summary>
        /// <returns>An ArrayList with Strings that represent the links found in those sections.</returns>
        public List<String> parseTitleLinks()
        {
            List<String> links = new List<String>();
            String startPat = "<b>Popular Titles</b>";
            String endPat = "<p><b>";
            int startInd = 0, endInd = 0;
            Regex startReg = new Regex(startPat);
            Match startMatch = startReg.Match(sB.ToString());
            if (startMatch.Success)
            {
                startInd = startMatch.Index + 20;
                Regex endReg = new Regex(endPat);
                Match endMatch = endReg.Match(sB.ToString(), startInd);
                if (endMatch.Success)
                {
                    endInd = endMatch.Index;
                    links = parseLinks(sB.ToString(startInd, endInd-startInd));
                }
            }

            startPat = "Titles \\(Exact Matches\\)";
            endPat = "<p><b>";
            startReg = new Regex(startPat);
            startMatch = startReg.Match(sB.ToString(),endInd);
            if (startMatch.Success)
            {
                startInd = startMatch.Index + 20;
                Regex endReg = new Regex(endPat);
                Match endMatch = endReg.Match(sB.ToString(), startInd);
                if (endMatch.Success)
                {
                    endInd = endMatch.Index;
                    if (links.Count == 0)
                        links = parseLinks(sB.ToString(startInd, endInd - startInd));
                    else
                    {
                        List<String> l = parseLinks(sB.ToString(startInd, endInd - startInd));

                        foreach (String s in l)
                        {
                            links.Add(s);
                        }
                    }
                }
            }
            return links;
        }

        /// <summary>
        /// Parse all the major links found in some piece of String.
        /// </summary>
        /// <param name="text">The String in wich links are to be found.</param>
        /// <returns>An ArrayList with Strings that represent the links found in the String.</returns>
        private List<String> parseLinks(String text)
        {
            List<String> links = new List<String>();
            String link, temp = "";
            String startPat = "href=\"";
            String endPat = "\"";
            Regex startReg = new Regex(startPat);
            Match startMatch = startReg.Match(text);
            Regex endReg;
            Match endMatch;
            int startInd, endInd = -1;
            while (text.Length > 10)
            {
                if (startMatch.Success)
                {
                    startInd = startMatch.Index + 6;
                    if (endInd != -1)
                    {
                        temp = text.Substring(0, startInd);
                    }
                    endReg = new Regex(endPat);
                    endMatch = endReg.Match(text, startInd);

                    if (endMatch.Success)
                    {
                        if (!temp.Contains("Season") && !temp.Contains("Episode"))
                        {
                            endInd = endMatch.Index;
                            link = text.Substring(startInd, endInd - startInd);
                            if (!links.Contains(link))
                                links.Add(link);
                            text = text.Substring(endInd);
                            startMatch = startReg.Match(text);
                        }
                        else
                        {
                            if (endInd < text.Length)
                                text = text.Substring(endInd);
                            else break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return links;
        }

        /// <summary>
        /// Parses an html page with  title information
        /// </summary>
        /// <param name="fields">The fields allowed to be parsed.</param>
        /// <param name="media">If it's to parse a movie or a TV Serie.</param>
        /// <param name="actorN">Number of actors to parse.</param>
        /// <returns>A list of Strings with the info from the title.</returns>
        public ArrayList parseTitlePage(bool[] fields, int media, int actorN)
        {
            StringBuilder sB = new StringBuilder(page);
            ArrayList results = new ArrayList();
            String temp = "";
            String startPat = "<title>";
            Regex startReg = new Regex(startPat);
            Match startMatch = startReg.Match(sB.ToString());
            String title = "", link = "";
            int sPos = 0, ePos  = 0;
            if (startMatch.Success)
            {
                sPos = startMatch.Index + 7;
                startPat = "</title>";
                startReg = new Regex(startPat);
                startMatch = startReg.Match(sB.ToString(), sPos);
                if (startMatch.Success)
                {
                    ePos = startMatch.Index - sPos;
                }
            }
            title = sB.ToString(sPos, ePos);

            bool parse = false;
            startPat = "<h1>";
            startReg = new Regex(startPat);
            startMatch = startReg.Match(sB.ToString(ePos, sB.Length - ePos));
            if (startMatch.Success)
            {
                ePos = startMatch.Index;
            }

            startPat = "</h1>";
            startReg = new Regex(startPat);
            startMatch = startReg.Match(sB.ToString(), ePos);
            if (startMatch.Success)
            {
                temp = sB.ToString(ePos, startMatch.Index - ePos);
                if ((temp.Contains("TV series") && media == 1) || (!temp.Contains("TV series") && media == 0))
                {
                    parse = true;
                }
            }

            if (parse)
            {
                link = "http://www.imdb.com" + sB.ToString(sB.ToString().IndexOf("/title/"), 16);

                results.Add(link);

                if (fields[0]) //Parse the titles's title
                {
                    temp = title.Substring(0, title.IndexOf("(") - 1);
                    if (temp.Contains("&#34;"))
                    {
                        temp = temp.Substring(5, temp.Length - 11);
                    }
                    temp.Replace("&#38;", "");
                    results.Add(temp);
                }
                else
                    results.Add("- ND -");

                if (fields[1]) //Parse the titles's year
                {
                    temp = title.Substring(title.IndexOf("("));
                    if (temp.Contains("/"))
                    {
                        results.Add(temp.Substring(1, temp.IndexOf("/") - 1));
                    }
                    else results.Add(temp.Substring(1, temp.IndexOf(")") - 1));
                }
                else
                    results.Add("- ND -");

                if (fields[2]) //Parse the titles's Cover link
                {
                    startPat = "\"poster\"";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    Regex tempReg = new Regex("http://ia.media-imdb.com/media/imdb/01/I/37/89/15/10.gif");
                    Match tempMatch = tempReg.Match(sB.ToString());
                    if (startMatch.Success && !tempMatch.Success)
                    {
                        sPos = startMatch.Index + 8;
                        startPat = "</a>";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                            temp = sB.ToString(sPos, ePos - sPos);
                            temp = temp.Substring(temp.IndexOf("src") + 5);
                            temp = temp.Substring(0, temp.IndexOf("\""));
                            results.Add(temp);
                        }

                    }
                    else results.Add("- ND -");
                }
                else
                    results.Add("- ND -");

                if (fields[3]) //Parse the titles's User Rating
                {
                    startPat = "<h5>User Rating";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 20;
                        startPat = "</b>";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        results.Add(sB.ToString(ePos - 6, 6));
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else
                        results.Add("- ND -");
                }
                else
                    results.Add("- ND -");

                if (fields[4]) //Parse the titles's Creator/Director
                {
                    parse = false;
                    startPat = "<h5>Director";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 18;
                        parse = true;
                    }
                    else
                    {
                        startPat = "<h5>Creator";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString());
                        if (startMatch.Success)
                        {
                            sPos = startMatch.Index + 17;
                            parse = true;
                        }
                    }
                    if (parse)
                    {
                        startPat = "</a><br/>";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        temp = sB.ToString(sPos, ePos - sPos);
                        startPat = "/\">";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(temp);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index + 3;
                        }

                        results.Add(temp.Substring(ePos, temp.Length - ePos));
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else results.Add("- ND -");
                }
                else
                    results.Add("- ND -");

                if (fields[5]) //Parse the titles's Genres
                {
                    List<String> genres = new List<string>();
                    startPat = "<h5>Genre";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 16;
                        startPat = "tn15more";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        temp = sB.ToString(sPos, ePos - sPos);
                        startPat = "/\">";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(temp);
                        String tempPat = "</a>";
                        Regex tempReg = new Regex(tempPat);
                        Match tempMatch;
                        while (startMatch.Success)
                        {
                            sPos = startMatch.Index + 3;
                            tempMatch = tempReg.Match(temp, sPos);
                            if (tempMatch.Success)
                            {
                                genres.Add(temp.Substring(sPos, tempMatch.Index - sPos));
                            }
                            startMatch = startMatch.NextMatch();
                        }
                        results.Add(genres);
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else
                        results.Add(null);
                }
                else
                    results.Add(null);

                if (fields[6]) //Parse the titles's Tagline
                {
                    startPat = "<h5>Tagline";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 18;
                        startPat = "<";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        results.Add(sB.ToString(sPos, ePos - sPos).Trim());
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else
                        results.Add("- ND -");
                }
                else
                    results.Add("- ND -");

                if (fields[7]) //Parse the titles's Plot
                {
                    startPat = "<h5>Plot";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 15;
                        startPat = "<";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        temp = sB.ToString(sPos, ePos - sPos);
                        temp = temp.Replace("|", "").Trim();
                        results.Add(temp);
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else
                        results.Add("- ND -");
                }
                else
                    results.Add("- ND -");

                if (fields[8]) //Parse the titles's Actors
                {
                    List<String> actors = new List<string>();
                    startPat = "<h3>Cast";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 13;
                        startPat = ">more<";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        temp = sB.ToString(sPos, ePos - sPos);
                        String temp2 = "", actor = "";

                        startPat = "<img src=\"";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(temp);
                        String tempPat = "</a></td>";
                        Regex tempReg = new Regex(tempPat);
                        Match tempMatch;
                        int count = 0;
                        while (startMatch.Success && count != actorN)
                        {
                            tempMatch = tempReg.Match(temp, startMatch.Index);
                            sPos = startMatch.Index + 10;
                            if (tempMatch.Success)
                            {
                                temp2 = temp.Substring(sPos, tempMatch.Index - sPos);
                            }
                            int ind = 0;
                            if (!temp2.Contains("addtiny.gif"))
                            {
                                ind = temp2.IndexOf(".jpg") + 4;
                                actor = temp2.Substring(0, ind) + ", ";
                            }
                            else
                            {
                                actor = "- ND -" + ", ";
                            }
                            temp2 = temp2.Substring(temp2.IndexOf("<a href=\"") + 9, temp2.Length - (temp2.IndexOf("<a href=\"") + 9));

                            ind = temp2.IndexOf("\"");

                            actor += "http://www.imdb.com" + temp2.Substring(0, ind) + ", ";
                            actor += temp2.Substring(ind + 2);

                            actors.Add(actor);

                            actor = "";

                            if (actorN != -1)
                                count++;

                            startMatch = startMatch.NextMatch();
                        }
                        results.Add(actors);
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else
                        results.Add(null);
                }
                else
                    results.Add(null);

                if (fields[9]) //Parse the titles's Runtime
                {
                    startPat = "<h5>Runtime";
                    startReg = new Regex(startPat);
                    startMatch = startReg.Match(sB.ToString());
                    if (startMatch.Success)
                    {
                        sPos = startMatch.Index + 18;
                        startPat = "\n";
                        startReg = new Regex(startPat);
                        startMatch = startReg.Match(sB.ToString(), sPos);
                        if (startMatch.Success)
                        {
                            ePos = startMatch.Index;
                        }
                        temp = sB.ToString(sPos, ePos - sPos);
                        temp = temp.Replace(" min", "").Trim();
                        int ind = temp.IndexOf("|");
                        if (ind != -1)
                            temp = temp.Substring(0, ind);
                        results.Add(temp);
                        sB = new StringBuilder(sB.ToString(ePos, sB.Length - ePos));
                    }
                    else
                        results.Add("- ND -");
                }
                else
                    results.Add("- ND -");
            }
            return results;
        }
    }
}
