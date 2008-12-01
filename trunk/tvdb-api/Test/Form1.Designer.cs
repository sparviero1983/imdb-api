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

namespace Test
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lvSearchResult = new System.Windows.Forms.ListView();
            this.chId = new System.Windows.Forms.ColumnHeader();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chOverview = new System.Windows.Forms.ColumnHeader();
            this.chLang = new System.Windows.Forms.ColumnHeader();
            this.chBanner = new System.Windows.Forms.ColumnHeader();
            this.chIMDB = new System.Windows.Forms.ColumnHeader();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "English",
            "Dansk",
            "Suomeksi",
            "Dutch",
            "Deutsch",
            "Italiano",
            "Español",
            "Français",
            "Polski",
            "Magyar",
            "Ελληνικά",
            "Türkçe",
            "русский язык",
            "עברית",
            "日本語",
            "Português",
            "中文",
            "čeština",
            "Slovenski",
            "English",
            "Svenska",
            "Norsk"});
            this.comboBox1.Location = new System.Drawing.Point(12, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // lvSearchResult
            // 
            this.lvSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chId,
            this.chName,
            this.chOverview,
            this.chLang,
            this.chBanner,
            this.chIMDB});
            this.lvSearchResult.FullRowSelect = true;
            this.lvSearchResult.Location = new System.Drawing.Point(162, 27);
            this.lvSearchResult.MultiSelect = false;
            this.lvSearchResult.Name = "lvSearchResult";
            this.lvSearchResult.Size = new System.Drawing.Size(908, 469);
            this.lvSearchResult.TabIndex = 1;
            this.lvSearchResult.UseCompatibleStateImageBehavior = false;
            this.lvSearchResult.View = System.Windows.Forms.View.Details;
            this.lvSearchResult.DoubleClick += new System.EventHandler(this.lvSearchResult_DoubleClick);
            // 
            // chId
            // 
            this.chId.Text = "Id";
            this.chId.Width = 61;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 198;
            // 
            // chOverview
            // 
            this.chOverview.Text = "Overview";
            this.chOverview.Width = 351;
            // 
            // chLang
            // 
            this.chLang.Text = "Language";
            this.chLang.Width = 68;
            // 
            // chBanner
            // 
            this.chBanner.Text = "Banner";
            this.chBanner.Width = 158;
            // 
            // chIMDB
            // 
            this.chIMDB.Text = "IMDB_ID";
            this.chIMDB.Width = 66;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 88);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(2, 502);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1068, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(12, 150);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(55, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "actors";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(12, 173);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(64, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "banners";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(12, 127);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(48, 17);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "serie";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 533);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lvSearchResult);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListView lvSearchResult;
        private System.Windows.Forms.ColumnHeader chId;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader chLang;
        private System.Windows.Forms.ColumnHeader chBanner;
        private System.Windows.Forms.ColumnHeader chOverview;
        private System.Windows.Forms.ColumnHeader chIMDB;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}

