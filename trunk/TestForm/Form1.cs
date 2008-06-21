/* To make use of all the functionalities of the API
 * you must implement all of the BackgroundWorker event handlers
 * methods shown here.
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMDBDLL;

namespace TestForm
{
    /// <summary>
    /// Test form
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Array of the workers
        /// </summary>
        public ArrayList workers = new ArrayList();
        /// <summary>
        /// Array to hold all the IMDb object created, to fetch their titles after the work is done
        /// </summary>
        public ArrayList imdbs = new ArrayList();
        /// <summary>
        /// Array to hold the links that were parsed by the IMDb object
        /// </summary>
        public ArrayList links;
        /// <summary>
        /// Quantity of workers running
        /// </summary>
        public int works;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Necessary method to give event handlers to the worker
        /// </summary>
        /// <param name="worker">Worker that will get the events</param>
        public void InitializeBackgoundWorker(BackgroundWorker worker)
        {
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
        }
        /// <summary>
        /// Defines the work to be done when the worker starts
        /// </summary>
        /// <param name="sender">Worker who raised the event</param>
        /// <param name="e">Wich IMDb the worker "owns"</param>
        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.DoEvents();
            BackgroundWorker worker = sender as BackgroundWorker;
            IMDB imdb = e.Argument as IMDB;
            Application.DoEvents();
            int tipo;
            if (radioButton4.Checked) tipo = 0;
            else tipo = 1;
            imdb.parseTitlePage(null, null, worker, tipo);
            Application.DoEvents();
        }
        /// <summary>
        /// Updates the progress bar
        /// </summary>
        /// <param name="sender">Worker who raised the event</param>
        /// <param name="e">Event argument that contains the amount of work done</param>
        public void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value += e.ProgressPercentage;
        }
        /// <summary>
        /// What to do when a worker finishes his work
        /// </summary>
        /// <param name="sender">Worker who raised the event</param>
        /// <param name="e">Event argument</param>
        public void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            works--;
        }
        /// <summary>
        /// Button click handler, to start the workers
        /// </summary>
        /// <param name="sender">Button that raised the event</param>
        /// <param name="e">Event argument</param>
        public void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Cursor = Cursors.WaitCursor;
            BackgroundWorker Worker;
            textBox2.Text = "";
            if (textBox1.Text != "")
            {
                IMDB imdb = new IMDB();
                bool success;
                if (radioButton1.Checked)
                {
                    success = imdb.searchByTitle(textBox1.Text);
                }
                else
                {
                    success = imdb.searchByID(textBox1.Text);
                }
                if (success)
                {
                    int type;
                    if(radioButton4.Checked)
                        type = imdb.getType(0);
                    else type = imdb.getType(1);
                    if (type == 0)
                    {
                        progressBar1.Maximum = 20;
                        progressBar1.Value = 10;
                        Worker = new BackgroundWorker();
                        Worker.WorkerReportsProgress = true;
                        InitializeBackgoundWorker(Worker);
                        Application.DoEvents();
                        workers.Add(Worker);
                        works = 1;
                        imdbs.Add(imdb);
                        Worker.RunWorkerAsync(imdb);
                        Application.DoEvents();
                    }
                    else if(type == 1) {
                        links = imdb.parseSearch();
                        progressBar1.Value = 10;
                        if (links != null)
                        {
                            progressBar1.Maximum = 10*links.Count+10;
                            for (int i = 0; i < links.Count; i++)
                            {
                                Application.DoEvents();
                                IMDB imdb2 = new IMDB();
                                string id = ((string)links[i]);
                                id = id.Substring(id.Length-10,9);
                                imdb2.searchByID(id);
                                Worker = new BackgroundWorker();
                                Application.DoEvents();
                                Worker.WorkerReportsProgress = true;
                                InitializeBackgoundWorker(Worker);
                                workers.Add(Worker);
                                works += 1;
                                imdbs.Add(imdb2);
                                Worker.RunWorkerAsync(imdb2);
                                Application.DoEvents();
                            }
                        }
                    }
                }
                while (works > 0)
                {
                    Application.DoEvents();
                }
                foreach (IMDB i in imdbs)
                {
                    if(i.getTitle()!=null)
                        textBox2.Text += i.getTitle().toString()+"\r\n\r\n";
                }
                Cursor = Cursors.Default;
                button1.Enabled = true; ;
            }
        }
    }
}
