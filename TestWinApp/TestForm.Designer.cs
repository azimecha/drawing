namespace TestWinApp {
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
            this.FitVertBrushBtn = new System.Windows.Forms.Button();
            this.FillVertBrushBtn = new System.Windows.Forms.Button();
            this.FillHorizBrushBtn = new System.Windows.Forms.Button();
            this.FitHorizBrushBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.SaveImageDlg = new System.Windows.Forms.SaveFileDialog();
            this.BitmapFontBtn = new System.Windows.Forms.Button();
            this.TruetypeFontBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RedBrushBtn
            // 
            this.RedBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RedBrushBtn.Location = new System.Drawing.Point(713, 415);
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
            this.ClearBtn.Location = new System.Drawing.Point(632, 386);
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
            this.PatBrushBtn.Location = new System.Drawing.Point(713, 386);
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
            this.StretchBrushBtn.Location = new System.Drawing.Point(713, 357);
            this.StretchBrushBtn.Name = "StretchBrushBtn";
            this.StretchBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.StretchBrushBtn.TabIndex = 3;
            this.StretchBrushBtn.Text = "Tr. Stretch";
            this.StretchBrushBtn.UseVisualStyleBackColor = true;
            this.StretchBrushBtn.Click += new System.EventHandler(this.StretchBrushBtn_Click);
            // 
            // FitVertBrushBtn
            // 
            this.FitVertBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FitVertBrushBtn.Location = new System.Drawing.Point(713, 328);
            this.FitVertBrushBtn.Name = "FitVertBrushBtn";
            this.FitVertBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.FitVertBrushBtn.TabIndex = 4;
            this.FitVertBrushBtn.Text = "Tr. Fit V";
            this.FitVertBrushBtn.UseVisualStyleBackColor = true;
            this.FitVertBrushBtn.Click += new System.EventHandler(this.FitVertBrushBtn_Click);
            // 
            // FillVertBrushBtn
            // 
            this.FillVertBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FillVertBrushBtn.Location = new System.Drawing.Point(713, 299);
            this.FillVertBrushBtn.Name = "FillVertBrushBtn";
            this.FillVertBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.FillVertBrushBtn.TabIndex = 5;
            this.FillVertBrushBtn.Text = "Op. Fill V";
            this.FillVertBrushBtn.UseVisualStyleBackColor = true;
            this.FillVertBrushBtn.Click += new System.EventHandler(this.FillVertBrushBtn_Click);
            // 
            // FillHorizBrushBtn
            // 
            this.FillHorizBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FillHorizBrushBtn.Location = new System.Drawing.Point(713, 241);
            this.FillHorizBrushBtn.Name = "FillHorizBrushBtn";
            this.FillHorizBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.FillHorizBrushBtn.TabIndex = 7;
            this.FillHorizBrushBtn.Text = "Op. Fill H";
            this.FillHorizBrushBtn.UseVisualStyleBackColor = true;
            this.FillHorizBrushBtn.Click += new System.EventHandler(this.FillHorizBrushBtn_Click);
            // 
            // FitHorizBrushBtn
            // 
            this.FitHorizBrushBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FitHorizBrushBtn.Location = new System.Drawing.Point(713, 270);
            this.FitHorizBrushBtn.Name = "FitHorizBrushBtn";
            this.FitHorizBrushBtn.Size = new System.Drawing.Size(75, 23);
            this.FitHorizBrushBtn.TabIndex = 6;
            this.FitHorizBrushBtn.Text = "Op. Fit H";
            this.FitHorizBrushBtn.UseVisualStyleBackColor = true;
            this.FitHorizBrushBtn.Click += new System.EventHandler(this.FitHorizBrushBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBtn.Location = new System.Drawing.Point(632, 415);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 8;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // SaveImageDlg
            // 
            this.SaveImageDlg.Filter = "Raw data|*.raw";
            this.SaveImageDlg.Title = "Save image";
            // 
            // BitmapFontBtn
            // 
            this.BitmapFontBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BitmapFontBtn.Location = new System.Drawing.Point(632, 357);
            this.BitmapFontBtn.Name = "BitmapFontBtn";
            this.BitmapFontBtn.Size = new System.Drawing.Size(75, 23);
            this.BitmapFontBtn.TabIndex = 9;
            this.BitmapFontBtn.Text = "Bitmap Font";
            this.BitmapFontBtn.UseVisualStyleBackColor = true;
            this.BitmapFontBtn.Click += new System.EventHandler(this.BitmapFontBtn_Click);
            // 
            // TruetypeFontBtn
            // 
            this.TruetypeFontBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TruetypeFontBtn.Location = new System.Drawing.Point(632, 328);
            this.TruetypeFontBtn.Name = "TruetypeFontBtn";
            this.TruetypeFontBtn.Size = new System.Drawing.Size(75, 23);
            this.TruetypeFontBtn.TabIndex = 10;
            this.TruetypeFontBtn.Text = "TrueType";
            this.TruetypeFontBtn.UseVisualStyleBackColor = true;
            this.TruetypeFontBtn.Click += new System.EventHandler(this.TruetypeFontBtn_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TruetypeFontBtn);
            this.Controls.Add(this.BitmapFontBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.FillHorizBrushBtn);
            this.Controls.Add(this.FitHorizBrushBtn);
            this.Controls.Add(this.FillVertBrushBtn);
            this.Controls.Add(this.FitVertBrushBtn);
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
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TestForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RedBrushBtn;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button PatBrushBtn;
        private System.Windows.Forms.Button StretchBrushBtn;
        private System.Windows.Forms.Button FitVertBrushBtn;
        private System.Windows.Forms.Button FillVertBrushBtn;
        private System.Windows.Forms.Button FillHorizBrushBtn;
        private System.Windows.Forms.Button FitHorizBrushBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.SaveFileDialog SaveImageDlg;
        private System.Windows.Forms.Button BitmapFontBtn;
        private System.Windows.Forms.Button TruetypeFontBtn;
    }
}

