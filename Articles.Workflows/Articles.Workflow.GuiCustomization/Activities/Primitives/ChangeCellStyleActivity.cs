using System.Activities;
using System.ComponentModel;
using System.Drawing;
using Articles.Workflow.GuiCustomization.Activities.Data;

namespace Articles.Workflow.GuiCustomization.Activities.Primitives
{

    public sealed class ChangeCellStyleActivity : CodeActivity
    {
        [RequiredArgument]
        public InArgument<CustomCellStyle> CellStyle { get; set; }

        [Category("Style")]
        public Color BackgroundColor { get; set; }
        [Category("Style")]
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">The context of activity execution.</param>
        protected override void Execute(CodeActivityContext context)
        {
            CustomCellStyle cellStyle = context.GetValue(CellStyle);
            cellStyle.BackgroundColor = BackgroundColor;
            cellStyle.ForegroundColor = ForegroundColor;
        }
    }
}
