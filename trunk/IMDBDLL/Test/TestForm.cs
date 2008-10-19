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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using IMDBDLL;
using IMDBDLL.MultiThreadEngine;

namespace Test
{
    /// <summary>
    /// Class that tests the API and shows how to use it.
    /// </summary>
    public partial class TestForm : Form
    {
        /// <summary>
        /// Delegate to call the processResults.
        /// </summary>
        /// <param name="result">The results.</param>
        public delegate void functionCall(ArrayList[] result);

        /// <summary>
        /// Delegate to call the errorHandler.
        /// </summary>
        /// <param name="error">The error message.</param>
        public delegate void errorCall(String error);

        /// <summary>
        /// Delegate to call the progressUpdater.
        /// </summary>
        /// <param name="value">Value to add to the progress bar.</param>
        public delegate void progressCall(int value);

        /// <summary>
        /// Event of functionCall.
        /// </summary>
        private event functionCall formFunctionCaller;

        /// <summary>
        /// Event of errorCall.
        /// </summary>
        private event errorCall formErrorCaller;

        /// <summary>
        /// Event of progressCall.
        /// </summary>
        private event progressCall formProgressCaller;

        /// <summary>
        /// Execution Start Time.
        /// </summary>
        private DateTime ExecutionStartTime;

        /// <summary>
        /// Execution Stop Time.
        /// </summary>
        private DateTime ExecutionStopTime;

        /// <summary>
        /// Total Execution Time.
        /// </summary>
        private TimeSpan ExecutionTime;

        /// <summary>
        /// Constructor
        /// </summary>
        public TestForm()
        {
            InitializeComponent();

            //Adds the events
            formFunctionCaller += new functionCall(processResult); 
            formErrorCaller += new errorCall(errorHandler);
            formProgressCaller += new progressCall(progressUpdater);
        }

        /// <summary>
        /// Displays a message box with the error that occured.
        /// </summary>
        /// <param name="error">Message of the error.</param>
        public void errorHandler(String error)
        {
            button1.Enabled = true;
            Cursor = Cursors.Default;
            progressBar1.Value = 0;
            MessageBox.Show(error, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Change the progress bar value.
        /// </summary>
        /// <param name="value">Value to add to the current value.</param>
        public void progressUpdater(int value)
        {
            progressBar1.Value += value;
        }

        /// <summary>
        /// Here we do whatever we want with the info.
        /// </summary>
        /// <param name="result">The info from the titles parsed.</param>
        public void processResult(ArrayList[] result)
        {
            foreach (ArrayList arr in result) // For each title parsed.
            {
                if (arr.Count != 0)// If there are infos.
                {
                    textBox2.Text += "**Title: " + arr[1] + "\r\n";
                    textBox2.Text += "**Link: " + arr[0] + "\r\n"; 
                    textBox2.Text += "**Year: " + arr[2] + "\r\n"; 
                    textBox2.Text += "**Cover: " + arr[3] + "\r\n";
                    textBox2.Text += "**User rating: " + arr[4] + "\r\n";
                    textBox2.Text += "**Director/creator: " + arr[5] + "\r\n";
                    textBox2.Text += "**Genres:" + "\r\n";
                    List<String> gens = (List<String>)arr[6];
                    if (gens != null)
                        foreach (String s in gens)
                            textBox2.Text += "- " + s + "\r\n";
                    textBox2.Text += "**Tagline: " + arr[7] + "\r\n";
                    textBox2.Text += "**Plot: " + arr[8] + "\r\n";
                    textBox2.Text += "**Actors info: " + "\r\n";
                    List<String> acts = (List<String>)arr[9];
                    if (acts != null)
                        foreach (String s in acts)
                            textBox2.Text += "- " + s + "\r\n";
                    textBox2.Text += "**Runtime: " + arr[10] + "\r\n";
                    textBox2.Text += "*******************************************\r\n";
                }
                
            }
            //Here we calculate the time of execution and displays that info. (for debug)
            ExecutionStopTime = DateTime.Now;
            ExecutionTime = ExecutionStopTime - ExecutionStartTime;
            textBox2.Text += String.Format("Time of execution: {0:0.00} seconds", ExecutionTime.TotalSeconds.ToString());
            
            button1.Enabled = true;
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Function that handles the search button click. It is here that the API is called.
        /// </summary>
        /// <param name="sender">default argument.</param>
        /// <param name="e">default argument.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            String text = textBox1.Text;
            if (text != "") // if there is text in the textBox.
            {
                Cursor = Cursors.WaitCursor;
                button1.Enabled = false;
                textBox2.Text = "";
                progressBar1.Value = 0;
                progressBar1.Maximum = 1000;
                ExecutionStartTime = DateTime.Now; //Starts the clock.
                
                // Starts the imdb class
                IMDB imdb = new IMDB();
                String success = "", url = "";
                int type = -1, media = 0;
                bool titleR = radioButton1.Checked, serieR = radioButton4.Checked;
                ArrayList results;

                if (titleR) //if its to search by title
                    url = "http://www.imdb.com/find?s=all&q=" + text;
                else // or by ID
                    url = "http://www.imdb.com/title/" + text + "/";

                success = imdb.getPage(url); // Connects to IMDb and gets the html page

                if(serieR) // If it's a TV serie we are looking for
                    media = 1;

                if (success == "OK" && titleR) // If we could get the page and searching by title
                {
                    type = imdb.getPageType(media, text); // Verify if it's a specific title page or a search result page
                    progressUpdater(100);
                }
                else if (success != "OK") // If there was an error fetching the html page
                {
                    errorHandler(success);
                }

                bool[] fields = { true, true, true, true, true, true, true, true, true, true }; //Parses all the fields.

                if (type == 0)
                {
                    List<String> links = new List<String>();
                    links = imdb.parseTitleLinks(); // Gets the relevant links from that page
                    if (links.Count != 0)
                    {
                        String l = "http://www.imdb.com";

                        for (int i = 0; i < links.Count; i++)
                        {
                            links[i] = l + links[i];
                        }
                        progressBar1.Maximum = 100 + links.Count * 100;

                        //Here we start the thread manager
                        MTManager MTM = new MTManager(links, fields, 5, media);
                        MTM.parentFormCaller = formFunctionCaller;
                        MTM.parentFormErrorCaller = formErrorCaller;
                        MTM.parentProgressCaller = formProgressCaller;
                        MTM.startManager();
                    }
                    else
                    {
                        errorHandler("No Results found");
                    }
                }
                else
                {
                    results = imdb.parseTitlePage(fields, media, 5);
                    processResult(new ArrayList[] { results });
                }    
            }
        }

        /// <summary>
        /// Detect if the enter key was pressed in the textbox.
        /// </summary>
        /// <param name="sender">default argument.</param>
        /// <param name="e">default argument.</param>
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, null);
        }
    }
}
