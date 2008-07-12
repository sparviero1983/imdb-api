using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMDBDLL
{
    /// <summary>
    /// Class that represents a movie or a tv serie
    /// </summary>
    public class Title
    {
        private string link, titulo, year, imageURL, siteRate, director, tagline, description, runningTime;
        private string[] genres, actors;

        /// <summary>
        /// get/set title
        /// </summary>
        public string Titulo
        {
            get
            {
                if (titulo != null)
                    return titulo;
                else return "- ND -";
            }

            set
            {
                titulo = value;
            }
        }

        /// <summary>
        /// get/set link
        /// </summary>
        public string Link
        {
            get
            {
                if (link != null)
                    return link;
                else return "- ND -";
            }

            set
            {
                link = value;
            }
        }

        /// <summary>
        /// get/set year
        /// </summary>
        public string Year
        {
            get
            {
                if (year != null)
                    return year;
                else return "- ND -";
            }

            set
            {
                year = value;
            }
        }

        /// <summary>
        /// get/set url of the cover
        /// </summary>
        public string ImageURL
        {
            get
            {
                if (imageURL != null)
                    return formatImageURL(imageURL);
                else return "- ND -";
            }

            set
            {
                imageURL = value;
            }
        }

        /// <summary>
        /// get/set rate of the site
        /// </summary>
        public string SiteRate
        {
            get
            {
                if (siteRate != null)
                    return siteRate;
                else return "- ND -";
            }

            set
            {
                siteRate = value;
            }
        }

        /// <summary>
        /// get/set director
        /// </summary>
        public string Director
        {
            get
            {
                if (director != null)
                    return director;
                else return "- ND -";
            }

            set
            {
                director = value;
            }
        }

        /// <summary>
        /// get/set array of genres
        /// </summary>
        public string[] Genres
        {
            get
            {
                if (genres != null)
                    return genres;
                else return null;
            }

            set
            {
                genres = value;
            }
        }

        /// <summary>
        /// get/set tagline
        /// </summary>
        public string Tagline
        {
            get
            {
                if (tagline != null)
                    return tagline;
                else return "- ND -";
            }

            set
            {
                tagline = value;
            }
        }

        /// <summary>
        /// get/set plot
        /// </summary>
        public string Description
        {
            get
            {
                if (description != null)
                    return description;
                else return "- ND -";
            }

            set
            {
                description = value;
            }
        }

        /// <summary>
        /// get/set array of actors
        /// </summary>
        public string[] Actors
        {
            get
            {
                if (actors != null)
                    return actors;
                else return null;
            }

            set
            {
                actors = value;
            }
        }

        /// <summary>
        /// get/set runtime
        /// </summary>
        public string RunningTime
        {
            get
            {
                if (runningTime != null)
                    return runningTime;
                else return "- ND -";
            }

            set
            {
                runningTime = value;
            }
        }

        /// <summary>
        /// Returns a string with all the info of the title, to be displayed on a textbox
        /// </summary>
        /// <returns>String with title infos</returns>
        public string toString()
        {
            string ret = "link: " + link + "\r\n titulo: " + titulo + "\r\n ano: " + year
            + "\r\n image url: " + imageURL + "\r\n rate: " + siteRate + "\r\n director: " + director;
            if (genres != null)
            {
                for (int i = 0; i < genres.Length; i++)
                {
                    if (genres[i] != "")
                    {
                        ret += "\r\ngenre" + (i + 1) + ": " + genres[i];
                    }
                }
            }
            ret += "\r\n tagline: " + tagline +"\r\n plot: " + description;
            if (actors != null)
            {
                for (int i = 0; i < actors.Length; i++)
                {
                    if (actors[i] != "")
                    {
                        ret += "\r\n actor" + (i + 1) + ": " + actors[i];
                    }
                }
            }
            ret += "\r\n runtime: " + runningTime;
            return ret;
        }

        /// <summary>
        /// To get the link to a cover with bigger height and width
        /// </summary>
        /// <param name="url">The link returned by IMDb</param>
        /// <returns>The correct link</returns>
        /// <remarks>You can change the size as you want, in the condition
        /// of maintaining the aspect ratio. In this code the cover
        /// will have 231px of width and 333px of height</remarks>
        private string formatImageURL(string url)
        {
            int bSX = url.IndexOf("SX");
            string tempSX = url.Substring(bSX);
            int fSX = tempSX.IndexOf("_");
            tempSX = tempSX.Substring(0, fSX);
            url = url.Replace(tempSX, "SX231");

            int bSY = url.IndexOf("SY");
            string tempSY = url.Substring(bSY);
            int fSY = tempSY.IndexOf("_");
            tempSY = tempSY.Substring(0, fSY);
            url = url.Replace(tempSY, "SY333");

            return url;
        }
    }
}
