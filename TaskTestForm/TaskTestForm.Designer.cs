namespace TaskTest
{
    partial class TaskTestForm
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
            this.btnChainTasks = new System.Windows.Forms.Button();
            this.btnRegidentTask = new System.Windows.Forms.Button();
            this.btnSetEvent = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnChainTasks
            // 
            this.btnChainTasks.Location = new System.Drawing.Point(13, 13);
            this.btnChainTasks.Name = "btnChainTasks";
            this.btnChainTasks.Size = new System.Drawing.Size(259, 23);
            this.btnChainTasks.TabIndex = 0;
            this.btnChainTasks.Text = "Chain Tasks";
            this.btnChainTasks.UseVisualStyleBackColor = true;
            this.btnChainTasks.Click += new System.EventHandler(this.btnChainTasks_Click);
            // 
            // btnRegidentTask
            // 
            this.btnRegidentTask.Location = new System.Drawing.Point(13, 43);
            this.btnRegidentTask.Name = "btnRegidentTask";
            this.btnRegidentTask.Size = new System.Drawing.Size(124, 23);
            this.btnRegidentTask.TabIndex = 1;
            this.btnRegidentTask.Text = "PersistentTask";
            this.btnRegidentTask.UseVisualStyleBackColor = true;
            this.btnRegidentTask.Click += new System.EventHandler(this.btnRegidentTask_Click);
            // 
            // btnSetEvent
            // 
            this.btnSetEvent.Location = new System.Drawing.Point(148, 43);
            this.btnSetEvent.Name = "btnSetEvent";
            this.btnSetEvent.Size = new System.Drawing.Size(124, 23);
            this.btnSetEvent.TabIndex = 2;
            this.btnSetEvent.Text = "Set()";
            this.btnSetEvent.UseVisualStyleBackColor = true;
            this.btnSetEvent.Click += new System.EventHandler(this.btnSetEvent_Click);
            // 
            // TaskTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnSetEvent);
            this.Controls.Add(this.btnRegidentTask);
            this.Controls.Add(this.btnChainTasks);
            this.Name = "TaskTestForm";
            this.Text = "Form for Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskTestForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TaskTestForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnChainTasks;
        private System.Windows.Forms.Button btnRegidentTask;
        private System.Windows.Forms.Button btnSetEvent;
    }
}

