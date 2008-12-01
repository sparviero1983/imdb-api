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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TVDBDLL;

namespace Test
{
    /// <summary>
    /// Main form
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Delegate to call the dataReceiverHandler.
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <param name="type">The Type of the data.</param>
        public delegate void dataCall(object data, int type);

        /// <summary>
        /// Delegate to call the errorHandler.
        /// </summary>
        /// <param name="exc">The Exception.</param>
        public delegate void errorCall(Exception exc);

        /// <summary>
        /// Delegate to call the progressUpdater.
        /// </summary>
        /// <param name="value">Value to add to the progress bar.</param>
        public delegate void progressCall(int value);

        /// <summary>
        /// Event of errorCall.
        /// </summary>
        private event errorCall formErrorCaller;

        /// <summary>
        /// Event of dataReceivedCall.
        /// </summary>
        private event dataCall formDataCaller;

        /// <summary>
        /// Event of progressCall.
        /// </summary>
        private event progressCall formProgressCaller;

        /// <summary>
        /// Object that holds reference to tvdb connection.
        /// </summary>
        private TVDB tvDbHandler;

        /// <summary>
        /// Constructor of the form.
        /// </summary>
        public Form1()
        {
            formErrorCaller += new errorCall(errorHandler);
            formProgressCaller += new progressCall(progressUpdater);
            formDataCaller += new dataCall(dataReceiver);

            InitializeComponent();
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        /// <summary>
        /// Change the progress bar value.
        /// </summary>
        /// <param name="value">Value to add to the current value.</param>
        public void progressUpdater(int value)
        {
            progressBar1.Value = value;
        }

        /// <summary>
        /// Receives data from the tvdbDLL.
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <param name="type">The Type of the data.</param>
        public void dataReceiver(object data, int type)
        {
            if (type == 0)//all
            {
                List<String> results = (List<String>) data;
                Console.WriteLine(results[0]);//serie info
                Console.WriteLine(results[1]);//actors info
                Console.WriteLine(results[2]);//banners info
            }
            else if (type == 1)// serie and actors 
            {
                List<String> results = (List<String>)data;
                Console.WriteLine(results[0]);//serie info
                Console.WriteLine(results[1]);//actors info
            }
            else if (type == 2)// serie and banners 
            {
                List<String> results = (List<String>)data;
                Console.WriteLine(results[0]);//serie info
                Console.WriteLine(results[2]);//banners info
            }
            else if (type == 3)// serie 
            {
                List<String> results = (List<String>)data;
                Console.WriteLine(results[0]);//serie info
            }
            else if (type == 4)// actors
            {
                List<String> results = (List<String>)data;
                Console.WriteLine(results[1]);//actors info
            }
            else if (type == 5)//banners 
            {
                List<String> results = (List<String>)data;
                Console.WriteLine(results[2]);//banners info
            }
            else if (type == 5)//actors and banners 
            {
                List<String> results = (List<String>)data;
                Console.WriteLine(results[1]);//actors info
                Console.WriteLine(results[2]);//banners info
            }
        }

        /// <summary>
        /// Displays a message box with the error that occured.
        /// </summary>
        /// <param name="exc">Exception occured.</param>
        public void errorHandler(Exception exc)
        {
            button1.Enabled = true;
            Cursor = Cursors.Default;
            progressBar1.Value = 0;
            MessageBox.Show(exc.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Function that handles the search button click. It is here that the API is called.
        /// </summary>
        /// <param name="sender">default argument.</param>
        /// <param name="e">default argument.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            progressBar1.Value = 0;
            lvSearchResult.Items.Clear();
            tvDbHandler = new TVDB();

            tvDbHandler.parentErrorCaller = formErrorCaller;
            tvDbHandler.parentProgressUpdaterCaller = formProgressCaller;
            tvDbHandler.parentDataReceiverCaller = formDataCaller;
            String lang = getLanguage();
            List<ResultItem> results = tvDbHandler.searchSerie(lang, textBox1.Text);

            foreach (ResultItem r in results)
            {
                ListViewItem item = new ListViewItem(r.ID);
                item.SubItems.Add(r.SerieName);
                item.SubItems.Add(r.Overview);
                item.SubItems.Add(r.Language);
                item.SubItems.Add(r.Banner);
                item.SubItems.Add(r.IMDB_ID);
                item.Tag = r;
                lvSearchResult.Items.Add(item);
            }
            button1.Enabled = true;
        }

        /// <summary>
        /// Changes the selected language to the site's format
        /// </summary>
        /// <returns>The language in the site's format</returns>
        private String getLanguage()
        {
            String l = comboBox1.Text;
            if (l == "English")
            {
                return "en";
            }
            else if (l == "Dansk")
            {
                return "da";
            }
            else if (l == "Suomeksi")
            {
                return "fi";
            }
            else if (l == "Dutch")
            {
                return "nl";
            }
            else if (l == "Deutsch")
            {
                return "de";
            }
            else if (l == "Italiano")
            {
                return "it";
            }
            else if (l == "Español")
            {
                return "es";
            }
            else if (l == "Français")
            {
                return "fr";
            }
            else if (l == "Polski")
            {
                return "pl";
            }
            else if (l == "Magyar")
            {
                return "hu";
            }
            else if (l == "Ελληνικά")
            {
                return "el";
            }
            else if (l == "Türkçe")
            {
                return "tr";
            }
            else if (l == "русский язык")
            {
                return "ru";
            }
            else if (l == "עברית")
            {
                return "he";
            }
            else if (l == "日本語")
            {
                return "ja";
            }
            else if (l == "Português")
            {
                return "pt";
            }
            else if (l == "中文")
            {
                return "zh";
            }
            else if (l == "čeština")
            {
                return "cs";
            }
            else if (l == "Slovenski")
            {
                return "sl";
            }
            else if (l == "Svenska")
            {
                return "sv";
            }
            else if (l == "Norsk")
            {
                return "no";
            }
            return "en";
        }

        /// <summary>
        /// Here we fetch the info of selected serie
        /// </summary>
        /// <param name="sender">default argument.</param>
        /// <param name="e">default argument.</param>
        private void lvSearchResult_DoubleClick(object sender, EventArgs e)
        {
            if (lvSearchResult.SelectedItems.Count == 1)
            {
                ResultItem selected = (ResultItem)lvSearchResult.SelectedItems[0].Tag;
                tvDbHandler.DownloadSerieZipped(selected.ID, checkBox3.Checked, checkBox1.Checked, checkBox2.Checked);
            }
        }
    }
}
