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
                else return "- NA -";
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
                else return "- NA -";
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
                else return "- NA -";
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
                    return imageURL;
                else return "- NA -";
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
                else return "- NA -";
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
                else return "- NA -";
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
                else return "- NA -";
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
                else return "- NA -";
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
                else return "- NA -";
            }

            set
            {
                runningTime = value;
            }
        }

        /// <summary>
        /// Returns a string with all the info of the title, to be displayed on a textbox
        /// </summary>
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
    }
}
