using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace IMDBDLL
{
    /// <summary>
    /// interface to access the api
    /// </summary>
    public interface IIMDb
    {
        /// <summary>
        /// Executes the search by title in IMDB.
        /// </summary>
        /// <param name="title">title to be searched</param>
        /// <returns>if the search was successful</returns>
        bool searchByTitle(string title);
        /// <summary>
        /// Executes the search by ID in IMDB.
        /// </summary>
        /// <param name="ID">id to be searched</param>
        /// <returns>if the search was successful</returns>
        bool searchByID(string ID);
        /// <summary>
        /// Gets the page type
        /// </summary>
        /// <param name="tipo">if is to search a movie title or a tv serie title
        /// 0- movie
        /// 1- tv serie
        /// </param>
        /// <returns>If the page is a result list page - 1
        /// or the actual result - 0</returns>
        int getType(int tipo);
        /// <summary>
        /// Parses the search results, to get the links
        /// </summary>
        /// <returns>Array of links</returns>
        ArrayList parseSearch();
        /// <summary>
        /// Parses the title page to get info from a movie or tv show
        /// </summary>
        /// <param name="fields">Fields to parse</param>
        void parseTitlePage(bool[] fields);
        /// <summary>
        /// returns the object with info of the title
        /// </summary>
        /// <returns>the object that contains the info fetched from the site</returns>
        Title getTitle();
    }
    /// <summary>
    /// interface to access the api thread
    /// </summary>
    public interface IIMDbT
    {
        /// <summary>
        /// Parses the title page to get info from a movie or tv show
        /// </summary>
        void parseTitlePage();
        /// <summary>
        /// returns the object with info of the title
        /// </summary>
        /// <returns>the object that contains the info fetched from the site</returns>
        Title getTitle();
    }
}
