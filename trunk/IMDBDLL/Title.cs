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
                return titulo;
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
                return link;
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
                return year;
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
                return imageURL;
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
                return siteRate;
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
                return director;
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
                return genres;
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
                return tagline;
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
                return description;
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
                return actors;
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
                return runningTime;
            }

            set
            {
                runningTime = value;
            }
        }

        /// <summary>
        /// prints all the info of the title
        /// </summary>
        public void toString()
        {
            Console.WriteLine("link: "+link);
            Console.WriteLine("titulo: " + titulo);
            Console.WriteLine("ano: " + year);
            Console.WriteLine("image url: " + imageURL);
            Console.WriteLine("rate: " + siteRate);
            Console.WriteLine("director: "+director);
            if (genres != null)
            {
                for (int i = 0; i < genres.Length; i++)
                {
                    if (genres[i] != "")
                    {
                        Console.WriteLine("genre" + (i + 1) + ": " + genres[i]);
                    }
                }
            }
            Console.WriteLine("tagline: " + tagline);
            Console.WriteLine("plot: " + description);
            if (actors != null)
            {
                for (int i = 0; i < actors.Length; i++)
                {
                    if (actors[i] != "")
                    {
                        Console.WriteLine("actor" + (i + 1) + ": " + actors[i]);
                    }
                }
            }
            Console.WriteLine("runtime: " + runningTime);
        }
    }
}
