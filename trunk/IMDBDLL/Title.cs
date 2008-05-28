using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMDBDLL
{
    public class Title
    {
        private string link, titulo, year, imageURL, siteRate, director, tagline, description, runningTime;
        private string[] genres, actors;

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
            Console.WriteLine("description: " + description);
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
