namespace ImageOperations
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.loadImageButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.setInitialImageButton = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.applyEffectButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(463, 252);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // loadImageButton
            // 
            this.loadImageButton.Location = new System.Drawing.Point(12, 270);
            this.loadImageButton.Name = "loadImageButton";
            this.loadImageButton.Size = new System.Drawing.Size(148, 27);
            this.loadImageButton.TabIndex = 1;
            this.loadImageButton.Text = "Выбрать изображение";
            this.loadImageButton.UseVisualStyleBackColor = true;
            this.loadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // setInitialImageButton
            // 
            this.setInitialImageButton.Location = new System.Drawing.Point(320, 270);
            this.setInitialImageButton.Name = "setInitialImageButton";
            this.setInitialImageButton.Size = new System.Drawing.Size(147, 27);
            this.setInitialImageButton.TabIndex = 2;
            this.setInitialImageButton.Text = "Исходное изображение";
            this.setInitialImageButton.UseVisualStyleBackColor = true;
            this.setInitialImageButton.Click += new System.EventHandler(this.setInitialImageButton_Click);
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Transparent;
            this.chart.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(12, 341);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            series1.LabelForeColor = System.Drawing.Color.Transparent;
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(463, 249);
            this.chart.TabIndex = 4;
            this.chart.Text = "chart1";
            // 
            // applyEffectButton
            // 
            this.applyEffectButton.Location = new System.Drawing.Point(166, 270);
            this.applyEffectButton.Name = "applyEffectButton";
            this.applyEffectButton.Size = new System.Drawing.Size(148, 27);
            this.applyEffectButton.TabIndex = 5;
            this.applyEffectButton.Text = "Применить эффект";
            this.applyEffectButton.UseVisualStyleBackColor = true;
            this.applyEffectButton.Click += new System.EventHandler(this.applyEffectButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 616);
            this.Controls.Add(this.applyEffectButton);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.setInitialImageButton);
            this.Controls.Add(this.loadImageButton);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.chart)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button applyEffectButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Button setInitialImageButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button loadImageButton;
        private System.Windows.Forms.PictureBox pictureBox;

        #endregion
    }
}