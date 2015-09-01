using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using Common;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Drawing3D.Animations;

namespace Articles.WorkflowScripting.Workflow.Activities
{
    [Designer(typeof(MoveObjectActivityDesigner))]
    [Description("Scales a 3D object by the given vector.")]
    [ToolboxBitmap(typeof(CreateObjectActivity))]
    public class ScaleObjectActivity : CodeActivity
    {
        [Category("Input")]
        [Description("The object to scale.")]
        [RequiredArgument]
        public InArgument<SceneSpacialObject> TargetObject { get; set; }

        [Category("Input")]
        [Description("The scale vector.")]
        [RequiredArgument]
        public InArgument<Vector3> Vector { get; set; }

        [Category("Input")]
        [Description("Total duration of movement.")]
        [RequiredArgument]
        public InArgument<TimeSpan> Duration { get; set; }

        /// <summary>
        /// Executes the CodeActivity.
        /// </summary>
        protected override void Execute(CodeActivityContext context)
        {
            //Get all input arguments
            SceneSpacialObject targetObject = TargetObject.Get(context);
            Vector3 scaleVector = Vector.Get(context);
            TimeSpan duration = Duration.Get(context);

            //Trigger the animation
            targetObject.BeginAnimationSequence()
                .Scale3D(scaleVector, duration)
                .Finish();
        }
    }
}
