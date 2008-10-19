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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace IMDBDLL.MultiThreadEngine
{
    /// <summary>
    /// Class for the Multi Thread Manager.
    /// </summary>
    public class MTManager
    {
        /// <summary>
        /// Number of threads created.
        /// </summary>
        private int numWorkers;

        /// <summary>
        /// Number of actors to parse.
        /// </summary>
        private int numActors;

        /// <summary>
        /// Type of the titles to be parsed.
        /// </summary>
        private int media;

        /// <summary>
        /// Array that holds all the workers.
        /// </summary>
        private MTWorker[] workers;

        /// <summary>
        /// Array that will hold the results.
        /// </summary>
        private ArrayList[] result;

        /// <summary>
        /// Array that holds information on wich fields are to be parsed.
        /// </summary>
        private bool[] fields;

        /// <summary>
        /// Array that holds the links of the titles to be parsed.
        /// </summary>
        private List<String> links;

        /// <summary>
        /// Boolean that says if there is any worker working.
        /// </summary>
        private bool workersWorking = false;

        /// <summary>
        /// Boolean that says that if there was an error.
        /// </summary>
        private bool error = false;

        /// <summary>
        /// Delegate to call the caller form function.
        /// </summary>
        public Delegate parentFormCaller;

        /// <summary>
        /// Delegate to update the progress bar.
        /// </summary>
        public Delegate parentProgressCaller;

        /// <summary>
        /// Delegate to call the caller form error handler.
        /// </summary>
        public Delegate parentFormErrorCaller;

        /// <summary>
        /// Constructor of the manager.
        /// </summary>
        /// <param name="l">Links to connect to.</param>
        /// <param name="f">Fields to be parsed.</param>
        /// <param name="nActors">Number of actors to parse.</param>
        /// <param name="m">Type of title to parse.</param>
        public MTManager(List<String> l, bool[] f, int nActors, int m)
        {
            numWorkers = l.Count;
            links = l;
            fields = f;
            numActors = nActors;
            media = m;
        }

        /// <summary>
        /// The manager start function.
        /// </summary>
        public void startManager()
        {
            workers = new MTWorker[numWorkers]; // Creates the array to hold the workers.
            result = new ArrayList[numWorkers]; // Creates the array to hold the results.
            for (int i = 0; i < numWorkers; i++)
            {
                workers[i] = new MTWorker(i); // Creates a new worker with id=i.
                configureWorker(workers[i]); // Sets some properties to the worker. 
            }
            AssignWorkers(); // Distribute work to each worker.
        }

        /// <summary>
        /// Function to distibute works to each worker.
        /// </summary>
        private void AssignWorkers()
        {
            foreach(MTWorker worker in workers) {
                if (!worker.IsBusy)
                {
                    // Create the structure to hold the parameters for the worker. (Struct is defined in MTWorker.cs)
                    Params param = new Params();
                    param.actorN = numActors;
                    param.fields = fields;
                    param.media = media;
                    param.url = links[worker.WorkerID];

                    worker.RunWorkerAsync(param); // Make the worker start his job.
                }
            }
        }

        /// <summary>
        /// Set some properties to the workers.
        /// </summary>
        /// <param name="MTW">The worker.</param>
        private void configureWorker(MTWorker MTW)
        {
            MTW.ProgressChanged += MTWorker_ProgressChanged;
            MTW.RunWorkerCompleted += MTWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Function to handle the Progress Report from a worker.
        /// </summary>
        /// <param name="sender">The worker who raised the event.</param>
        /// <param name="e">The argument of the event.</param>
        private void MTWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            parentProgressCaller.DynamicInvoke(new Object[] { e.ProgressPercentage }); // Call the progressUpdater function located in the parent form.
        }

        /// <summary>
        /// Function to handle the end of work from a worker.
        /// </summary>
        /// <param name="sender">The worker who raised the event.</param>
        /// <param name="e">The argument of the event.</param>
        private void MTWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MTWorker worker = sender as MTWorker; // Worker that raised the event.
            ArrayList res; // Arraylist that holds the worker results of the parsing.
            if (e.Error != null) // If there was an error during the parse.
            {
                if (!error)
                {
                    error = true;
                    foreach (MTWorker w in workers) // Cancel all the workers still running.
                    {
                        if (w.IsBusy)
                        {
                            w.WorkerSupportsCancellation = true;
                            w.CancelAsync();
                        }
                    }
                    parentFormErrorCaller.DynamicInvoke(new Object[] { e.Error.Message }); // Tell the parent Form there was an error.
                }
            }
            else
            {
                res = (ArrayList)e.Result; // get the result from the worker.
                result[worker.WorkerID] = res; // add it to the results array.
            }

            if (!error)
            {
                workersWorking = false;

                foreach (MTWorker w in workers) //Checks if there are still workers working.
                {
                    if (w.IsBusy)
                        workersWorking = true;
                }
                if (!workersWorking) // if not, call the function that will process the results, located in the caller form.
                {
                    parentFormCaller.DynamicInvoke(new Object[] { result });
                }
            }
        }
    }
}
