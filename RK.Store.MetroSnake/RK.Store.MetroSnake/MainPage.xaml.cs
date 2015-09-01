using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RK.Common;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Gui;
using RK.Common.GraphicsEngine.Objects;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RK.Common.GraphicsEngine.Animations;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Store.MetroSnake.Util;
using RK.Store.MetroSnake.Game;
using Windows.System;
using RK.Common.GraphicsEngine.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI;
using RK.Store.MetroSnake.UI;
using System.Diagnostics;
using Windows.UI.Input;
using RK.Store.MetroSnake.Game.WormBehaviors;
using Windows.UI.ViewManagement;

namespace RK.Store.MetroSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : SwapChainBackgroundPanel
    {
        private GameViewModel m_viewModel;
        private MapProperties m_mapProperties;
        private BackgroundPanelDirectXSceneView m_renderTarget;
        private bool m_gameRunning;
        private bool m_inGameLoop;
        private UIElementInputCatcher m_inputCatcher;
        private GraphicsContainer m_graphicsContainer;
        private UIGestureCatcher m_uiGestureCatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.MainContentGrid.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

            m_viewModel = new GameViewModel();
            this.DataContext = m_viewModel;

            //Define current map
            m_mapProperties = new MapProperties();
            m_mapProperties.TilesX = 10;
            m_mapProperties.TilesY = 10;

            //Load render panel
            m_renderTarget = new BackgroundPanelDirectXSceneView(this);

            m_inputCatcher = new UIElementInputCatcher(this);

            //Define all graphics resource
            InitializeGraphics();

            //Initialize game objects
            m_viewModel.Worm.Add(new WormElement(m_graphicsContainer.WormHead, 200)); 
            ExpandWorm();
            for (int loop = 0; loop < 40; loop++)
            {
                ExpandWorm();
            }

            //Relocate current gem
            RelocateGem();

            //Prepare camera
            float rotation90Deg = (float)Math.PI / 2f;
            Camera camera = m_renderTarget.Camera;
            camera.Position = new Vector3(0, 0, 0);
            camera.RelativeTarget = new Vector3(0f, 0f, 1f);
            camera.TargetRotation = new Vector2(rotation90Deg, -(float)Math.PI / 7f);
            camera.Zoom(-10f);
            camera.UpdateCamera();

            //Apply gesture capture
            m_uiGestureCatcher = new UIGestureCatcher(this);
            m_uiGestureCatcher.MoveTop += (sender, eArgs) => m_viewModel.TryTurnWormTo(WormMoveDirection.Up);
            m_uiGestureCatcher.MoveDown += (sender, eArgs) => m_viewModel.TryTurnWormTo(WormMoveDirection.Down); 
            m_uiGestureCatcher.MoveRight += (sender, eArgs) => m_viewModel.TryTurnWormTo(WormMoveDirection.Right); 
            m_uiGestureCatcher.MoveLeft += (sender, eArgs) => m_viewModel.TryTurnWormTo(WormMoveDirection.Left); 

            //Start mainloop
            m_gameRunning = true;
            this.InvokeDelayedWhile(
                () => m_gameRunning,
                () => MainLoop(),
                TimeSpan.FromMilliseconds(30.0),
                () => 
                {
                    this.MainContentGrid.Height = this.ActualHeight;
                    m_renderTarget.DiscardRendering = true;
                },
                InvokeDelayedMode.EnsuredTimerInterval);

            //this.InvokeDelayedWhile(
            //    () => m_gameRunning,
            //    () => m_viewModel.CurrentWormBehavior = new ReverseDirectionBehavior(m_viewModel),
            //    TimeSpan.FromSeconds(5.0),
            //    () => { });

            this.Loaded += (sender, eArgs) =>
            {
                //Navigate to main frame
                MainFrame.Navigate(typeof(GameStartupPage));
            };
        }

        /// <summary>
        /// Executes the main loop.
        /// </summary>
        private void MainLoop()
        {
            //Do continue with mainloop logic?
            bool doMainLoopLogic = true;
            bool raisePause = false;
            if (this.MainFrame.Content != null) { doMainLoopLogic = false; }
            if (m_inGameLoop) { doMainLoopLogic = false; }
            if (!Window.Current.Visible) { doMainLoopLogic = false; raisePause = true; }
            if (!Window.Current.CoreWindow.IsInputEnabled) { doMainLoopLogic = false; raisePause = true; }

            //Performance improvemen.. ensure that xaml area is as less as possible!
            // See best practices using SwapChainBackgroundPanel here http://msdn.microsoft.com/en-us/library/windows/apps/hh825871.aspx
            if (this.MainFrame.Content != null)
            { 
                this.MainContentGrid.Height = this.ActualHeight;
                m_renderTarget.DiscardRendering = m_renderTarget.TotalRenderCount > 2;
            }
            else
            { 
                this.MainContentGrid.Height = 100.0;
                m_renderTarget.DiscardRendering = false;
            }

            //Toolbar is only active when game runs
            this.ToolBar.IsHitTestVisible = doMainLoopLogic;

            //Cancel this loop pass when needed
            if (!doMainLoopLogic)
            {
                m_inputCatcher.Clear();
                if (raisePause){ if (this.MainFrame.Content == null) { this.MainFrame.Navigate(typeof(GamePausePage)); } }
                return;
            }

            m_inGameLoop = true;
            try
            {
                //Switch to pause state if p or pause is pressed
                if (m_inputCatcher.IsKeyDown(VirtualKey.P) ||
                    m_inputCatcher.IsKeyDown(VirtualKey.Pause))
                {
                    this.MainFrame.Navigate(typeof(GamePausePage));
                }

                //Update current worm behavior and check wheter to continue standard game logic
                bool continueWithGame = true;
                if (m_viewModel.CurrentWormBehavior != null)
                {
                    m_viewModel.CurrentWormBehavior.Update();
                    if (m_viewModel.CurrentWormBehavior.IsFinished) { m_viewModel.CurrentWormBehavior = null; }
                    else if (!m_viewModel.CurrentWormBehavior.AllowsNormalGameLogic) { continueWithGame = false; }
                }

                if (continueWithGame)
                {
                    //Capture key input
                    if (m_inputCatcher.IsKeyDown(VirtualKey.Up, VirtualKey.W)) { m_viewModel.TryTurnWormTo(WormMoveDirection.Up); }
                    else if (m_inputCatcher.IsKeyDown(VirtualKey.Right, VirtualKey.D)) {m_viewModel.TryTurnWormTo(WormMoveDirection.Right); }
                    else if (m_inputCatcher.IsKeyDown(VirtualKey.Down, VirtualKey.S)) { m_viewModel.TryTurnWormTo(WormMoveDirection.Down); }
                    else if (m_inputCatcher.IsKeyDown(VirtualKey.Left, VirtualKey.A)) { m_viewModel.TryTurnWormTo(WormMoveDirection.Left); }

                    //Move the worm
                    m_viewModel.Worm.MoveWorm(m_viewModel.LastWormMoveDirection);

                    //Handle gem collisions
                    bool wormCollidesWithGem = DoesWormCollideWithGem();
                    if (wormCollidesWithGem)
                    {
                        int gotPoints = m_graphicsContainer.GetCurrentGemModelIndex() + 1;
                        m_viewModel.Points = m_viewModel.Points + gotPoints;

                        for (int loop = 0; loop < gotPoints; loop++)
                        {
                            ExpandWorm();
                        }

                        RelocateGem();
                    }

                    //Handle map borders
                    bool wormLeavesMap = DoesWormLeaveMap();
                    if (wormLeavesMap)
                    {
                        //Try to restart the app
                        m_gameRunning = false;
                        this.MainFrame.Navigate(typeof(GameOverPage));
                    }

                    //
                    bool wormCollideItself = DoesWormCollideWithItself();
                    if (wormCollideItself)
                    {
                        //Try to restart the app
                        m_gameRunning = false;
                        this.MainFrame.Navigate(typeof(GameOverPage));
                    }
                }

                //Update camera location
                float rotation90Deg = (float)Math.PI / 2f;
                Camera camera = m_renderTarget.Camera;
                camera.Position = m_graphicsContainer.WormHead.Position;
                camera.RelativeTarget = new Vector3(0f, 0f, 1f);
                camera.TargetRotation = new Vector2(rotation90Deg, -(float)Math.PI / 7f);
                camera.Zoom(-10f);
                camera.UpdateCamera();
            }
            finally
            {
                m_inGameLoop = false;
            }
        }

        /// <summary>
        /// Expands the worm by one element.
        /// </summary>
        private void ExpandWorm()
        {
            m_viewModel.Worm.Add(new WormElement(m_graphicsContainer.CreateWormElement(m_viewModel.Worm.Last()), 100, 4));
        }

        /// <summary>
        /// Relocates the gem.
        /// </summary>
        private void RelocateGem()
        {
            Random randomizer = new Random(Environment.TickCount);
            bool relocated = false;
            while (!relocated)
            {
                Vector3 newLocation = new Vector3(
                    -4.5f + randomizer.Next(0, m_mapProperties.TilesX) * 1f,
                    0f,
                    -4.5f + randomizer.Next(0, m_mapProperties.TilesY) * 1f);
                if (!DoesWormCollideWithSphere(new BoundingSphere(newLocation, 0.4f)))
                {
                    m_graphicsContainer.Gem.Position = newLocation;
                    m_graphicsContainer.ChangeGemModel();
                    relocated = true;
                }
            }
        }

        /// <summary>
        /// Does the form collide with itself?
        /// </summary>
        public bool DoesWormCollideWithItself()
        {
            WormElement firstOne = m_viewModel.Worm.First();
            BoundingSphere mainSphere = new BoundingSphere(firstOne.Object3D.Position, 0.2f);
            int actIndex = 0;
            foreach (WormElement actWormElement in m_viewModel.Worm)
            {
                if (actIndex >= 2)
                {
                    if ((actWormElement != firstOne) &&
                        (actWormElement.LastExecutedMove != WormMoveDirection.None))
                    {
                        BoundingSphere actSphere = new BoundingSphere(actWormElement.Object3D.Position, 0.2f);
                        if (actSphere.Intersects(mainSphere)) { return true; }
                    }
                }
                actIndex++;
            }
            return false;
        }

        /// <summary>
        /// Does the worm collide with current gem?
        /// </summary>
        private bool DoesWormCollideWithGem()
        {
            return DoesWormCollideWithSphere(new BoundingSphere(
                m_graphicsContainer.Gem.Position,
                0.3f));
        }

        /// <summary>
        /// Does the worm collide with the given sphere.
        /// </summary>
        private bool DoesWormCollideWithSphere(BoundingSphere sphere)
        {
            foreach (WormElement actWormElement in m_viewModel.Worm)
            {
                BoundingSphere actWormSphere = new BoundingSphere(
                    actWormElement.Object3D.Position,
                    0.4f);
                if (actWormSphere.Intersects(sphere)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Does the worm leave the map?
        /// </summary>
        /// <returns></returns>
        private bool DoesWormLeaveMap()
        {
            foreach (WormElement actWormElement in m_viewModel.Worm)
            {
                Vector3 actElementLocation = actWormElement.Object3D.Position;
                if (Math.Abs(actElementLocation.X) > (m_mapProperties.TilesX / 2f)) { return true; }
                if (Math.Abs(actElementLocation.Z) > (m_mapProperties.TilesY / 2f)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Initializes all graphics
        /// </summary>
        private void InitializeGraphics()
        {
            m_graphicsContainer = new GraphicsContainer(m_renderTarget.Scene, m_mapProperties);

            GenericObject wormHeader = m_graphicsContainer.WormHead;
            wormHeader.RotationHV = new Vector2(2f, 0f);
            m_renderTarget.Scene.Add(wormHeader);

            m_renderTarget.Scene.PrepareRendering();
        }

        /// <summary>
        /// Called when user wants to restart the game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnCmdRestartGame(object sender, RoutedEventArgs e)
        {
            m_gameRunning = false;
            Window.Current.Content = new MainPage();
        }

        /// <summary>
        /// Called when user clicks pause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCmdPauseClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(typeof(GamePausePage));
        }
    }
}
