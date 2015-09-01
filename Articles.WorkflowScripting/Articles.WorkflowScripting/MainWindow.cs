using System;
using System.Activities;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Articles.WorkflowScripting.Gui;
using Common;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Drawing3D.Resources;
using Common.GraphicsEngine.Objects;
using Common.GraphicsEngine.Objects.Construction;
using Common.GraphicsEngine.Objects.ObjectTypes;

namespace Articles.WorkflowScripting
{
    public partial class MainWindow : Form
    {
        private Dictionary<Type, Activity> m_activityCache;
        private MainWindowDataSource m_viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //Create datasource
            m_activityCache = new Dictionary<Type, Activity>();
            m_viewModel = new MainWindowDataSource();
            m_bindingSource.DataSource = m_viewModel;
        }

        /// <summary>
        /// Executes the given activity.
        /// </summary>
        /// <param name="name">Name of the activity.</param>
        /// <param name="myActivity">The activity to execute.</param>
        private void ExecuteActivity(string name, Activity myActivity)
        {
            Dictionary<string, object> inputArgumetns = new Dictionary<string,object>();
            PropertyInfo propertyInfo = myActivity.GetType().GetProperty("Scene");
            if (propertyInfo != null)
            {
                inputArgumetns["Scene"] = m_view3D.Scene;
            }

            WorkflowApplication workflowApplication = new WorkflowApplication(myActivity, inputArgumetns);
            workflowApplication.Extensions.Add<Scene>(() => m_view3D.Scene);
            workflowApplication.SynchronizationContext = SynchronizationContext.Current;

            RunningWorkflowInformation wfInfo = new RunningWorkflowInformation(name, workflowApplication);
            m_viewModel.RunningWorkflows.Add(wfInfo);

            workflowApplication.Completed = (eArgs) => m_viewModel.RunningWorkflows.Remove(wfInfo);
            workflowApplication.Aborted = (eArgs) => m_viewModel.RunningWorkflows.Remove(wfInfo);
            workflowApplication.Unloaded = (eArgs) => m_viewModel.RunningWorkflows.Remove(wfInfo);
            workflowApplication.OnUnhandledException = (eArgs) =>
            {
                Application.OnThreadException(eArgs.UnhandledException);
                return UnhandledExceptionAction.Terminate;
            };

            workflowApplication.Run();
        }

        /// <summary>
        /// Clears the scene.
        /// </summary>
        private void ClearScene()
        {
            //Clear the scene
            m_view3D.Scene.Clear(false);

            //Adds the background texture
            AddBackgroundToScene();

            //Initialize camera
            m_view3D.Camera.Position = new Vector3(-14.24f, 9f, 7f);
            m_view3D.Camera.TargetRotation = new Vector2(-0.49f, -0.53f);
        }

        /// <summary>
        /// Adds some blur effect to the scene.
        /// </summary>
        private void AddBlurToScene()
        {
            CopiedTextureResource textureResource = new CopiedTextureResource("InnerScene", m_view3D.BackBufferSource);
            m_view3D.Scene.Resources.AddAndLoadResource(textureResource);

            TexturePainter texturePainter = new TexturePainter("InnerScene");
            texturePainter.Scaling = 1.01f;
            m_view3D.Scene.Add(texturePainter);

            //Get earth texture
            BitmapTextureResource earthTexture = new BitmapTextureResource("Texture", Properties.Resources.TextureEarth);
            BitmapTextureResource earthHeightMapTexture = new BitmapTextureResource("TextureHeightMap", Properties.Resources.TextureEarthHighMap);
            m_view3D.Scene.Resources.AddAndLoadResource(earthTexture);
            m_view3D.Scene.Resources.AddAndLoadResource(earthHeightMapTexture);

            //Create and add all cubes
            VertexStructure vertexStructure = new VertexStructure();
            vertexStructure.BuildShpere(50, 50, 5f);

            SimpleStructuredObject structuredObject = new SimpleStructuredObject("Texture", "TextureHeightMap", 0.2f, vertexStructure, m_view3D.Camera);
            structuredObject.DisplacementFactor = 1.5f;
            m_view3D.Scene.Add(structuredObject);
        }

        /// <summary>
        /// Add ground object to the scene.
        /// </summary>
        private void AddGroundToScene()
        {
            //Define all needed resources
            m_view3D.Scene.Resources.AddResource(new BitmapTextureResource("GroundTexture", Properties.Resources.TextureGround));
            m_view3D.Scene.Resources.AddResource(new DrawingBrushTextureResource("GroundBorderTexture", new LinearGradientBrush(
                new Point(0, 0),
                new Point(32, 32),
                Color.Gray,
                Color.DarkGray), 32, 32));

            m_view3D.Scene.Resources.AddResource(new SimpleColoredMaterialResource("GroundMaterial", "GroundTexture"));
            m_view3D.Scene.Resources.AddResource(new SimpleColoredMaterialResource("GroundBorderMaterial", "GroundBorderTexture"));

            GroundType groundType = new GroundType(new Vector2(5f, 5f), 0f);
            groundType.BorderMaterial = "GroundBorderMaterial";
            groundType.BottomMaterial = "GroundMaterial";
            groundType.DefaultGroundMaterial = "GroundMaterial";
            groundType.SideMaterial = "GroundMaterial";
            groundType.SetTilemap(8, 8);
            m_view3D.Scene.Resources.AddResource(new GeometryResource("GroundGeometry", groundType));

            //Define 3D object
            GenericObject groundObject = new GenericObject("GroundGeometry");
            m_view3D.Scene.Add(groundObject);
        }

        /// <summary>
        /// Adds the background texture.
        /// </summary>
        private void AddBackgroundToScene()
        {
            LinearGradientBrush gradientBrush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(32, 32),
                Color.FromArgb(200, 225, 255),
                Color.FromArgb(0x43, 0x72, 0xFB));
            DrawingBrushTextureResource drawingTexture = new DrawingBrushTextureResource("BackgroundTexture", gradientBrush, 32, 32);
            m_view3D.Scene.Resources.AddResource(drawingTexture);

            //Add background object
            TexturePainter texturePainter = new TexturePainter("BackgroundTexture", 1.5f);
            m_view3D.Scene.Add(texturePainter);

            //Add coordinate structure
            ObjectType coordType = new CoordinateAxesType("CoordMaterial");
            m_view3D.Scene.Resources.AddAndLoadResource(new SimpleColoredMaterialResource("CoordMaterial"));
            m_view3D.Scene.Resources.AddAndLoadResource(new GeometryResource("CoordGeometry", coordType));
            m_view3D.Scene.Add(new GenericObject("CoordGeometry"));
        }

        /// <summary>
        /// Loads all 3d models.
        /// </summary>
        private void Load3DModels()
        {
            //Define load method
            Func<string, GeometryResource> loadResourceAction = (resourcePath) =>
            {
                using (FileStream inStream = File.OpenRead(resourcePath))
                {
                    return new GeometryResource(Path.GetFileName(resourcePath), ObjectType.FromACFile(inStream));
                }
            };

            //Load all resources (hard coded..)
            List<GeometryResource> geoResource = new List<GeometryResource>();
            foreach (string actFileName in Directory.GetFiles("Resources"))
            {
                geoResource.Add(loadResourceAction(actFileName));
            }

            //Add all loaded resources to the scene
            geoResource.ForEach((actResource) => m_view3D.Scene.Resources.AddAndLoadResource(actResource));
        }

        /// <summary>
        /// Called when this window is opened.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (this.DesignMode) { return; }

            //Load all 3d models
            Load3DModels();
        }

        /// <summary>
        /// Standard load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Cancel when in windows forms designer
            if (this.DesignMode) { return; }

            //Build workflow menu (search all workflows using reflection)
            IEnumerable<Type> workflowTypes =
                (  from actType in Assembly.GetExecutingAssembly().GetTypes()
                  where actType.FullName.StartsWith("Articles.WorkflowScripting.Workflow.Scripts") &&
                        actType.IsSubclassOf(typeof(Activity))
                 select actType)
                 .OrderBy((t) => t.Name);
            int actIndex = 1;
            foreach (Type actType in workflowTypes)
            {
                Type workflowType = actType;
                ToolStripMenuItem addedItem = m_mnuWorkflows.DropDownItems.Add(
                    actType.Name, 
                    Properties.Resources.IconCogExecute16x16,
                    (sender, eArgs) => OnWorkflowItemClick(workflowType)) as ToolStripMenuItem;
                addedItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Shortcut), "Alt" + actIndex);

                actIndex++;
            }

            //Set scene to initial state
            ClearScene();
            AddGroundToScene();
        }

        /// <summary>
        /// Called when user wants to change current scene.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuScenePlainClick(object sender, EventArgs e)
        {
            ClearScene();
            AddGroundToScene();
        }

        /// <summary>
        /// Called when user wants to change current scene.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuSceneMotionBlurClick(object sender, EventArgs e)
        {
            ClearScene();
            AddBlurToScene();
        }

        /// <summary>
        /// Called when user wants to execute a workflow.
        /// </summary>
        /// <param name="workflowType">Type of the workflow to execute.</param>
        private void OnWorkflowItemClick(Type workflowType)
        {
            //Create Activitiy or get it from the cache
            Activity myActivity = null;
            if (m_activityCache.ContainsKey(workflowType)) { myActivity = m_activityCache[workflowType]; }
            else
            {
                myActivity = Activator.CreateInstance(workflowType) as Activity;
                m_activityCache[workflowType] = myActivity;
            }

            //Create and execute the workflow
            if (myActivity != null)
            {
                ExecuteActivity(workflowType.Name, myActivity);
            }
        }

        /// <summary>
        /// Called when user wants to close the application.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnMnuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Called when the refresh timer ticks.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnRefreshTimerTick(object sender, EventArgs e)
        {
            m_bindingSource.ResetBindings(false);

            m_lblRunningWokflow.Text = "Running Workflows (" + m_viewModel.RunningWorkflows.Count + ")";
        }
    }
}
