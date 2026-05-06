namespace ThreadTestApp
{
    partial class ThreadTestApp
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnThreadStart = new System.Windows.Forms.Button();
            this.btnTaskStart = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnTaskStart2 = new System.Windows.Forms.Button();
            this.btnThreadStart2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnThreadStart
            // 
            this.btnThreadStart.Location = new System.Drawing.Point(13, 13);
            this.btnThreadStart.Name = "btnThreadStart";
            this.btnThreadStart.Size = new System.Drawing.Size(222, 23);
            this.btnThreadStart.TabIndex = 0;
            this.btnThreadStart.Text = "Raw Thread Start";
            this.btnThreadStart.UseVisualStyleBackColor = true;
            this.btnThreadStart.Click += new System.EventHandler(this.btnThreadStart_Click);
            // 
            // btnTaskStart
            // 
            this.btnTaskStart.Location = new System.Drawing.Point(13, 42);
            this.btnTaskStart.Name = "btnTaskStart";
            this.btnTaskStart.Size = new System.Drawing.Size(222, 23);
            this.btnTaskStart.TabIndex = 0;
            this.btnTaskStart.Text = "Task Start";
            this.btnTaskStart.UseVisualStyleBackColor = true;
            this.btnTaskStart.Click += new System.EventHandler(this.btnTaskStart_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(13, 219);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(622, 22);
            this.txtMessage.TabIndex = 1;
            // 
            // btnTaskStart2
            // 
            this.btnTaskStart2.Location = new System.Drawing.Point(13, 71);
            this.btnTaskStart2.Name = "btnTaskStart2";
            this.btnTaskStart2.Size = new System.Drawing.Size(222, 23);
            this.btnTaskStart2.TabIndex = 0;
            this.btnTaskStart2.Text = "Task Start2";
            this.btnTaskStart2.UseVisualStyleBackColor = true;
            this.btnTaskStart2.Click += new System.EventHandler(this.btnTaskStart2_Click);
            // 
            // btnThreadStart2
            // 
            this.btnThreadStart2.Location = new System.Drawing.Point(241, 13);
            this.btnThreadStart2.Name = "btnThreadStart2";
            this.btnThreadStart2.Size = new System.Drawing.Size(222, 23);
            this.btnThreadStart2.TabIndex = 2;
            this.btnThreadStart2.Text = "Raw Thread Start2";
            this.btnThreadStart2.UseVisualStyleBackColor = true;
            this.btnThreadStart2.Click += new System.EventHandler(this.btnThreadStart2_Click);
            // 
            // ThreadTestApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 253);
            this.Controls.Add(this.btnThreadStart2);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnTaskStart2);
            this.Controls.Add(this.btnTaskStart);
            this.Controls.Add(this.btnThreadStart);
            this.Name = "ThreadTestApp";
            this.Text = "TreadTestApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnThreadStart;
        private System.Windows.Forms.Button btnTaskStart;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnTaskStart2;
        private System.Windows.Forms.Button btnThreadStart2;
    }
}

