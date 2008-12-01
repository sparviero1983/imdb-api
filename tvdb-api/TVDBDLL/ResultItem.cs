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

namespace TVDBDLL
{
    /// <summary>
    /// Class that represents one result fetched from the site
    /// </summary>
    public class ResultItem
    {
        /// <summary>
        /// id of the serie
        /// </summary>
        private String id;

        /// <summary>
        /// Serie name
        /// </summary>
        private String serieName;

        /// <summary>
        /// Overview of the serie
        /// </summary>
        private String overview;

        /// <summary>
        /// Language of the serie's info
        /// </summary>
        private String language;

        /// <summary>
        /// Main banner of the serie
        /// </summary>
        private String banner;

        /// <summary>
        /// IMDB ID of the serie
        /// </summary>
        private String ImdbId;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sN">name</param>
        /// <param name="l">language</param>
        /// <param name="o">overview</param>
        /// <param name="b">banner</param>
        /// <param name="imdb">imdb id</param>
        public ResultItem(String id, String sN, String l, String o, String b, String imdb)
        {
            this.id = id;
            this.serieName = sN;
            this.language = l;
            this.overview = o;
            this.banner = b;
            this.ImdbId = imdb;
        }

        /// <summary>
        /// getter for id
        /// </summary>
        public String ID
        {
            get { return id; }
        }

        /// <summary>
        /// getter for imdb id
        /// </summary>
        public String IMDB_ID
        {
            get { return ImdbId; }
        }

        /// <summary>
        /// getter for main banner
        /// </summary>
        public String Banner
        {
            get { return banner; }
        }

        /// <summary>
        /// getter for name
        /// </summary>
        public String SerieName
        {
            get { return serieName; }
        }

        /// <summary>
        /// getter for language
        /// </summary>
        public String Language
        {
            get { return language; }
        }

        /// <summary>
        /// getter for overview
        /// </summary>
        public String Overview
        {
            get { return overview; }
        }
    }
}
