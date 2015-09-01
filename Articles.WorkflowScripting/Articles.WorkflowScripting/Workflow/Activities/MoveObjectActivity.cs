using System;
using System.Activities;
using System.ComponentModel;
using Common;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Drawing3D.Animations;

namespace Articles.WorkflowScripting.Workflow.Activities
{
    //[ToolboxItem(true)]
    //[ToolboxBitmap(typeof(MoveObjectActivity))]
    [Designer(typeof(MoveObjectActivityDesigner))]
    [Description("Moves a 3D object by the given vector.")]
    public class MoveObjectActivity : CodeActivity
    {
        [Category("Input")]
        [Description("The object to move.")]
        [RequiredArgument]
        public InArgument<SceneSpacialObject> TargetObject { get; set; }

        [Category("Input")]
        [Description("The move vector.")]
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
            Vector3 moveVector = Vector.Get(context);
            TimeSpan duration = Duration.Get(context);

            //Trigger the animation
            targetObject.BeginAnimationSequence()
                .Move3D(moveVector, duration)
                .Finish();
        }
    }
}
