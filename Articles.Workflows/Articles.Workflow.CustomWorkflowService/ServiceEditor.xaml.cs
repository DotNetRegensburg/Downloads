using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Articles.Workflow.CustomWorkflowService
{
    /// <summary>
    /// Interaktionslogik für ServiceEditor.xaml
    /// </summary>
    public partial class ServiceEditor : UserControl
    {
        private WorkflowDesigner m_designer; 

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceEditor"/> class.
        /// </summary>
        public ServiceEditor()
        {
            InitializeComponent();

            // register metadata
            (new DesignerMetadata()).Register();

            //Load workflow designer
            ReloadWorkflowDesigner((Activity)null);
        }

        /// <summary>
        /// Saves the workflow to the given file.
        /// </summary>
        /// <param name="filePath">Path to the file to save to.</param>
        public void PerformSave(string filePath)
        {
            m_designer.Save(filePath);
        }

        /// <summary>
        /// Loads a workflow from the given path.
        /// </summary>
        public void PerformLoad(string filePath)
        {
            ////Load the activity.
            //Activity loadedActivity = null;
            //using (FileStream inStream = File.OpenRead(filePath))
            //{
            //    loadedActivity = XamlReader.Load(inStream) as Activity;
            //    if (loadedActivity == null) { throw new ApplicationException("Unable to load activity from the given file (" + filePath + ")"); }
            //}

            ////Load designer
            //ReloadWorkflowDesigner(loadedActivity);

            ReloadWorkflowDesigner(filePath);
        }

        /// <summary>
        /// Reloads the WorkflowDesigner using the given activity.
        /// </summary>
        private void ReloadWorkflowDesigner(Activity activity)
        {
            //Unload previous designer
            if (m_designer != null)
            {
                DesignerBorder.Child = null;
                PropertyBorder.Child = null;
                m_designer = null;
            }

            //Create the workflow designer
            m_designer = new WorkflowDesigner();
            if (activity != null) { m_designer.Load(activity); }
            else { m_designer.Load(new ActivityBuilder()); }
            DesignerBorder.Child = m_designer.View;
            PropertyBorder.Child = m_designer.PropertyInspectorView;
        }

        private void ReloadWorkflowDesigner(string fileName)
        {
            //Unload previous designer
            if (m_designer != null)
            {
                DesignerBorder.Child = null;
                PropertyBorder.Child = null;
                m_designer = null;
            }

            //Create the workflow designer
            m_designer = new WorkflowDesigner();
            m_designer.Load(fileName);
            DesignerBorder.Child = m_designer.View;
            PropertyBorder.Child = m_designer.PropertyInspectorView;
        }



        //Display standard bitmaps
        //*************************************************************************************************
        //private static void LoadToolboxIconsForBuiltInActivities()
        //{
        //    AttributeTableBuilder builder = new AttributeTableBuilder();

        //    Assembly sourceAssembly = Assembly.LoadFile(@"C:\RehostedDesigner_DependentDlls\Microsoft.VisualStudio.Activities.dll");

        //    System.Resources.ResourceReader resourceReader = new System.Resources.ResourceReader(
        //        sourceAssembly.GetManifestResourceStream("Microsoft.VisualStudio.Activities.Resources.resources"));

        //    foreach (Type type in typeof(System.Activities.Activity).Assembly.GetTypes().Where(t => t.Namespace == "System.Activities.Statements"))
        //    {
        //        CreateToolboxBitmapAttributeForActivity(builder, resourceReader, type);
        //    }

        //    MetadataStore.AddAttributeTable(builder.CreateTable());
        //}

        //private static void CreateToolboxBitmapAttributeForActivity(AttributeTableBuilder builder, System.Resources.ResourceReader resourceReader, Type builtInActivityType)
        //{
        //    System.Drawing.Bitmap bitmap = ExtractBitmapResource(resourceReader, builtInActivityType.IsGenericType ? builtInActivityType.Name.Split('`')[0] : builtInActivityType.Name);

        //    if (bitmap != null)
        //    {
        //        Type tbaType = typeof(System.Drawing.ToolboxBitmapAttribute);
        //        Type imageType = typeof(System.Drawing.Image);
        //        ConstructorInfo constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { imageType, imageType }, null);
        //        System.Drawing.ToolboxBitmapAttribute tba = constructor.Invoke(new object[] { bitmap, bitmap }) as System.Drawing.ToolboxBitmapAttribute;
        //        builder.AddCustomAttributes(builtInActivityType, tba);
        //    }
        //}

        //private static System.Drawing.Bitmap ExtractBitmapResource(System.Resources.ResourceReader resourceReader, string bitmapName)
        //{
        //    System.Collections.IDictionaryEnumerator dictEnum = resourceReader.GetEnumerator();

        //    System.Drawing.Bitmap bitmap = null;
        //    while (dictEnum.MoveNext())
        //    {
        //        if (String.Equals(dictEnum.Key, bitmapName))
        //        {
        //            bitmap = dictEnum.Value as System.Drawing.Bitmap;
        //            System.Drawing.Color pixel = bitmap.GetPixel(bitmap.Width - 1, 0);
        //            bitmap.MakeTransparent(pixel);

        //            break;
        //        }
        //    }

        //    return bitmap;
        //}
    }
}
