﻿namespace TestWinApp {
    partial class TestForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.RedBrushBtn = new System.Windows.Forms.Button();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.PatBrushBtn = new System.Windows.Forms.Button();
            this.StretchBrushBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RedBrushBtn
            // 
            this.RedBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RedBrushBtn.Location = new System.Drawing.Point(713, 386);
            this.RedBrushBtn.Name = "RedBrushBtn";
            this.RedBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.RedBrushBtn.TabIndex = 0;
            this.RedBrushBtn.Text = "Red 50%";
            this.RedBrushBtn.UseVisualStyleBackColor = true;
            this.RedBrushBtn.Click += new System.EventHandler(this.RedBrushBtn_Click);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearBtn.Location = new System.Drawing.Point(713, 415);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 1;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // PatBrushBtn
            // 
            this.PatBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PatBrushBtn.Location = new System.Drawing.Point(713, 357);
            this.PatBrushBtn.Name = "PatBrushBtn";
            this.PatBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.PatBrushBtn.TabIndex = 2;
            this.PatBrushBtn.Text = "Tr. Pattern";
            this.PatBrushBtn.UseVisualStyleBackColor = true;
            this.PatBrushBtn.Click += new System.EventHandler(this.PatBrushBtn_Click);
            // 
            // StretchBrushBtn
            // 
            this.StretchBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StretchBrushBtn.Location = new System.Drawing.Point(713, 328);
            this.StretchBrushBtn.Name = "StretchBrushBtn";
            this.StretchBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.StretchBrushBtn.TabIndex = 3;
            this.StretchBrushBtn.Text = "Tr. Stretch";
            this.StretchBrushBtn.UseVisualStyleBackColor = true;
            this.StretchBrushBtn.Click += new System.EventHandler(this.StretchBrushBtn_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StretchBrushBtn);
            this.Controls.Add(this.PatBrushBtn);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.RedBrushBtn);
            this.Name = "TestForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TestForm_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TestForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TestForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TestForm_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RedBrushBtn;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button PatBrushBtn;
        private System.Windows.Forms.Button StretchBrushBtn;
    }
}

