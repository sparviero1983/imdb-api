using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using IMDBDLL;

namespace Test
{
    class Program
    {
        /// <summary>
        /// Constructor of the class
        /// </summary>
        public Program()
        {
            IIMDb imdb = new IMDB();
            //make a search by movie
            bool success = imdb.searchByTitle("matrix");
            //search by id bool success = imdb.searchByID("tt0133093");
            if (success)
            {
                //if the search returns a list of results
                if (imdb.getType(0) == 1)
                {
                    //get the links to the movies of the results
                    ArrayList links = imdb.parseSearch();
                    if (links != null)
                    {
                        ArrayList res = new ArrayList();
                        //to wait all the threads to finish
                        WaitHandle[] waitHandles = new WaitHandle[links.Count];
                        IIMDbT imdbT;
                        //one thread for each result
                        for (int i = 0; i < links.Count; i++)
                        {
                            try
                            {
                                AutoResetEvent aRE = new AutoResetEvent(false);
                                waitHandles[i] = aRE;
                                //the last parameter is to ddefine if it is to search a movie or a tv serie
                                //0 for movies
                                //1 for tv series
                                imdbT = new IMDBThread((string)links[i], aRE, 0);
                                Thread thread = new Thread(
                                new ThreadStart(imdbT.parseTitlePage));
                                res.Add(imdbT);
                                thread.Start();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        WaitAll(waitHandles);
                        foreach (IIMDbT iT in res)
                        {
                            //prints the movie info
                            iT.getTitle().toString();
                            //iT.getTitle().Titulo returns the title of the movie
                        }
                    }
                }
                else //if returns the correct movie web page
                {
                    imdb.parseTitlePage();
                    //prints the movie info
                    imdb.getTitle().toString();
                }
            }
        }

        /// <summary>
        /// start point of the program
        /// </summary>
        /// <param name="args">arguments of the program</param>
        static void Main(string[] args)
        {
            new Program();
        }

        /// <summary>
        /// This will make the program to wait multiple threads to finish before going on
        /// </summary>
        /// <param name="waitHandles"></param>
        private void WaitAll(WaitHandle[] waitHandles)
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                // WaitAll for multiple handles on an STA thread is not supported.
                // ...so wait on each handle individually.
                foreach (WaitHandle myWaitHandle in waitHandles)
                {
                    WaitHandle.WaitAny(new WaitHandle[] { myWaitHandle });
                }
            }
            else
            {
                WaitHandle.WaitAll(waitHandles);
            }
        }
    }
}
