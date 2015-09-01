using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace Articles.Workflows.CustomExtension.Activities
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(WriteItemToMainWindowActivity))]
    public class WriteItemToMainWindowActivity : CodeActivity
    {
        /// <summary>
        /// The item to add.
        /// </summary>
        [RequiredArgument]
        public InArgument<string> ItemToAdd { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            //Define required extension
            metadata.RequireExtension<IMainWindow>();
        }

        /// <summary>
        /// Executes the workflow.
        /// </summary>
        /// <param name="context">Context of execution.</param>
        protected override void Execute(CodeActivityContext context)
        {
            string itemToAdd = context.GetValue(this.ItemToAdd);
            IMainWindow mainWindow = context.GetExtension<IMainWindow>();

            if (mainWindow == null) { throw new ApplicationException("MainWindow extension is not set!"); }
            if (string.IsNullOrEmpty(itemToAdd)) { throw new ApplicationException("Not item given!"); }

            mainWindow.AddItem(itemToAdd);
        }
    }
}
