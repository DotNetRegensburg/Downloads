using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using Common;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Objects;

namespace Articles.WorkflowScripting.Workflow.Activities
{
    [Designer(typeof(CreateObjectActivityDesigner))]
    [ToolboxBitmap(typeof(CreateObjectActivity))]
    [Description("Creates a new 3D object.")]
    public sealed class CreateObjectActivity : CodeActivity
    {
        public string MyTestParam { get; set; }

        [Category("Input")]
        [Description("The scene to modify")]
        [RequiredArgument]
        public InArgument<Scene> Scene { get; set; }

        [Category("Input")]
        [Description("Path to the resource to load")]
        public InArgument<string> ResourcePath{ get; set; }

        [Category("Input")]
        [Description("Location where to set the created object")]
        public InArgument<Vector3> Position { get; set; }

        [Category("Input")]
        [Description("Type of the resource to load")]
        [RequiredArgument]
        public ResourceType ResourceType { get; set; }

        [Category("Output")]
        public OutArgument<SceneSpacialObject> CreatedObject { get; set; }

        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">The context of execution.</param>
        protected override void Execute(CodeActivityContext context)
        {
            string resourcePath = ResourcePath.Get(context);
            Scene scene = context.GetExtension<Scene>();

            //Check arguments
            if (scene == null) { throw new ApplicationException("Scene not set!"); }
            if (string.IsNullOrEmpty(resourcePath)) { throw new ApplicationException("Resource path not set!"); }

            //Create the object
            SceneSpacialObject newObject = null;
            switch (ResourceType)
            {
                //Load the object from a file 
                case Activities.ResourceType.File:
                    string dummyResourceName = resourcePath;
                    if (!scene.Resources.ContainsResource(dummyResourceName))
                    {
                        throw new ApplicationException("Object " + resourcePath + " is not available!");
                    }
                    newObject = new GenericObject(dummyResourceName);
                    scene.Add(newObject);
                    break;

                //Load the object from local resources
                case Activities.ResourceType.LocalResource:
                    break;
            }

            //Throw error if object is not created
            if (newObject == null)
            {
                throw new ApplicationException("Unknown error whiel creating the object!");
            }

            //Set initial data
            newObject.Position = Position.Get(context);

            //Append object to scene
            scene.Add(newObject);

            //Set result
            CreatedObject.Set(context, newObject);
        }
    }
}
