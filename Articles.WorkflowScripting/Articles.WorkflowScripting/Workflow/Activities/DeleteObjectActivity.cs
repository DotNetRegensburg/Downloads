using System.Activities;
using System.ComponentModel;
using System.Drawing;
using Common.GraphicsEngine.Drawing3D;

namespace Articles.WorkflowScripting.Workflow.Activities
{
    [ToolboxItem(true)]
    [Designer(typeof(DeleteObjectActivityDesigner))]
    [ToolboxBitmap(typeof(CreateObjectActivity))]
    public class DeleteObjectActivity : CodeActivity
    {
        /// <summary>
        /// The object to be deleted.
        /// </summary>
        [Category("Input")]
        [Description("The object to be deleted.")]
        [RequiredArgument]
        public InArgument<SceneSpacialObject> TargetObject { get; set; }

        /// <summary>
        /// Executes the CodeActivity.
        /// </summary>
        protected override void Execute(CodeActivityContext context)
        {
            Scene scene = context.GetExtension<Scene>();
            scene.Remove(context.GetValue(TargetObject));
        }
    }
}
