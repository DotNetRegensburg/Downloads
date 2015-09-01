using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Workflows
{
	public sealed partial class UselessWorkflow: SequentialWorkflowActivity
	{
        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }

		public UselessWorkflow()
		{
			InitializeComponent();
           
		}

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            this.Message = "I'm really useless ...";
        }
	}
}
