using System;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IMDBDLL
{
    /// <summary>
    /// Class to execute the first steps of a search
    /// </summary>
    public class IMDB : IIMDb
    {
        private string m_title;
        private string page;
        private Title title;
        private string tit, temp;
        //maximum of 3 genres per title
        private string[] t_genres = new string[3];
        //maximum of 5 actors per title
        private string[] t_actors = new string[5];
        private int ind, index;

        /// <summary>
        /// Executes the search by title in IMDB.
        /// </summary>
        public bool searchByTitle(string title)
        {
            m_title = title;
            page = "";
            string url = "http://www.imdb.com/find?s=all&q=" + m_title + "&x=0&y=0";

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                page = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Executes the search by id in IMDB.
        /// </summary>
        public bool searchByID(string ID)
        {
            page = "";
            string url = "http://www.imdb.com/title/"+ID+"/";

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                page = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the page type
        /// </summary>
        ///  <param name="formato"> Defines if it's to search a movie or a tv show
        ///  0 - movie
        ///  1 - tv show</param>
        /// <returns>If the page is a result list page - 1
        /// or the actual result - 0</returns>
        public int getType(int formato)
        {
            ParseHTML parse = new ParseHTML();
            parse.Source = page;
            bool b = true;
            while (!parse.Eof() && b)
            {
                char ch = parse.Parse();
                if (ch == 0)
                {
                    AttributeList tag = parse.GetTag();
                    if (tag["content"] != null)
                    {
                        string res = tag["content"].Value;
                        res = res.ToLower();
                        string t = "IMDb  Search";
                        if (formato == 0 && res.Contains(m_title.ToLower()) && !page.Contains("TV series"))
                        {
                            m_title = tag["content"].Value;
                            b = false;
                            return 0;
                        }
                        else if (formato == 0 && res.Contains(t.ToLower()))
                        {
                            b = false;
                            return 1;
                        }
                        else if (formato == 1 && res.Contains(m_title.ToLower()) && page.Contains("TV series"))
                        {
                            m_title = tag["content"].Value;
                            b = false;
                            return 0;
                        }
                        else if (formato == 1 && res.Contains(t.ToLower()))
                        {
                            b = false;
                            return 1;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Parses the search results, to get the links
        /// </summary>
        /// <returns>An array of links of popular titles and exact matches</returns>
        public ArrayList parseSearch()
        {
            bool pop = true, ex = true;
            int ind = page.IndexOf("Popular Titles");
            if (ind < 0)
            {
                pop = false;
                ind = page.IndexOf("Titles (Exact Matches)");
                if (ind < 0)
                {
                    ex = false;
                    return null;
                }
            }
            if(pop || ex)
            {
                string temp = page.Substring(ind);
                if (pop)
                {
                    ind = temp.IndexOf("Titles (Exact Matches)");
                    if (ind < 0)
                        temp = temp.Substring(0, temp.IndexOf("<p><b>"));
                    else
                    {
                        int pos = temp.IndexOf("<p><b>");
                        pos = temp.IndexOf("<p><b>", pos + 1);
                        temp = temp.Substring(0, pos);
                    }
                }
                else
                {
                    temp = temp.Substring(0, temp.IndexOf("<p><b>"));
                }

                ParseHTML parse = new ParseHTML();
                ArrayList links = new ArrayList();
                parse.Source = temp;
                while (!parse.Eof())
                {
                    char ch = parse.Parse();
                    if (ch == 0)
                    {
                        AttributeList tag = parse.GetTag();
                        if (tag["href"] != null)
                        {
                            if (!links.Contains(tag["href"].Value))
                                links.Add(tag["href"].Value);
                        }
                    }
                }
                return links;
            }
            return null;
        }

        /// <summary>
        /// Returns the object with info of the title
        /// </summary>
        /// <returns>The object that contains the info fetched from the site</returns>
        public Title getTitle()
        {
            return title;
        }

        /// <summary>
        /// Gets the title from IMDb
        /// </summary>
        private void parseTitle()
        {
            string temp;
            temp = tit.Substring(0, tit.IndexOf("("));
            if (temp.Contains("&#34;"))
            {
                temp = temp.Substring(5, temp.Length - 10);
            }
            if (temp.Contains("&#38;"))
            {
                temp.Replace("&#38;", "&");
            }
            title.Titulo = temp;
        }

        /// <summary>
        /// Gets the year from IMDb
        /// </summary>
        private void parseYear()
        {
            tit = tit.Substring(tit.IndexOf("("));
            if (tit.Contains("/"))
            {
                title.Year = tit.Substring(1, tit.IndexOf("/") - 1);
            }
            else title.Year = tit.Substring(1, tit.IndexOf(")") - 1);
        }

        /// <summary>
        /// Gets the cover link from IMDb
        /// </summary>
        private void parseCoverLink()
        {
            ind = page.IndexOf("\"poster\"");
            index = 0;
            temp = "";
            if (ind != -1)
            {
                temp = page.Substring(ind + 90, 300);
                index = ind + 90 + temp.Length;
                temp = temp.Substring(temp.IndexOf("src") + 5);
                temp = temp.Substring(0, temp.IndexOf("\""));

                temp = temp.Replace("SY140_SX100", "SY400_SX600");
                title.ImageURL = temp;
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the link from IMDb
        /// </summary>
        private void parseLink()
        {
            title.Link = "http://www.imdb.com" + page.Substring(page.IndexOf("/title/"), 17);
        }

        /// <summary>
        /// Gets the rate from IMDb
        /// </summary>
        private void parseRate()
        {
            ind = -1;
            temp = "";
            ind = page.IndexOf("User Rating:");
            if (ind != -1)
            {
                temp = page.Substring(ind + 21, 10);
                index = ind + 21 + temp.Length;
                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("/"))
                    {
                        title.SiteRate = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the director from IMDb
        /// </summary>
        private void parseDirector()
        {
            ind = -1;
            temp = "";
            ind = page.IndexOf("Director:");
            if (ind != -1)
            {
                bool b = true;
                temp = page.Substring(ind + 15, 150);
                index = ind + 15 + temp.Length;
                temp = temp.Substring(temp.IndexOf("\">") + 2);
                for (int i = 1; (i <= temp.Length) || (b); i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        try
                        {
                            title.Director = temp.Substring(0, i - 1);
                            break;
                        }
                        catch (Exception)
                        {
                            b = false;
                        }
                    }
                }
                page = page.Substring(index);
            }
            else if ((ind = page.IndexOf("Directors:")) != -1)
            {
                bool b = true;
                temp = page.Substring(ind + 15, 150);
                index = ind + 15 + temp.Length;
                temp = temp.Substring(temp.IndexOf("\">") + 2);
                for (int i = 1; (i <= temp.Length) || (b); i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        try
                        {
                            title.Director = temp.Substring(0, i - 1);
                            break;
                        }
                        catch (Exception)
                        {
                            b = false;
                        }
                    }
                }
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the genres from IMDb
        /// </summary>
        private void parseGenres()
        {
            ind = -1;
            temp = "";
            ind = page.IndexOf("Genre:");
            if (ind != -1)
            {
                int j = 0;
                bool b = true;
                temp = page.Substring(ind + 35);
                temp = temp.Substring(temp.IndexOf(">") + 1, temp.IndexOf(">more<"));
                index = ind + 35 + temp.Length;
                for (int i = 1; (i <= temp.Length && j < 3) || (b); i++)
                {
                    try
                    {
                        if (temp.Substring(0, i).EndsWith("<"))
                        {

                            t_genres[j] = temp.Substring(0, i - 1);
                            j++;
                            temp = temp.Substring(temp.IndexOf("<a href") + 10);
                            temp = temp.Substring(temp.IndexOf(">") + 1);
                            i = 0;
                        }
                    }
                    catch (Exception)
                    {
                        b = false;
                    }
                }
                title.Genres = t_genres;
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the tagline from IMDb
        /// </summary>
        private void parseTag()
        {
            ind = -1;
            temp = "";
            ind = page.IndexOf("Tagline:</h5>");
            if (ind != -1)
            {
                temp = page.Substring(ind + 14);
                temp = temp.Substring(0, temp.IndexOf("<div class=\"info\">"));
                index = ind + 14 + temp.Length;
                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        title.Tagline = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the plot from IMDb
        /// </summary>
        private void parsePlot()
        {
            ind = -1;
            temp = "";
            ind = page.IndexOf("Plot:");
            if (ind != -1)
            {
                temp = page.Substring(ind + 10);
                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        title.Description = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                index = ind + 20 + title.Description.Length;
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the actors from IMDb
        /// </summary>
        private void parseActors()
        {
            ind = -1;
            temp = "";
            ind = page.IndexOf("\"cast\"");
            if (ind != -1)
            {
                int j = 0;
                bool b = true;
                temp = page.Substring(ind + 25);
                temp = temp.Substring(temp.IndexOf("img src"));
                temp = temp.Substring(temp.IndexOf("href"));
                temp = temp.Substring(temp.IndexOf(">") + 1, temp.IndexOf(">more<"));
                index = ind + 25 + temp.Length;
                for (int i = 1; (i <= temp.Length && j < 5) || (b); i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        try
                        {
                            t_actors[j] = temp.Substring(0, i - 1);

                            temp = temp.Substring(200);
                            j++;
                            if (j < 5)
                            {
                                temp = temp.Substring(temp.IndexOf("img src"));
                                temp = temp.Substring(temp.IndexOf("href"));
                                temp = temp.Substring(temp.IndexOf(">") + 1);
                            }
                            i = 0;
                        }
                        catch (Exception)
                        {
                            b = false;
                        }
                    }
                }
                title.Actors = t_actors;
                page = page.Substring(index);
            }
        }

        /// <summary>
        /// Gets the runtime from IMDb
        /// </summary>
        private void parseRuntime()
        {
            string t_runningTime = "";
            ind = -1;
            temp = "";
            ind = page.IndexOf("Runtime:");
            if (ind != -1)
            {
                temp = page.Substring(ind + 14);

                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        t_runningTime = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                title.RunningTime = t_runningTime.Substring(0, t_runningTime.IndexOf("min"));
            }
        }

        /// <summary>
        /// Parses the title page to get info from a movie or tv show
        /// </summary>
        /// <param name="fields">Fields to parse</param>
        public void parseTitlePage(bool[] fields)
        {
            title = new Title();

            int pos = page.IndexOf("<title>") + 7;
            int l = page.IndexOf("</title>", pos + 1) - pos;
            tit = page.Substring(pos, l);

            //link
            if(fields==null)
                parseLink();
            //title
            if (fields==null || fields[0])
                parseTitle();

            //year
            if (fields == null || fields[1])
                parseYear();

            //image link
            if (fields == null || fields[9])
                parseCoverLink();
            
            //user rating
            if (fields == null || fields[7])
                parseRate();

            //directors
            if (fields == null || fields[2])
                parseDirector();

            //genre
            if (fields == null || fields[3])
                parseGenres();
            
            //tagline
            if (fields == null || fields[4])
                parseTag();

            //plot
            if (fields == null || fields[5])
                parsePlot();
            
            //actors
            if (fields == null || fields[6])
                parseActors();

            //runtime
            if (fields == null || fields[8])
                parseRuntime();
        }
    }

    /// <summary>
    /// Class to create a thread for each page to be parsed
    /// </summary>
    public class IMDBThread : IIMDbT
    {
        private string link, page;
        private AutoResetEvent aRE;
        private Title title;
        private int tipo;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public IMDBThread(string l, AutoResetEvent a, int t)
        {
            link = l;
            aRE = a;
            tipo = t;
            title = new Title();
        }

        /// <summary>
        /// gets the correct title page and parses it to get info from a movie or tv show
        /// </summary>
        public void parseTitlePage()
        {
            page = "";
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://www.imdb.com" + link);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                page = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            int pos = page.IndexOf("<title>") + 7;
            int l = page.IndexOf("</title>", pos + 1) - pos;
            string tit = page.Substring(pos, l);

            if (tipo != 1)
            {
                pos = page.IndexOf("<div id=\"tn15title\">") + 20;
                l = page.IndexOf("</div>", pos + 1) - pos;
                string temp = page.Substring(pos, l);
                if (!temp.Contains("TV series"))
                {
                    parse(tit, page);
                }
            }
            else
            {
                pos = page.IndexOf("<div id=\"tn15title\">") + 20;
                l = page.IndexOf("</div>", pos + 1) - pos;
                string temp = page.Substring(pos, l);
                if (temp.Contains("TV series"))
                {
                    parse(tit, page);
                }
            }
            aRE.Set();
        }

        //parse the title page and creates an object with its info
        private void parse(string tit, string page)
        {
            string temp, t_runningTime = "";

            //maximum of 3 genres per title
            string[] t_genres = new string[3];
            //maximum of 5 actors per title
            string[] t_actors = new string[5];

            //link
            title.Link = "http://www.imdb.com" + link;
            //title
            temp = tit.Substring(0, tit.IndexOf("("));
            if (temp.Contains("&#34;"))
            {
                temp = temp.Substring(5, temp.Length - 10);
            }
            if (temp.Contains("&#38;"))
            {
                temp.Replace("&#38;", "&");
            }
            title.Titulo = temp;
            //year
            tit = tit.Substring(tit.IndexOf("("));
            if (tit.Contains("/"))
            {
                title.Year = tit.Substring(1, tit.IndexOf("/") - 1);
            }
            else title.Year = tit.Substring(1, tit.IndexOf(")") - 1);
            //image link
            int ind = page.IndexOf("\"poster\"");
            int index = 0;
            temp = "";
            if (ind != -1)
            {
                temp = page.Substring(ind + 90, 300);
                index = ind + 90 + temp.Length;
                temp = temp.Substring(temp.IndexOf("src") + 5);
                temp = temp.Substring(0, temp.IndexOf("\""));

                temp = temp.Replace("SY140_SX100", "SY400_SX600");
                title.ImageURL = temp;
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //user rating
            ind = page.IndexOf("User Rating:");
            if (ind != -1)
            {
                temp = page.Substring(ind + 21, 10);
                index = ind + 21 + temp.Length;
                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("/"))
                    {
                        title.SiteRate = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //directors
            ind = page.IndexOf("Director:");
            if (ind != -1)
            {
                bool b = true;
                temp = page.Substring(ind + 15, 150);
                index = ind + 15 + temp.Length;
                temp = temp.Substring(temp.IndexOf("\">") + 2);
                for (int i = 1; (i <= temp.Length) || (b); i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        try
                        {
                            title.Director = temp.Substring(0, i - 1);
                            break;
                        }
                        catch (Exception)
                        {
                            b = false;
                        }
                    }
                }
                page = page.Substring(index);
            }
            else if ((ind = page.IndexOf("Directors:")) != -1)
            {
                bool b = true;
                temp = page.Substring(ind + 15, 150);
                index = ind + 15 + temp.Length;
                temp = temp.Substring(temp.IndexOf("\">") + 2);
                for (int i = 1; (i <= temp.Length) || (b); i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        try
                        {
                            title.Director = temp.Substring(0, i - 1);
                            break;
                        }
                        catch (Exception)
                        {
                            b = false;
                        }
                    }
                }
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //genre
            ind = page.IndexOf("Genre:");
            if (ind != -1)
            {
                int j = 0;
                bool b = true;
                temp = page.Substring(ind + 35);
                temp = temp.Substring(temp.IndexOf(">") + 1, temp.IndexOf(">more<"));
                index = ind + 35 + temp.Length;
                for (int i = 1; (i <= temp.Length && j < 3) || (b); i++)
                {
                    try
                    {
                        if (temp.Substring(0, i).EndsWith("<"))
                        {

                            t_genres[j] = temp.Substring(0, i - 1);
                            j++;
                            temp = temp.Substring(temp.IndexOf("<a href") + 10);
                            temp = temp.Substring(temp.IndexOf(">") + 1);
                            i = 0;
                        }
                    }
                    catch (Exception)
                    {
                        b = false;
                    }
                }
                title.Genres = t_genres;
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //tagline
            ind = page.IndexOf("Tagline:</h5>");
            if (ind != -1)
            {
                temp = page.Substring(ind + 14);
                temp = temp.Substring(0, temp.IndexOf("<div class=\"info\">"));
                index = ind + 14 + temp.Length;
                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        title.Tagline = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //description
            ind = page.IndexOf("Plot:");
            if (ind != -1)
            {
                temp = page.Substring(ind + 10);
                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        title.Description = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                index = ind + 20 + title.Description.Length;
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //actors
            ind = page.IndexOf("\"cast\"");
            if (ind != -1)
            {
                int j = 0;
                bool b = true;
                temp = page.Substring(ind + 25);
                temp = temp.Substring(temp.IndexOf("img src"));
                temp = temp.Substring(temp.IndexOf("href"));
                temp = temp.Substring(temp.IndexOf(">") + 1, temp.IndexOf(">more<"));
                index = ind + 25 + temp.Length;
                for (int i = 1; (i <= temp.Length && j < 5) || (b); i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        try
                        {
                            t_actors[j] = temp.Substring(0, i - 1);

                            temp = temp.Substring(200);
                            j++;
                            if (j < 5)
                            {
                                temp = temp.Substring(temp.IndexOf("img src"));
                                temp = temp.Substring(temp.IndexOf("href"));
                                temp = temp.Substring(temp.IndexOf(">") + 1);
                            }
                            i = 0;
                        }
                        catch (Exception)
                        {
                            b = false;
                        }
                    }
                }
                title.Actors = t_actors;
                page = page.Substring(index);
            }
            ind = -1;
            temp = "";
            //runtime
            ind = page.IndexOf("Runtime:");
            if (ind != -1)
            {
                temp = page.Substring(ind + 14);

                for (int i = 1; i <= temp.Length; i++)
                {
                    if (temp.Substring(0, i).EndsWith("<"))
                    {
                        t_runningTime = temp.Substring(0, i - 1);
                        i = temp.Length + 1;
                    }
                }
                title.RunningTime = t_runningTime.Substring(0, t_runningTime.IndexOf("min"));
            }
        }

        /// <summary>
        /// Returns the object with info of the title
        /// </summary>
        /// <returns>The object that contains the info fetched from the site</returns>
        public Title getTitle()
        {
            return title;
        }
    }
}
