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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace IMDBDLL.MultiThreadEngine
{
    /// <summary>
    /// Structure that represents the parameters received by the worker
    /// </summary>
    struct Params
    {
        public String url;
        public bool[] fields;
        public int media;
        public int actorN;
        public int sSeas;
        public int eSeas;
        public XmlDocument xmlDoc;
    }

    /// <summary>
    /// Class that represents a Single BackgroundWorker.
    /// </summary>
    public class MTWorker : BackgroundWorker
    {
        /// <summary>
        /// The ID of the Worker
        /// </summary>
        private int workerID;
        /// <summary>
        /// Returns the ID of the worker
        /// </summary>
        public int WorkerID { get { return workerID; } }

        /// <summary>
        /// Event that raises the progress updater caller.
        /// </summary>
        private event progressCall progressCaller;

        /// <summary>
        /// Event that raises the error caller.
        /// </summary>
        private event errorCall errorCaller;

        /// <summary>
        /// Delegate that handles error calls.
        /// </summary>
        /// <param name="exc">Exception that occured.</param>
        public delegate void errorCall(Exception exc);

        /// <summary>
        /// Delegate that handles progress update calls.
        /// </summary>
        /// <param name="value">value to add to actual progress value.</param>
        public delegate void progressCall(int value);

        /// <summary>
        /// Delegate that calls the parent error handler.
        /// </summary>
        public Delegate parentErrorCaller;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MTWorker()
        {
            WorkerReportsProgress = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">The ID of the worker.</param>
        public MTWorker(int id)
        {
            workerID = id;
            progressCaller += new progressCall(progressHandler);
            errorCaller += new errorCall(errorHandler);
        }

        /// <summary>
        /// Override the function from the BackgroundWorker Class
        /// </summary>
        /// <param name="e">The object that we want to be processed.</param>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            WorkerReportsProgress = true;
            String url = ((Params)e.Argument).url;
            bool[] fields = ((Params)e.Argument).fields;
            int media = ((Params)e.Argument).media;
            int actorN = ((Params)e.Argument).actorN;
            int sSeas = ((Params)e.Argument).sSeas;
            int eSeas = ((Params)e.Argument).eSeas;
            XmlDocument xmlDoc = ((Params)e.Argument).xmlDoc;
            IMDB imdb = new IMDB();
            imdb.parentProgressCaller = progressCaller;
            imdb.parentErrorCaller = errorCaller;

            bool success = imdb.getPage(url);

            if (success)
            {
                this.ReportProgress(40);
                imdb.parseTitlePage(xmlDoc, fields, media, actorN, sSeas, eSeas);
            }
        }

        private void progressHandler(int value)
        {
            this.ReportProgress(value);
        }

        private void errorHandler(Exception exc)
        {
            parentErrorCaller.DynamicInvoke(new object[] { exc });
        }
    }
}
