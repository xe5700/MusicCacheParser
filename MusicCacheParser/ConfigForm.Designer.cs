namespace MusicCacheParser
{
    partial class ConfigForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.neteaseCachePath = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.savePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFilename = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tempPath = new System.Windows.Forms.Label();
            this.neteaseAutoParse = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 397);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(776, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "保存他娘的设置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 80);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "保存格式";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 13);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(764, 61);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.neteaseAutoParse);
            this.groupBox2.Controls.Add(this.neteaseCachePath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 222);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(775, 76);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "网易云音乐设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "缓存目录";
            // 
            // neteaseCachePath
            // 
            this.neteaseCachePath.Location = new System.Drawing.Point(70, 18);
            this.neteaseCachePath.Name = "neteaseCachePath";
            this.neteaseCachePath.Size = new System.Drawing.Size(688, 21);
            this.neteaseCachePath.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.tempPath);
            this.groupBox3.Controls.Add(this.saveFilename);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.savePath);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(775, 118);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "保存设置";
            // 
            // savePath
            // 
            this.savePath.Location = new System.Drawing.Point(70, 18);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(688, 21);
            this.savePath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 23);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "保存目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 47);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "文件名";
            // 
            // saveFilename
            // 
            this.saveFilename.Location = new System.Drawing.Point(70, 45);
            this.saveFilename.Name = "saveFilename";
            this.saveFilename.Size = new System.Drawing.Size(156, 21);
            this.saveFilename.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(70, 71);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(688, 21);
            this.textBox1.TabIndex = 5;
            // 
            // tempPath
            // 
            this.tempPath.AutoSize = true;
            this.tempPath.Location = new System.Drawing.Point(11, 76);
            this.tempPath.Name = "tempPath";
            this.tempPath.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tempPath.Size = new System.Drawing.Size(53, 12);
            this.tempPath.TabIndex = 4;
            this.tempPath.Text = "临时目录";
            // 
            // neteaseAutoParse
            // 
            this.neteaseAutoParse.AutoSize = true;
            this.neteaseAutoParse.Location = new System.Drawing.Point(11, 48);
            this.neteaseAutoParse.Name = "neteaseAutoParse";
            this.neteaseAutoParse.Size = new System.Drawing.Size(96, 16);
            this.neteaseAutoParse.TabIndex = 6;
            this.neteaseAutoParse.Text = "开启自动导出";
            this.neteaseAutoParse.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.Text = "Config";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
            this.Load += new System.EventHandler(this.Config_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox neteaseCachePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox saveFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox savePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label tempPath;
        private System.Windows.Forms.CheckBox neteaseAutoParse;
    }
}