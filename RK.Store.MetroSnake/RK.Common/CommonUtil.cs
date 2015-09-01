using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Security;
using System.Security.Principal;
using System.Xml.Serialization;
using System.Windows;
using System.Threading;
//using Windows.Storage;

namespace RK.Common
{
    public static class CommonUtil
    {
        private const string TEMP_DOMAIN_NAME = "Temporary Domain";
        private const string KEY_ARGUMENT = "CrossAppDomain.Argument";
        private const string KEY_ACTION = "CrossAppDomain.Action";

        private static Dictionary<string, Stopwatch> s_watches;
        private static object s_watchesLock;
        //private static bool s_assemblyResolveRegistered;

        /// <summary>
        /// Initializes the <see cref="CommonUtil"/> class.
        /// </summary>
        static CommonUtil()
        {
            s_watches = new Dictionary<string, Stopwatch>();
            s_watchesLock = new object();
        }

#if DESKTOP
        /// <summary>
        /// Gets the attribute object attached to the given type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="sourceType"></param>
        public static T GetCustomAttribute<T>(Type sourceType)
            where T : Attribute
        {
            T result = Attribute.GetCustomAttribute(sourceType, typeof(T)) as T;
            if (result == null) { throw new InvalidOperationException("Attribute " + typeof(T).FullName + " not found on type " + sourceType.FullName + "!"); }
            return result;
        }

        /// <summary>
        /// Executes the given action after the given amount of time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="delayTime">The delay time.</param>
        public static void InvokeDelayed(Action action, TimeSpan delayTime)
        {
            //Gets current Synchronization context
            SynchronizationContext syncContext = SynchronizationContext.Current;
            if (syncContext == null)
            {
                throw new InvalidOperationException("No SynchronizationContext is available on current thread!");
            }
            if (syncContext.GetType() == typeof(SynchronizationContext))
            {
                throw new InvalidOperationException("This method is not available on default synchronization context!");
            }

            //Start a timer object wich fires an event after delay time
            System.Threading.Timer delayTimer = null;
            delayTimer = new System.Threading.Timer(
                new TimerCallback((arg) =>
                {
                    //Stop the delay timer
                    delayTimer.Dispose();

                    //Check if the control is still available
                    syncContext.Send(new SendOrPostCallback((innerArg) =>
                    {
                        //Handle exception by global exception handler
                        action();
                    }), null);
                }),
                null,
                delayTime,
                TimeSpan.FromDays(5));
        }
#endif

        /// <summary>
        /// Mixes the given colors and returns the result.
        /// </summary>
        /// <param name="leftOne">One of the colors to mix.</param>
        /// <param name="rightOne">One of the colors to mix.</param>
        public static Color4 MixColors(Color4 leftOne, Color4 rightOne)
        {
            return new Color4(
                (leftOne.R + rightOne.R) / 2,
                (leftOne.G + rightOne.G) / 2,
                (leftOne.B + rightOne.B) / 2,
                (leftOne.A + rightOne.A) / 2);
        }

        /// <summary>
        /// Performs the given action only within debug builds.
        /// </summary>
        /// <param name="debugAction">The action to perform</param>
        [Conditional("DEBUG")]
        public static void PerformDebugAction(Action debugAction)
        {
            if (debugAction != null)
            {
                debugAction();
            }
        }

        /// <summary>
        /// Gets the name of the member defined in given expression.
        /// Pass something like () => this.m_testMember to get the name "m_testMember".
        /// </summary>
        /// <param name="getMemberAction">Action for getting the member.</param>
        public static string GetMemberName(Expression<Func<object>> getMemberAction)
        {
            MemberExpression memberExpression = getMemberAction.Body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }
            else
            {
                throw new ArgumentException("Body of given expression is not a MemberExpression!");
            }
        }

        /// <summary>
        /// Starts watching using the given key.
        /// </summary>
        public static IDisposable StartTimeWatching(string key)
        {
            lock (s_watchesLock)
            {
                if (s_watches.ContainsKey(key)) { throw new ArgumentException("There is already a watch running with the given key!", "key"); }

                //Create a dummy disposable object so it can be used within a using-construction
                DummyDisposable result = new DummyDisposable(() =>
                {
                    lock (s_watchesLock)
                    {
                        if (s_watches.ContainsKey(key)) { s_watches.Remove(key); }
                    }
                });

                //Create the watch
                Stopwatch newWatch = new Stopwatch();
                s_watches[key] = newWatch;
                newWatch.Start();

                return result;
            }
        }

        /// <summary>
        /// Stops watching for the given key.
        /// </summary>
        public static TimeSpan StopTimeWatching(string key)
        {
            lock (s_watchesLock)
            {
                if (!s_watches.ContainsKey(key)) { throw new ArgumentException("There is no watch running with the given key!", "key"); }

                //Stop the watch with the given key
                Stopwatch watch = null;
                watch = s_watches[key];
                watch.Stop();
                s_watches.Remove(key);

                return watch.Elapsed;
            }
        }

        /// <summary>
        /// Creates a new EqualityComparer using the given compare and hash function.
        /// </summary>
        public static IEqualityComparer<T> CreateEqualityComparer<T>(Func<T, T, bool> compareFunction, Func<T, int> hashFunction)
        {
            return new GenericEqualityComparer<T>(compareFunction, hashFunction);
        }

        /// <summary>
        /// Ensures that the given string is not a null value.
        /// </summary>
        public static string EnsureNotNull(string value)
        {
            if (value == null) { return string.Empty; }
            else { return value; }
        }

        /// <summary>
        /// Ensures that the given array is not a null value.
        /// </summary>
        public static T[] EnsureNotNull<T>(T[] array)
        {
            if (array == null) { return new T[0]; }
            else { return array; }
        }

        /// <summary>
        /// Ensures that the given list is not a null value.
        /// </summary>
        public static List<T> EnsureNotNull<T>(List<T> list)
        {
            if (list == null) { return new List<T>(); }
            else { return list; }
        }

        /// <summary>
        /// Clones the given object using xml serialization.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="toClone">The object to clone.</param>
        public static T CloneUsingXmlSerialization<T>(T toClone)
            where T : class
        {
            if (toClone == null) { throw new ArgumentNullException("toClone"); }

            StringBuilder xmlString = new StringBuilder();
            SerializeToString(toClone, xmlString);
            T result = DeserializeFromString<T>(xmlString);
            if (result == null) { throw new CommonLibraryException("Unable to clone the object: Deserialization returned null!"); }

            return result;
        }

        /// <summary>
        /// Serializes the given object into the given StringBuilder object.
        /// </summary>
        /// <param name="toSerialize">The object to serialize.</param>
        /// <param name="result">The object to write the result in.</param>
        public static void SerializeToString(object toSerialize, StringBuilder result)
        {
            SerializeToString(toSerialize, result, string.Empty);
        }

        /// <summary>
        /// Serializes the given object into the given StringBuilder object.
        /// </summary>
        /// <param name="toSerialize">The object to serialize.</param>
        /// <param name="result">The object to write the result in.</param>
        /// <param name="defaultNamespace">The default namespace to use.</param>
        public static void SerializeToString(object toSerialize, StringBuilder result, string defaultNamespace)
        {
            if (toSerialize == null) { throw new ArgumentNullException("toSerialize"); }
            if (result == null) { throw new ArgumentNullException("result"); }

            StringWriter stringWriter = null;
            try
            {
                //Create the serializer
                XmlSerializer serializer = null;
                if (string.IsNullOrEmpty(defaultNamespace)) { serializer = new XmlSerializer(toSerialize.GetType()); }
                else { serializer = new XmlSerializer(toSerialize.GetType(), defaultNamespace); }

                //Serialize into local memory stream
                stringWriter = new StringWriter(result);
                serializer.Serialize(stringWriter, toSerialize);
            }
            finally
            {
                if (stringWriter != null) { stringWriter.Dispose(); }
            }
        }

        /// <summary>
        /// Deserializes an object from the StringBuilder object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="stringBuilder">The source of the deserialization.</param>
        public static T DeserializeFromString<T>(StringBuilder stringBuilder)
            where T : class
        {
            return DeserializeFromString<T>(stringBuilder, string.Empty);
        }

        /// <summary>
        /// Deserializes an object from the StringBuilder object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="stringBuilder">The source of the deserialization.</param>
        /// <param name="defaultNamespace">Standard namespace for xml-file.</param>
        public static T DeserializeFromString<T>(StringBuilder stringBuilder, string defaultNamespace)
            where T : class
        {
            if (stringBuilder == null) { throw new ArgumentNullException("stringBuilder"); }

            Type genericType = typeof(T);
            StringReader stringReader = null;
            try
            {
                //Create the serializer
                XmlSerializer serializer = null;
                if (string.IsNullOrEmpty(defaultNamespace)) { serializer = new XmlSerializer(genericType); }
                else { serializer = new XmlSerializer(genericType, defaultNamespace); }

                //Open the file for reading
                stringReader = new StringReader(stringBuilder.ToString());

                return serializer.Deserialize(stringReader) as T;
            }
            finally
            {
                if (stringReader != null) { stringReader.Dispose(); }
            }
        }

        ///// <summary>
        ///// Serializes the given object into the given file using xml serialization.
        ///// </summary>
        ///// <param name="toSerialize">Object to serialize.</param>
        ///// <param name="fileName">Target file path.</param>
        //public static void SerializeToFile(object toSerialize, string fileName)
        //{
        //    SerializeToFile(toSerialize, fileName, string.Empty, false, false);
        //}

        ///// <summary>
        ///// Serializes the given object into the given file using xml serialization.
        ///// </summary>
        ///// <param name="toSerialize">Object to serialize.</param>
        ///// <param name="fileName">Target file path.</param>
        ///// <param name="defaultNamespace">Standard namespace for xml-file.</param>
        //public static void SerializeToFile(object toSerialize, string fileName, string defaultNamespace)
        //{
        //    SerializeToFile(toSerialize, fileName, defaultNamespace, false, false);
        //}

        ///// <summary>
        ///// Serializes the given object into the given file using xml serialization.
        ///// </summary>
        ///// <param name="toSerialize">Object to serialize.</param>
        ///// <param name="fileName">Target file path.</param>
        ///// <param name="defaultNamespace">Standard namespace for xml-file.</param>
        ///// <param name="expandRights">Expand file rights after serialization?</param>
        //public static void SerializeToFile(object toSerialize, string fileName, string defaultNamespace, bool createDirectory, bool expandRights)
        //{
        //    if (toSerialize == null) { throw new ArgumentNullException("toSerialize"); }
        //    if (string.IsNullOrEmpty(fileName)) { throw new ArgumentNullException("fileName"); }

        //    MemoryStream outStream = null;
        //    try
        //    {
        //        //Create target directory 
        //        if (createDirectory)
        //        {
        //            string directory = Path.GetDirectoryName(fileName);
        //            if (!Directory.Exists(directory))
        //            {
        //                Directory.CreateDirectory(directory);
        //                if (expandRights) { ExpandDirectoryRights(directory); }
        //            }
        //        }

        //        //Create the serializer
        //        XmlSerializer serializer = null;
        //        if (string.IsNullOrEmpty(defaultNamespace)) { serializer = new XmlSerializer(toSerialize.GetType()); }
        //        else { serializer = new XmlSerializer(toSerialize.GetType(), defaultNamespace); }

        //        //Serialize into local memory stream
        //        outStream = new MemoryStream();
        //        serializer.Serialize(outStream, toSerialize);

        //        //Write all bytes to the file
        //        File.WriteAllBytes(fileName, outStream.ToArray());

        //        //Expand file rights if specified
        //        if (expandRights && (!FileRightsExpanded(fileName)))
        //        {
        //            ExpandFileRights(fileName);
        //        }
        //    }
        //    finally
        //    {
        //        if (outStream != null) { outStream.Dispose(); }
        //    }
        //}

        ///// <summary>
        ///// Deserializes an object from the given file.
        ///// </summary>
        ///// <typeparam name="T">Type of the object.</typeparam>
        ///// <param name="fileName">Name of the file to read from.</param>
        ///// <param name="defaultNamespace">Standard namespace for xml-file.</param>
        //public static T DeserializeFromFile<T>(string fileName, string defaultNamespace)
        //    where T : class
        //{
        //    if (string.IsNullOrEmpty(fileName)) { throw new ArgumentNullException("fileName"); }

        //    Type genericType = typeof(T);
        //    FileStream inStream = null;
        //    try
        //    {
        //        //Create the serializer
        //        XmlSerializer serializer = null;
        //        if (string.IsNullOrEmpty(defaultNamespace)) { serializer = new XmlSerializer(genericType); }
        //        else { serializer = new XmlSerializer(genericType, defaultNamespace); }

        //        //Open the file for reading
        //        inStream = File.OpenRead(fileName);

        //        return serializer.Deserialize(inStream) as T;
        //    }
        //    finally
        //    {
        //        if (inStream != null) { inStream.Close(); }
        //    }
        //}

        ///// <summary>
        ///// Gets the name of the application.
        ///// </summary>
        //public static string GetApplicationName()
        //{
        //    Assembly mainAssembly = Assembly.GetEntryAssembly();
        //    if (mainAssembly != null)
        //    {
        //        AssemblyTitleAttribute attrib = Attribute.GetCustomAttribute(mainAssembly, typeof(AssemblyTitleAttribute))
        //            as AssemblyTitleAttribute;
        //        if (attrib != null)
        //        {
        //            return attrib.Title;
        //        }
        //    }
        //    return string.Empty;
        //}

        ///// <summary>
        ///// Gets the RK.Common application path for the application name provided by the first assembly of this domain.
        ///// </summary>
        //public static string GetCommonApplicationPath()
        //{
        //    string appName = GetApplicationName();
        //    if (string.IsNullOrEmpty(appName)) { throw new CommonLibraryException("Application name not found in the assemblie's properties!"); }

        //    return GetCommonApplicationPath(appName);
        //}

        ///// <summary>
        ///// Gets the RK.Common application path for the given application name.
        ///// </summary>
        ///// <param name="applicationName">Name of the application.</param>
        //public static string GetCommonApplicationPath(string applicationName)
        //{
        //    string appPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        //    return Path.Combine(appPath, applicationName);
        //}

        ///// <summary>
        ///// Gets the RK.Common application path for the current user and the application name provided by the first assembly of this domain.
        ///// </summary>
        //public static string GetCommonUserApplicationPath()
        //{
        //    string appName = GetApplicationName();
        //    if (string.IsNullOrEmpty(appName)) { throw new CommonLibraryException("Application name not found in the assemblie's properties!"); }

        //    return GetCommonUserApplicationPath(appName);
        //}

        ///// <summary>
        ///// Gets the RK.Common application path for the current user and the given application name.
        ///// </summary>
        ///// <param name="applicationName">Name of the application.</param>
        //public static string GetCommonUserApplicationPath(string applicationName)
        //{
        //    string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    return Path.Combine(appPath, applicationName);
        //}

        ///// <summary>
        ///// Gets the title of the assembly that calls this method.
        ///// </summary>
        //public static string GetAssemblyTitleFromCalling()
        //{
        //    return GetAssemblyTitle(Assembly.GetCallingAssembly());
        //}

        ///// <summary>
        ///// Gets the title of the given assembly.
        ///// </summary>
        ///// <param name="assembly">Assembly to read the title from.</param>
        //public static string GetAssemblyTitle(Assembly assembly)
        //{
        //    AssemblyTitleAttribute titleAttrib = null;

        //    try
        //    {
        //        titleAttrib = AssemblyTitleAttribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute))
        //            as AssemblyTitleAttribute;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new CommonLibraryException("Unable to read the title of the given assembly (" + assembly.FullName + ")!", ex);
        //    }

        //    if (titleAttrib != null)
        //    {
        //        return titleAttrib.Title;
        //    }
        //    else
        //    {
        //        throw new CommonLibraryException("Unable to read the title of the given assembly (" + assembly.FullName + ")!");
        //    }
        //}

        ///// <summary>
        ///// Gives all users all privileges to the given file
        ///// </summary>
        //public static void ExpandFileRights(string filePath)
        //{
        //    if (File.Exists(filePath) && (!FileRightsExpanded(filePath)))
        //    {
        //        FileInfo fInfo = new FileInfo(filePath);
        //        FileSecurity fSecurity = fInfo.GetAccessControl();

        //        IdentityReference idReference = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        //        fSecurity.AddAccessRule(new FileSystemAccessRule(
        //            idReference,
        //            FileSystemRights.FullControl,
        //            AccessControlType.Allow));
        //        fInfo.SetAccessControl(fSecurity);
        //    }
        //}

        ///// <summary>
        ///// Gives all users all privileges
        ///// </summary>
        //public static void ExpandDirectoryRights(string directoryName)
        //{
        //    if (Directory.Exists(directoryName) && (!DirectoryRightsExpanded(directoryName)))
        //    {
        //        DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
        //        DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

        //        IdentityReference idReference = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        //        dirSecurity.AddAccessRule(new FileSystemAccessRule(
        //            idReference,
        //            FileSystemRights.FullControl,
        //            AccessControlType.Allow));
        //        dirInfo.SetAccessControl(dirSecurity);
        //    }
        //}

        ///// <summary>
        ///// Are filerights expanded?
        ///// </summary>
        //public static bool FileRightsExpanded(string filePath)
        //{
        //    if (!File.Exists(filePath)) { return false; }

        //    FileInfo fInfo = new FileInfo(filePath);
        //    FileSecurity fSecurity = fInfo.GetAccessControl();

        //    IdentityReference idReference = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        //    AuthorizationRuleCollection accessRules = fSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

        //    foreach (AuthorizationRule actRule in accessRules)
        //    {
        //        FileSystemAccessRule actFileAccessRule = actRule as FileSystemAccessRule;

        //        if (actFileAccessRule != null)
        //        {
        //            if ((actRule.IdentityReference.Value == idReference.Value) &&
        //                (actFileAccessRule.FileSystemRights == FileSystemRights.FullControl) &&
        //                (actFileAccessRule.AccessControlType == AccessControlType.Allow))
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Are directory rights expanded?
        ///// </summary>
        //public static bool DirectoryRightsExpanded(string directoryPath)
        //{
        //    if (!Directory.Exists(directoryPath)) { return false; }

        //    DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
        //    DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

        //    IdentityReference idReference = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        //    AuthorizationRuleCollection accessRules = dirSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

        //    foreach (AuthorizationRule actRule in accessRules)
        //    {
        //        FileSystemAccessRule actDirAccessRule = actRule as FileSystemAccessRule;
        //        if (actDirAccessRule != null)
        //        {
        //            if ((actRule.IdentityReference.Value == idReference.Value) &&
        //                (actDirAccessRule.FileSystemRights == FileSystemRights.FullControl) &&
        //                (actDirAccessRule.AccessControlType == AccessControlType.Allow))
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Calls the given action in a separate AppDomain.
        ///// </summary>
        ///// <param name="actionToPerform">The action to perform.</param>
        //public static void CallInTemporaryAppDomain(Action actionToPerform)
        //{
        //    AppDomain appDomain = AppDomain.CreateDomain(TEMP_DOMAIN_NAME);
        //    try
        //    {
        //        //Call given action
        //        appDomain.DoCallBack(new CrossAppDomainDelegate(actionToPerform));
        //    }
        //    finally
        //    {
        //        AppDomain.Unload(appDomain);
        //    }
        //}

        ///// <summary>
        ///// Calls the given action in a separate AppDomain.
        ///// </summary>
        ///// <typeparam name="T">
        ///// Type of the argument object.
        ///// This object musst be serializable.
        ///// </typeparam>
        ///// <param name="actionToPerform">The action to perform.</param>
        ///// <param name="argument">
        ///// The argument for usage in the other domain.
        ///// This object musst be serializable.
        ///// </param>
        //public static void CallInTemporaryAppDomain<T>(Action<T> actionToPerform, T argument)
        //    where T : class
        //{
        //    AppDomain appDomain = AppDomain.CreateDomain(TEMP_DOMAIN_NAME);
        //    appDomain.SetData(KEY_ARGUMENT, argument);
        //    appDomain.SetData(KEY_ACTION, actionToPerform);
        //    try
        //    {
        //        appDomain.DoCallBack(new CrossAppDomainDelegate(() =>
        //        {
        //            //Get argument (this code runs in temporary AppDomain)
        //            T localArgument =
        //                AppDomain.CurrentDomain.GetData(KEY_ARGUMENT) as T;
        //            Action<T> localActionToPerform =
        //                AppDomain.CurrentDomain.GetData(KEY_ACTION) as Action<T>;

        //            //Call given action
        //            localActionToPerform(localArgument);
        //        }));
        //    }
        //    finally
        //    {
        //        AppDomain.Unload(appDomain);
        //    }
        //}

        /// <summary>
        /// Disposes the given object.
        /// </summary>
        /// <typeparam name="T">Type of the object to dispose.</typeparam>
        /// <param name="objectToDispose">The object to dispose.</param>
        public static T DisposeObject<T>(T objectToDispose)
            where T : class, IDisposable
        {
            if (objectToDispose == null) { return null; }

            try { objectToDispose.Dispose(); }
            catch (Exception) { }

            return null;
        }

        /// <summary>
        /// Disposes the given object.
        /// </summary>
        /// <typeparam name="T">Type of the object to dispose.</typeparam>
        /// <param name="objectToDispose">The object to dispose.</param>
        public static T DisposeObject<T>(T objectToDispose, Action<Exception> onError)
            where T : class, IDisposable
        {
            if (objectToDispose == null) { return null; }

            try { objectToDispose.Dispose(); }
            catch (Exception ex)
            {
                onError(ex);

            }
            return null;
        }

        ////*********************************************************************
        ////*********************************************************************
        ////*********************************************************************
        ///// <summary>
        ///// Wrapper class arround WindowInteropHelper that implements IWin32Window.
        ///// </summary>
        //private class InteropWindowWrapper : WinForms.IWin32Window
        //{
        //    private WindowInteropHelper m_interopHelper;

        //    /// <summary>
        //    /// Creates a new wrapper around the given interop helper.
        //    /// </summary>
        //    public InteropWindowWrapper(WindowInteropHelper interopHelper)
        //    {
        //        m_interopHelper = interopHelper;
        //    }

        //    /// <summary>
        //    /// Gets the handle of the window.
        //    /// </summary>
        //    public IntPtr Handle
        //    {
        //        get { return m_interopHelper.Handle; }
        //    }
        //}

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Just a helper class.
        /// </summary>
        private class DummyDisposable : IDisposable
        {
            private Action m_actionToPerform;

            /// <summary>
            /// Initializes a new instance of the <see cref="DummyDisposable"/> class.
            /// </summary>
            /// <param name="actionToPerform">The action to perform.</param>
            public DummyDisposable(Action actionToPerform)
            {
                m_actionToPerform = actionToPerform;
            }

            /// <summary>
            /// Disposes this object.
            /// </summary>
            public void Dispose()
            {
                m_actionToPerform();
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Helper class for CreateEqualityComparer.
        /// </summary>
        private class GenericEqualityComparer<T> : IEqualityComparer<T>
        {
            private Func<T, T, bool> m_compareFunction;
            private Func<T, int> m_hashFunction;

            public GenericEqualityComparer(Func<T, T, bool> compareFunction, Func<T, int> hashFunction)
            {
                m_compareFunction = compareFunction;
                m_hashFunction = hashFunction;
            }

            /// <summary>
            /// Checks the given objects for equality.
            /// </summary>
            public bool Equals(T x, T y)
            {
                return m_compareFunction(x, y);
            }

            /// <summary>
            /// Gets the hash code for the given object.
            /// </summary>
            public int GetHashCode(T obj)
            {
                return m_hashFunction(obj);
            }
        }
    }
}
